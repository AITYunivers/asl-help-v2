using AslHelp.Core.Collections;
using AslHelp.Core.Exceptions;
using AslHelp.Core.IO.Logging;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
    : IMemoryManager
{
    private readonly ILogger _logger;

    protected readonly nint _processHandle;
    protected bool _isDisposed;

    public MemoryManagerBase(Process process, ILogger logger)
    {
        ThrowHelper.ThrowIfNull(process);
        if (process.HasExited)
        {
            ThrowHelper.Throw.InvalidOperation("Cannot interact with the memory of an exited process.");
        }

        _logger = logger;
        _processHandle = process.Handle;

        Process = process;
        Is64Bit = Native.Is64Bit(_processHandle);
        PtrSize = (byte)(Is64Bit ? 0x8 : 0x4);
        Modules = new(process);
        MainModule = Modules.FirstOrDefault();

        process.Exited += DisposeEvent;
    }

    public Process Process { get; }
    public bool Is64Bit { get; }
    public byte PtrSize { get; }

    public Module MainModule { get; }
    public ModuleCache Modules { get; }

    public uint Tick { get; private set; }

    public virtual void Update()
    {
        Process.Refresh();
        Tick++;
    }

    public nint FromAbsoluteAddress(nint address)
    {
        return Read<nint>(address);
    }

    public nint FromRelativeAddress(nint address)
    {
        return address + 0x4 + Read<int>(address);
    }

    public nint FromAssemblyAddress(nint address)
    {
        return Is64Bit ? FromRelativeAddress(address) : FromAbsoluteAddress(address);
    }

    public void Log(object output)
    {
        _logger?.Log(output);
    }

    public virtual void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        Log("Disposing of memory manager...");

        _isDisposed = true;
        Process.Dispose();
    }

    private void DisposeEvent(object sender, EventArgs e)
    {
        if (sender is not Process process)
        {
            return;
        }

        Dispose();
        process.Exited -= DisposeEvent;
    }
}
