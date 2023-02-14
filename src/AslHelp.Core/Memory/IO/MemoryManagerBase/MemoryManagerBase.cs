using AslHelp.Core.Collections;
using AslHelp.Core.Exceptions;
using AslHelp.Core.IO.Logging;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
    : IMemoryManager
{
    private readonly LoggerBase _logger;

    public MemoryManagerBase(Process process, LoggerBase logger)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        Process = process;
        Is64Bit = process.Is64Bit();
        PtrSize = (byte)(Is64Bit ? 0x8 : 0x4);
        Modules = new(process);

        _logger = logger;
    }

    public Process Process { get; }
    public bool Is64Bit { get; }
    public byte PtrSize { get; }

    public Module MainModule => Modules.FirstOrDefault();
    public ModuleCache Modules { get; }

    public uint Tick { get; private set; }

    public virtual void Update()
    {
        Process?.Refresh();
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
}
