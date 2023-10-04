using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

using OneOf;

namespace AslHelp.Unity.Memory.Pointers.Initialization;

public interface IMonoPointerFactory : IPointerFactory
{
    Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged;
}
