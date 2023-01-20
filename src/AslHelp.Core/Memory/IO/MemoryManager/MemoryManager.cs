using AslHelp.Core.Collections;
using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
    : IMemoryManager
{
    private Process _process;

    public Process Process
    {
        get => _process;
        set
        {
            ThrowHelper.ThrowIfNullOrExited(value);

            _process = value;
            Is64Bit = Native.Is64Bit(value);
            Modules = new(value);
        }
    }

    public bool Is64Bit { get; private set; }

    public Module MainModule => Modules.FirstOrDefault();
    public ModuleCache Modules { get; private set; }

    public uint Tick { get; private set; }

    public void Update()
    {
        Tick++;
        _process?.Refresh();
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
        // TODO
    }

    public void Dispose()
    {
        _process?.Dispose();
    }
}
