using System.Reflection;
using System.Windows.Forms;
using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    private bool _isInitialized;
    private bool _hasGenerated;

    private void Init()
    {
        if (_isInitialized)
        {
            string msg = "Helper is already initialized.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _logger = new(new DebugLogger());

        Debug.Info("Loading asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

        try
        {
            AslHelp.Core.LiveSplitInterop.Timer.Init();
            Script.Init();
        }
        catch (Exception ex)
        {
            _ = Debug.Show($"""
                asl-help had to abort due to an initialization error.

                {ex}
                """, MessageBoxButtons.OK, MessageBoxIcon.Error);

            throw;
        }

        Debug.Info("  => Complete.");

        _isInitialized = true;
    }

    public void GenerateCode()
    {
        if (_hasGenerated)
        {
            string msg = "Code has already been generated.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        Debug.Info("  => Generating code...");

#pragma warning disable IDE0004
        Script.Vars["AslHelp"] = (IAslHelper)this;
#pragma warning restore IDE0004
        Debug.Info("    => Created `vars.AslHelp`.");

        Script.Vars["Log"] = (Action<object>)(output => _logger.Log(output));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Methods.shutdown.Prepend("vars.AslHelp.Dispose();");

        _hasGenerated = true;
    }

    private Assembly AssemblyResolve(object sender, ResolveEventArgs e)
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
    }
}
