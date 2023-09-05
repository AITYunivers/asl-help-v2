using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.MonoInterop.MonoV1;

public abstract class MonoManagerBase : IMonoManager
{
    protected IMonoMemoryManager _memory;
    protected NativeStructMap _structs;

    public MonoManagerBase(IMonoMemoryManager memory)
    {
        Initialize(memory);
    }

    [MemberNotNull(nameof(_memory), nameof(_structs))]
    protected virtual void Initialize(IMonoMemoryManager memory)
    {
        nuint assembliesTable = FindAssembliesTable();
        if (assembliesTable == 0)
        {
            const string msg = "Unable to find the loaded assemblies.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _memory = memory;
        _structs = InitializeStructData();
    }

    protected abstract NativeStructMap InitializeStructData();
    protected abstract nuint FindAssembliesTable();

    public abstract MonoClass FindClass(string className);
    public abstract MonoClass FindClass(string @namespace, string className);
    public abstract MonoImage FindImage(string imageName);
    public abstract MonoClass GetParentClass(MonoClass monoClass);
    public abstract bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass);
    public abstract bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);
    public abstract bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage);
    public abstract bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}

public class MonoV1Manager : MonoManagerBase
{
#nullable disable
    private Signature[] _loadedAssembliesSignatures;
#nullable restore

    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory)
    { }

    [MemberNotNull(nameof(_loadedAssembliesSignatures))]
    protected override void Initialize(IMonoMemoryManager memory)
    {
        _loadedAssembliesSignatures = memory.Is64Bit
            ? [new(3, "48 8B 0D")]
            : [new(2, "FF 35"), new(2, "8B 0D")];

        base.Initialize(memory);
    }

    protected override NativeStructMap InitializeStructData()
    {
        return NativeStructMap.Parse("Mono", "mono", "v1", _memory.Is64Bit);
    }

    protected override nuint FindAssembliesTable()
    {
        nuint monoAssemblyForeach = _memory.MonoModule.Symbols["mono_assembly_foreach"].Address;
        if (monoAssemblyForeach == 0)
        {
            const string msg = "Unable to find symbol 'mono_assembly_foreach'.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint loadedAssemblies = 0;
        foreach (Signature signature in _loadedAssembliesSignatures)
        {
            // Attempt to find one of the signatures within the next 256 bytes from the beginning of the mono_assembly_foreach function.
            loadedAssemblies = _memory.Scan(signature, monoAssemblyForeach, 0x100);
            if (loadedAssemblies != 0)
            {
                break;
            }
        }

        if (loadedAssemblies == 0)
        {
            const string msg = "Failed scanning for a reference to 'loaded_assemblies'.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return _memory.Read<nuint>(_memory.ReadRelative(loadedAssemblies));
    }

    public override MonoClass FindClass(string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass FindClass(string @namespace, string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoImage FindImage(string imageName)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass GetParentClass(MonoClass monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent)
    {
        throw new System.NotImplementedException();
    }
}
