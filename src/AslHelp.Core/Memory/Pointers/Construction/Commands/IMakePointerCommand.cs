using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IMakePointerCommand
{
    IPointer Result { get; }

    void Name(string name);
    void LogChange();
    void UpdateOnFail();

    bool TryExecute(IMemoryManager manager);
}
