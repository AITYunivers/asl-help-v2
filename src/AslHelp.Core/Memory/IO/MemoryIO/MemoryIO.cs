using AslHelp.Core.Collections;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
    : IMemoryManager
{
    public Process Process { get; }
    public bool Is64Bit { get; }

    public Module MainModule { get; }
    public ModuleCache Modules { get; }

    public uint Tick { get; }

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
        throw new NotImplementedException();
    }
}
