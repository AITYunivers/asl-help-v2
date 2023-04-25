using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
    : IAslHelper,
    IAslHelper.InitStage
{
    protected abstract void Exit();

    IAslHelper IAslHelper.Exit()
    {
        if (Actions.CurrentAction != "exit")
        {
            string msg = $"Attempted to call {nameof(Exit)} outside of the 'exit' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        DisposeMemory();

        Exit();

        return this;
    }

    protected abstract void Shutdown();

    IAslHelper IAslHelper.Shutdown()
    {
        if (Actions.CurrentAction != "shutdown")
        {
            string msg = $"Attempted to call {nameof(Shutdown)} outside of the 'shutdown' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        DisposeMemory();

        Shutdown();

        return this;
    }
}
