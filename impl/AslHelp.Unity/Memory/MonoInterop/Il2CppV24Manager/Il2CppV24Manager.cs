using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager : MonoManagerBase
{
    // private int _assembliesSize;

    public Il2CppV24Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override Result<nuint, MonoInitializationError> FindLoadedAssemblies()
    {
        throw new System.NotImplementedException();
    }

    protected override Result<NativeStructMap, ParseError> InitializeStructs()
    {
        throw new System.NotImplementedException();
    }

    public override nuint GetArrayClass(nuint arrayClass)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<nuint> GetClassFields(nuint klass)
    {
        throw new System.NotImplementedException();
    }

    public override string GetClassName(nuint klass)
    {
        throw new System.NotImplementedException();
    }

    public override string GetClassNamespace(nuint klass)
    {
        throw new System.NotImplementedException();
    }

    public override nuint GetClassParent(nuint klass)
    {
        throw new System.NotImplementedException();
    }

    public override string GetFieldName(nuint field)
    {
        throw new System.NotImplementedException();
    }

    public override int GetFieldOffset(nuint field)
    {
        throw new System.NotImplementedException();
    }

    public override nuint GetFieldType(nuint field)
    {
        throw new System.NotImplementedException();
    }

    public override nuint GetGenericInstClass(nuint genericClass)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<nuint> GetImageClasses(nuint image)
    {
        throw new System.NotImplementedException();
    }

    public override string GetImageFileName(nuint image)
    {
        throw new System.NotImplementedException();
    }

    public override string GetImageName(nuint image)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerable<nuint> GetImages()
    {
        throw new System.NotImplementedException();
    }

    public override MonoFieldAttribute GetTypeAttributes(nuint type)
    {
        throw new System.NotImplementedException();
    }

    public override nuint GetTypeData(nuint type)
    {
        throw new System.NotImplementedException();
    }

    public override MonoElementType GetTypeElementType(nuint type)
    {
        throw new System.NotImplementedException();
    }

    // protected override NativeStructMap InitializeStructs()
    // {
    //     return NativeStructMap.FromFile("Unity", "il2cpp", "v24", _memory.Is64Bit);
    // }

    // protected override nuint FindLoadedAssemblies()
    // {
    //     Signature signature =
    //         _memory.Is64Bit
    //         ? new(12, "48 FF C5 80 3C ?? 00 75 ?? 48 8B 1D")
    //         : new(9, "8A 07 47 84 C0 75 ?? 8B 35");

    //     nuint sAssembliesRelative = _memory.Scan(signature, _memory.MonoModule);
    //     nuint sAssemblies = _memory.ReadRelative(sAssembliesRelative);

    //     // il2cpp::vm::s_Assemblies is a std::vector<Il2CppAssembly*>
    //     nuint front = _memory.Read<nuint>(sAssemblies);
    //     nuint back = _memory.Read<nuint>(sAssemblies + _memory.PointerSize);

    //     _assembliesSize = (int)(back - front) / _memory.PointerSize;

    //     Debug.Warn($"{(ulong)front:X}");
    //     Debug.Warn($"{(ulong)back:X}");
    //     Debug.Warn($"{_assembliesSize}");

    //     return front;
    // }
}
