using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IMakePointerCommand
{
    bool TryEvaluate(IMemoryManager manager, out IPointer pointer);
}

