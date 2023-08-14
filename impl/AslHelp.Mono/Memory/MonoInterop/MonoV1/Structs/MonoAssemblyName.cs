using System.Runtime.InteropServices;

namespace AslHelp.Mono.Memory.MonoInterop.MonoV1.Structs;

[StructLayout(LayoutKind.Explicit)]
internal readonly struct MonoAssembly64
{
    [FieldOffset(0x58)] public nuint
}
