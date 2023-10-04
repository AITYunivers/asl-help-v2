using System.Collections.Generic;

namespace AslHelp.Unity.Memory.MonoInterop;

public interface IMonoManager
{
    IEnumerable<nuint> GetImages();
    string GetImageName(nuint image);
    string GetImageFileName(nuint image);

    IEnumerable<nuint> GetImageClasses(nuint image);
    string GetClassName(nuint klass);
    string GetClassNamespace(nuint klass);

    IEnumerable<nuint> GetClassFields(nuint klass);
    string GetFieldName(nuint field);
    int GetFieldOffset(nuint field);
}
