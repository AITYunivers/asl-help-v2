namespace AslHelp.Unity.Memory.MonoInterop;

public record MonoDefaults(
    MonoImage Corlib,
    MonoClass ObjectClass,
    MonoClass ByteClass,
    MonoClass VoidClass,
    MonoClass BooleanClass,
    MonoClass SByteClass,
    MonoClass Int16Class,
    MonoClass UInt16Class,
    MonoClass Int32Class,
    MonoClass UInt32Class,
    MonoClass IntPtrClass,
    MonoClass UIntPtrClass,
    MonoClass Int64Class,
    MonoClass UInt64Class,
    MonoClass SingleClass,
    MonoClass DoubleClass,
    MonoClass CharClass,
    MonoClass StringClass);
