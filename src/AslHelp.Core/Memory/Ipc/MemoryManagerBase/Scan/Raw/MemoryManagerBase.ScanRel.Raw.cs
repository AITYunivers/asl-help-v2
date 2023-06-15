using System.Linq;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nint ScanRel(int offset, params string[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(int offset, params byte[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(string moduleName, int offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(string moduleName, int offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(Module module, int offset, params string[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, nint endAddress, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, nint endAddress, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, int size, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, int size, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
