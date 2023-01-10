using System.Runtime.InteropServices;

namespace AslHelp.Native;

public static unsafe class EntryPoint
{
    [UnmanagedCallersOnly(EntryPoint = $"{nameof(AslHelp)}.{nameof(Native)}.{nameof(EntryPoint)}.{nameof(Execute)}")]
    public static uint Execute(void* _)
    {
        return 0;
    }
}
