
using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Interoperator
{
    public override IEnumerable<nuint> GetImages()
    {
        byte ptrSize = _memory.PointerSize;

        // il2cpp::vm::s_Assemblies is a std::vector<Il2CppAssembly*>
        nuint front = _memory.Read<nuint>(_loadedAssemblies + (ptrSize * 0U));
        nuint back = _memory.Read<nuint>(_loadedAssemblies + (ptrSize * 1U));

        int assembliesSize = (int)(back - front) / ptrSize;

        nuint[] assemblies = _memory.ReadSpan<nuint>(assembliesSize, front);
        foreach (nuint assembly in assemblies)
        {
            yield return Il2CppAssemblyImage(assembly);
        }
    }

    public override string GetImageName(nuint image)
    {
        nuint assemblyNameStart = _memory.Read<nuint>(image + _structs["Il2CppImage"]["nameNoExt"]);
        return _memory.ReadString(256, ReadStringType.UTF8, assemblyNameStart);
    }

    public override string GetImageFileName(nuint image)
    {
        // TODO: Check if this is correct. Might be Il2CppImage.assembly.aname.name?
        nuint assemblyNameStart = _memory.Read<nuint>(image + _structs["Il2CppImage"]["name"]);
        return _memory.ReadString(260, ReadStringType.UTF8, assemblyNameStart);
    }
}
