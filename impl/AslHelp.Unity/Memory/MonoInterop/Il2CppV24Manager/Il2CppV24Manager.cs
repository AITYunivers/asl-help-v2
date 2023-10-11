using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager : MonoManagerBase
{
    private nuint _typeInfoDefinitionTable;

    public Il2CppV24Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override Result<nuint, MonoInitializationError> FindLoadedAssemblies()
    {
        Signature sAssembliesSignatures =
            _memory.Is64Bit
            ? new(12, "48 FF C5 80 3C ?? 00 75 ?? 48 8B 1D")
            : new(9, "8A 07 47 84 C0 75 ?? 8B 35");

        nuint sAssembliesRelative = _memory.Scan(sAssembliesSignatures, _memory.MonoModule);
        if (sAssembliesRelative == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SAssembliesSignaturesNotResolved);
        }

        if (!_memory.TryReadRelative(sAssembliesRelative, out nuint sAssemblies))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SAssembliesRelativeReadFailed);
        }

        Signature typeInfoDefinitionTableSignatures =
            _memory.Is64Bit
            ? new(-4, "48 83 3C ?? 00 75 ?? 8B C? E8")
            : new(2, "C3 A1 ???????? 83 3C ?? 00");

        nuint typeInfoDefinitionTableRelative = _memory.Scan(typeInfoDefinitionTableSignatures, _memory.MonoModule);
        if (typeInfoDefinitionTableRelative == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.TypeInfoDefinitionTableSignaturesNotResolved);
        }

        if (!_memory.TryReadRelative(typeInfoDefinitionTableRelative, out _typeInfoDefinitionTable))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.TypeInfoDefinitionTableRelativeReadFailed);
        }

        return new(
            IsSuccess: true,
            Value: sAssemblies);
    }

    protected override Result<NativeStructMap, ParseError> InitializeStructs()
    {
        return NativeStructMap.InitializeFromResource("Unity", "il2cpp", "v24", _memory.Is64Bit);
    }
}
