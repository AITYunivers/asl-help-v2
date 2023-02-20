using AslHelp.Core.Exceptions;
using AslHelp.Core.LiveSplitInterop;
using System.Reflection;
using System.Windows.Forms;

public partial class Basic
{
    private void Init(bool generateCode)
    {
        if (Methods.CurrentMethod != "startup")
        {
            Debug.Error();
            throw new();
        }

        Debug.Info("Loading asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
        {
            string name = e.Name;
            int i = name.IndexOf(',');
            if (i == -1)
            {
                ThrowHelper.Throw.Argument(nameof(e.Name), "Assembly name was in an unexpected format.");
            }

            string file = $"Components/{name[..i]}.dll";

            return File.Exists(file)
                ? Assembly.LoadFrom(file)
                : null;
        };

        try
        {
            AslHelp.Core.LiveSplitInterop.Timer.Init();
            Script.Init();

            if (generateCode)
            {
                Debug.Info("  => Generating code...");
                GenerateCode();
            }
        }
        catch (Exception ex)
        {
            _ = Debug.Show($"""
                asl-help had to abort due to a startup error.

                {ex}
                """, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Debug.Info("  => Complete.");
    }

    private void GenerateCode()
    {
        Script.Vars.AslHelp = this;
        Debug.Info("    => Set helper to `vars.AslHelp`.");

        Script.Vars.Log = (Action<object>)Log;
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Methods.shutdown.Prepend("vars.AslHelp.Dispose();");

        Script.Vars.StartBench = (Action<string>)StartBench;
        Script.Vars.StopBench = (Action<string>)StopBench;
    }
}
