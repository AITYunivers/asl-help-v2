using System.Linq;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanRel(uint offset, params string[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(uint offset, params byte[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, uint offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, uint offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(Module module, uint offset, params string[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(Module module, uint offset, params byte[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, nuint endAddress, uint offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, nuint endAddress, uint offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, uint size, uint offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, uint size, uint offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
