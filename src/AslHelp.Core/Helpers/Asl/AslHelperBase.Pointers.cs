using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public abstract IPointer this[string name] { get; set; }
    public abstract IPointerFactory? Pointers { get; }

    public abstract void MapPointerValuesToCurrent();
}
