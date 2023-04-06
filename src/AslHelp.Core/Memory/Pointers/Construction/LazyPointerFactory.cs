using IMakeCmd = AslHelp.Core.Memory.Pointers.Construction.Commands.IMakePointerCommand;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface IParentStage
{
    IParentStage MakeParent(params int[] offsets);


}

public interface IRefineStage
{
    IRefineStage Name(string name);
    IRefineStage LogChange();
    IRefineStage UpdateOnFail();
}

public class LazyPointerFactory
    : ILazyPointerFactory
{
    private readonly List<IMakeCmd> _commands = new();
    private readonly List<IMakeCmd> _parents = new();

    private IMakeCmd _last;

    public ILazyPointerFactory MakeParent(nint baseAddress, params int[] offsets)
    {
        IMakeCmd cmd =
            _parents.LastOrDefault() is IMakeCmd parent
            ? new Commands.MakePointer<nint>(parent, )
            : new Commands.MakePointer<nint>(baseAddress, offsets);

        _commands.Add(cmd);
        _parents.Add(cmd);

        return this;
    }

    public ILazyPointerFactory MakeParent(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public ILazyPointerFactory MakeParent(string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public ILazyPointerFactory MakeParent(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
