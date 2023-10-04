using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    public override IEnumerable<nuint> GetImages()
    {
        nuint assemblies = _assemblies;

        while (assemblies != 0)
        {
            nuint data = GListData(assemblies);
            yield return MonoAssemblyImage(data);

            assemblies = GListNext(assemblies);
        }
    }

    public override string GetImageName(nuint image)
    {
        nuint assemblyNameStart = _memory.Read<nuint>(image + _structs["MonoImage"]["assembly_name"]);
        return _memory.ReadString(256, ReadStringType.UTF8, assemblyNameStart);
    }

    public override string GetImageFileName(nuint image)
    {
        nuint moduleNameStart = _memory.Read<nuint>(image + _structs["MonoImage"]["module_name"]);
        return _memory.ReadString(260, ReadStringType.UTF8, moduleNameStart);
    }
}
