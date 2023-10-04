using AslHelp.Core.Memory.Pointers;

using OneOf;

namespace AslHelp.Unity.Memory.Pointers.Initialization;

public interface IPointerFromClassFactory
{
    Pointer<T> Make<T>(string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged;
}
