namespace AslHelp.Unity.Memory.MonoInterop;

public record struct MonoDefaults(
    nuint Corlib,
    nuint ObjectClass,
    nuint ByteClass,
    nuint VoidClass,
    nuint BooleanClass,
    nuint SByteClass,
    nuint Int16Class,
    nuint UInt16Class,
    nuint Int32Class,
    nuint UInt32Class,
    nuint IntPtrClass,
    nuint UIntPtrClass,
    nuint Int64Class,
    nuint UInt64Class,
    nuint SingleClass,
    nuint DoubleClass,
    nuint CharClass,
    nuint StringClass);
