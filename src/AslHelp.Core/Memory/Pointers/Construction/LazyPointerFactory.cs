using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface INameChildStage
{
    IRefineChildStage Name(string name);
}

public interface IRefineChildStage
{
    IRefineChildStage LogChange();
    IRefineChildStage UpdateOnFail();
}
