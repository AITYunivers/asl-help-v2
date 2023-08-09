using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;

using AslHelp.Core.IO;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Native;

using BenchmarkDotNet.Attributes;

namespace AslHelp.Core.Memory.Benchmarks;

[MemoryDiagnoser(false)]
public class MemoryReadingBenchmarks
{
#nullable disable
    private ExternalMemoryManager _externalMemory;
    private InternalMemoryManager _internalMemory;
#nullable restore

    [GlobalSetup]
    public void Setup()
    {
        var process = Process.GetProcessesByName("Post Void").Single();

        nuint processHandle = (nuint)(nint)process.Handle;
        uint processId = (uint)process.Id;

        string arch = process.ProcessIs64Bit() ? "x64" : "x86";
        string res = $"AslHelp.Native.{arch}.dll";

        Directory.CreateDirectory(arch);
        string path = Path.GetFullPath($"{arch}/{res}");

        if (!WinInteropWrapper.IsInjected(processHandle, processId, path, out Module? module))
        {
            if (!EmbeddedResource.TryInject(processHandle, res, arch)
                || !WinInteropWrapper.IsInjected(processHandle, processId, path, out module))
            {
                throw new("not injected");
            }
        }

        // if (!WinInteropWrapper.TryCallEntryPoint(processHandle, module.Base, "AslHelp_Native_EntryPoint"u8))
        // {
        //     throw new("couldn't call entry point");
        // }

        NamedPipeClientStream pipe = new("asl-help-pipe");
        pipe.Connect(3000);

        _externalMemory = new ExternalMemoryManager(process);
        _internalMemory = new InternalMemoryManager(process, pipe);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _externalMemory.Dispose();
        _internalMemory.Dispose();
    }

    [Benchmark(Baseline = true)]
    public double ReadDouble_External()
    {
        return _externalMemory.Read<double>(0x813390, 0x48, 0x10, 0x690, 0x30);
    }

    [Benchmark]
    public double ReadDouble_Pipe()
    {
        return _internalMemory.Read<double>(0x813390, 0x48, 0x10, 0x690, 0x30);
    }
}
