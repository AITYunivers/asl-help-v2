using AslHelp.Common.Results;
using AslHelp.Core.Extensions;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager : MonoManagerBase
{
    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override Result<NativeStructMap, ParseError> InitializeStructs()
    {
        return NativeStructMap.InitializeFromResource("Unity", "mono", "v1", _memory.Is64Bit);
    }

    protected override Result<nuint, MonoInitializationError> FindLoadedAssemblies()
    {
        if (!_memory.MonoModule.Symbols.TryGetValue("mono_assembly_foreach", out DebugSymbol symMonoAssemblyForeach))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SymbolMonoAssemblyForeachNotFound);
        }

        if (symMonoAssemblyForeach.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SymbolMonoAssemblyForeachNull);
        }

        Signature[] signatures =
            _memory.Is64Bit
            ? [new(3, "48 8B 0D")]
            : [new(2, "FF 35"), new(2, "8B 0D")];

        nuint loadedAssembliesRelative = _memory.Scan(signatures, symMonoAssemblyForeach.Address, 0x100);
        if (loadedAssembliesRelative == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.MonoAssemblyForeachSignaturesNotResolved);
        }

        if (!_memory.TryReadRelative(loadedAssembliesRelative, out nuint loadedAssemblies))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.MonoAssemblyForeachRelativeReadFailed);
        }

        return new(
            IsSuccess: true,
            Value: loadedAssemblies);
    }

    protected override Result<nuint[], MonoInitializationError> FindDefaults()
    {
        if (!_memory.MonoModule.Symbols.TryGetValue("mono_get_corlib", out DebugSymbol symMonoGetCorLib))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        if (symMonoGetCorLib.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        Signature signature =
            _memory.Is64Bit
            ? new(3, "48 8B 05 ???????? C3")
            : new(1, "A1 ???????? C3");

        nuint monoDefaultsRelative = _memory.Scan(signature, symMonoGetCorLib.Address, 0x10);
        if (monoDefaultsRelative == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        if (!_memory.TryReadRelative(monoDefaultsRelative, out nuint monoDefaults))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        if (!_memory.TryReadSpan(out nuint[]? defaultInstances, 18, monoDefaults))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        return new(
            IsSuccess: true,
            Value: defaultInstances);
    }
}
