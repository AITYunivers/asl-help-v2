using System.Collections.Generic;

using AslHelp.Core.Collections;

namespace AslHelp.Unity.Memory.MonoInterop;

public interface IMonoManager
{
    LazyDictionary<string, MonoImage> Images { get; }

    IEnumerable<nuint> GetImages();
    string GetImageName(nuint image);
    string GetImageFileName(nuint image);

    IEnumerable<nuint> GetImageClasses(nuint image);
    string GetClassName(nuint klass);
    string GetClassNamespace(nuint klass);

    nuint GetClassParent(nuint klass);
    nuint GetArrayClass(nuint arrayClass);
    nuint GetGenericInstClass(nuint genericClass);

    IEnumerable<nuint> GetClassFields(nuint klass);
    string GetFieldName(nuint field);
    int GetFieldOffset(nuint field);
    nuint GetFieldType(nuint field);

    nuint GetTypeData(nuint type);
    MonoFieldAttribute GetTypeAttributes(nuint type);
    MonoElementType GetTypeElementType(nuint type);
}
