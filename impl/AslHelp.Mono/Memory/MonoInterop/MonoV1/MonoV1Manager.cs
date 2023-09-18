using System.Buffers;
using System.Collections.Generic;

using AslHelp.Core.Diagnostics;
using AslHelp.Mono.Memory.Ipc;

using LiveSplit.ComponentUtil;

namespace AslHelp.Mono.Memory.MonoInterop.MonoV1;

internal class MonoV1Manager : MonoManager
{
    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory, new MonoV1Initializer()) { }

    protected MonoV1Manager(IMonoMemoryManager memory, IMonoInitializer initializer)
        : base(memory, initializer) { }

    public override IEnumerable<nuint> GetImages()
    {
        nuint assemblies = _loadedAssemblies;

        while (assemblies != 0)
        {
            nuint assembly = _memory.Read<nuint>(assemblies + _structs["GList"]["data"]);
            nuint image = _memory.Read<nuint>(assembly + _structs["MonoAssembly"]["image"]);

            yield return image;

            assemblies = _memory.Read<nuint>(assemblies + _structs["GList"]["next"]);
        }
    }

    public override string GetImageName(nuint image)
    {
        nuint name = _memory.Read<nuint>(image + _structs["MonoImage"]["assembly_name"]);
        return _memory.ReadString(256, ReadStringType.UTF8, name);
    }

    public override string GetImagePath(nuint image)
    {
        nuint path = _memory.Read<nuint>(image + _structs["MonoImage"]["module_name"]);
        return _memory.ReadString(260, ReadStringType.UTF8, path);
    }

    public override IEnumerable<nuint> GetClasses(nuint image)
    {
        nuint classCache = _memory.Read<nuint>(image + _structs["MonoImage"]["class_cache"] + _structs["MonoInternalHashTable"]["table"]);
        int size = _memory.Read<int>(image + _structs["MonoImage"]["class_cache"] + _structs["MonoInternalHashTable"]["size"]);

        nuint[] classes = ArrayPool<nuint>.Shared.Rent(size);

        if (!_memory.TryReadSpan<nuint>(classes, classCache))
        {
            ArrayPool<nuint>.Shared.Return(classes);

            yield break;
        }

        for (int i = 0; i < size; i++)
        {
            nuint klass = classes[i];
            while (klass != 0)
            {
                yield return klass;

                klass = _memory.Read<nuint>(klass + _structs["MonoClass"]["next_class_cache"]);
            }
        }

        ArrayPool<nuint>.Shared.Return(classes);
    }

    public override string GetClassName(nuint klass)
    {
        nuint name = _memory.Read<nuint>(klass + _structs["MonoClass"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, name);
    }

    public override string GetClassNamespace(nuint klass)
    {
        nuint nameSpace = _memory.Read<nuint>(klass + _structs["MonoClass"]["name_space"]);
        return _memory.ReadString(256, ReadStringType.UTF8, nameSpace);
    }

    public override IEnumerable<nuint> GetFields(nuint klass)
    {
        while (klass != 0)
        {
            int fieldCount = _memory.Read<int>(klass + _structs["MonoClass"]["field.count"]);
            nuint fields = _memory.Read<nuint>(klass + _structs["MonoClass"]["fields"]);

            for (int i = 0; i < fieldCount; i++)
            {
                nuint field = _memory.Read<nuint>(fields + (uint)(_structs["MonoClassField"].SelfAlignedSize * i));

                yield return field;
            }

            klass = _memory.Read<nuint>(klass + _structs["MonoClass"]["parent"]);
        }
    }

    public override string GetFieldName(nuint field)
    {
        nuint name = _memory.Read<nuint>(field + _structs["MonoClassField"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, name);
    }

    public override int GetFieldOffset(nuint field)
    {
        return _memory.Read<int>(field + _structs["MonoClassField"]["offset"]);
    }
}
