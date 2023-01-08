﻿using System.Reflection;
using System.Windows.Forms;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    private void Init(bool generateCode)
    {
        if (Methods.CurrentMethod != "startup")
        {
            Debug.Error("asl-help may only be initialized in the `startup` action.");
            throw new();
        }

        Debug.Info("Loading asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += (sender, e)
            => Assembly.LoadFrom($"Components/{e.Name.Split(',')[0]}.dll");

        try
        {
            Timer.Init();
            Script.Init();

            if (generateCode)
            {
                Debug.Info("  => Generating code...");
                GenerateCode();
            }
        }
        catch (Exception ex)
        {
            Debug.Show($"""
                asl-help had to abort due to a startup error.

                {ex}
                """, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void GenerateCode()
    {
        Script.Vars.Helper = this;
        Debug.Info("    => Set helper to `vars.Helper`.");

        Script.Vars.Log = (Action<object>)Log;
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Methods.shutdown.Prepend("vars.Helper.Dispose();");

        Script.Vars.StartBench = (Action<string>)StartBench;
        Script.Vars.StopBench = (Action<string>)StopBench;
    }
}
