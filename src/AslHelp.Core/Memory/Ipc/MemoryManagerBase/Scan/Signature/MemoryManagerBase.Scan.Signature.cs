using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Scan(Signature signature, uint alignment = 1)
    {
        return ScanAll(signature, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, uint size, uint alignment = 1)
    {
        return ScanAll(signature, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, string moduleName, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Scan] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, string moduleName, uint size, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Scan] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, Module module, uint alignment = 1)
    {
        return ScanAll(signature, module, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, Module module, uint size, uint alignment = 1)
    {
        return ScanAll(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1)
    {
        return ScanAll(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, nuint startAddress, uint size, uint alignment = 1)
    {
        return ScanAll(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
