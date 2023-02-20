using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.LiveSplitInterop;

public partial class BasicNeo
{
    private void InitForAsl()
    {
        if (Methods.CurrentMethod != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }
    }

    private bool _generated;
    public IAslHelper GenerateCode()
    {
        if (_generated)
        {
            string msg = ;
            ThrowHelper.Throw.InvalidOperation();
        }

        Script.Vars.AslHelp = (IHelper)this;
        Debug.Info("    => Set helper to `vars.AslHelp`.");

        Script.Vars.Log = (Action<object>)(output => Logger.Log(output));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Methods.shutdown.Prepend("vars.AslHelp.Dispose();");

        _generated = true;

        return this;
    }
}
