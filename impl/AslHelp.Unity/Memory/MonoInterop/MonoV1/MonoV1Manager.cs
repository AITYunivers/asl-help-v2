using System.Collections.Generic;

using AslHelp.Core.Extensions;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager : MonoManagerBase
{
    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override NativeStructMap InitializeStructs()
    {
        return NativeStructMap.FromFile("Unity", "mono", "v1", _memory.Is64Bit);
    }

    protected override nuint FindLoadedAssemblies()
    {
        nuint monoAssemblyForeach = _memory.MonoModule.Symbols["mono_assembly_foreach"].Address;

        Signature[] signatures =
            _memory.Is64Bit
            ? [new(3, "48 8B 0D")]
            : [new(2, "FF 35"), new(2, "8B 0D")];

        nuint loadedAssembliesRelative = _memory.Scan(signatures, monoAssemblyForeach, 0x100);
        nuint loadedAssemblies = _memory.ReadRelative(loadedAssembliesRelative);
        return _memory.Read<nuint>(loadedAssemblies);
    }
}
