using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    protected override void GenerateCode()
    {
        Script.Vars["Log"] = (Action<object>)(output => _logger.Log($"[{GameName}] {output}"));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Actions.exit.Prepend($"vars.AslHelp.{nameof(Exit)}();");
        Actions.shutdown.Prepend($"vars.AslHelp.{nameof(Shutdown)}();");
    }

    protected override void Complete()
    {
        _logger.Add(new DebugLogger());

        Timer.Init();
        Script.Init();
    }
}
