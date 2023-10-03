using AslHelp.Core.Memory.Pointers;

using OneOf;

namespace AslHelp.Mono.Memory.Pointers.Initialization;

public interface IPointerFromImageFactory
{
    Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged;
}
