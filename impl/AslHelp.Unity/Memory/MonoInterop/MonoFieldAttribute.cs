using System;

namespace AslHelp.Unity.Memory.MonoInterop;

[Flags]
public enum MonoFieldAttribute : ushort
{
    COMPILER_CONTROLLED = 0x0000,
    PRIVATE = 0x0001,
    FAM_AND_ASSEM = 0x0002,
    ASSEMBLY = 0x0003,
    FAMILY = 0x0004,
    FAM_OR_ASSEM = 0x0005,
    PUBLIC = 0x0006,
    FIELD_ACCESS = 0x0007,

    STATIC = 0x0010,
    INIT_ONLY = 0x0020,
    LITERAL = 0x0040,
    NOT_SERIALIZED = 0x0080,
    SPECIAL_NAME = 0x0200,
    PINVOKE_IMPL = 0x2000,

    HAS_RVA = 0x0100,
    RT_SPECIAL_NAME = 0x0400,
    HAS_MARSHAL = 0x1000,
    HAS_DEFAULT = 0x8000,
    RESERVED_MASK = 0x9500
}
