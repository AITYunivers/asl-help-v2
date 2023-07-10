using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public abstract IMemoryManager? Memory { get; protected set; }

    protected abstract IMemoryManager InitializeMemory(Process process, ILogger logger);

    protected virtual void DisposeMemory()
    {
        Memory?.Dispose();
        Memory = null;
    }

    protected static unsafe bool TryInjectAslCoreNative(Process process, bool is64Bit, [NotNullWhen(true)] out NamedPipeClientStream? pipe)
    {
        Debug.Info("  => Attempting to inject...");

        nuint processHandle = (nuint)(nint)process.Handle;
        uint processId = (uint)process.Id;

        string arch = is64Bit ? "x64" : "x86";
        string res = $"AslHelp.Native.{arch}.dll";

        string path = Path.GetFullPath($"{arch}/{res}");

        if (!WinInteropWrapper.IsInjected(processHandle, processId, path, out Module? module))
        {
            if (!EmbeddedResource.TryInject(processHandle, res, arch)
                || !WinInteropWrapper.IsInjected(processHandle, processId, path, out module))
            {
                pipe = null;

                Debug.Warn("    => Failure!");
                return false;
            }
        }

        if (!WinInteropWrapper.TryCallEntryPoint(processHandle, module.Base, "AslHelpPipe.EntryPoint"u8))
        {
            pipe = null;

            Debug.Warn("    => Failure!");
            return false;
        }

        Debug.Info("    => Success.");
        Debug.Info("  => Connecting named pipe...");

        try
        {
            pipe = new("asl-help-pipe");
            pipe.Connect(3000);

            Debug.Info("    => Success.");
            return true;
        }
        catch (TimeoutException)
        {
            pipe = null;

            Debug.Warn("    => Timed out!");
            return false;
        }
    }
}
