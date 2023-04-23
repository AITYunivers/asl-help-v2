using AslHelp.Core.Exceptions;
using AslHelp.Core.Reflection;
using LiveSplit.ASL;

namespace AslHelp.Core.LiveSplitInterop;

internal static class Actions
{
    private static ASLScript.Methods _methods;

    public static void Init(ASLScript script)
    {
        _methods = script.GetFieldValue<ASLScript.Methods>("_methods");
    }

    public static string CurrentAction
    {
        get
        {
            const string Prefix = "ASLScript.";
            const int PrefixLength = 10;

            foreach (string trace in Debug.Trace)
            {
                int i = trace.IndexOf(Prefix);
                if (i == -1)
                {
                    continue;
                }

                if (trace[i + PrefixLength] == 'D')
                {
                    return trace[(i + PrefixLength + 2)..].ToLower();
                }

                if (trace[i + PrefixLength] == 'R')
                {
                    return trace[(i + PrefixLength + 3)..].ToLower();
                }
            }

            throw new Exception("Not in an ASL.");
        }
    }

#pragma warning disable IDE1006
    public static Action startup { get; set; } = new("startup");
    public static Action shutdown { get; set; } = new("shutdown");
    public static Action init { get; set; } = new("init");
    public static Action exit { get; set; } = new("exit");
    public static Action update { get; set; } = new("update");
    public static Action start { get; set; } = new("start");
    public static Action split { get; set; } = new("split");
    public static Action reset { get; set; } = new("reset");
    public static Action gameTime { get; set; } = new("gameTime");
    public static Action isLoading { get; set; } = new("isLoading");
    public static Action onStart { get; set; } = new("onStart");
    public static Action onSplit { get; set; } = new("onSplit");
    public static Action onReset { get; set; } = new("onReset");
#pragma warning restore IDE1006

    internal struct Action
    {
        public Action(string name)
            : this("", name, 0) { }

        public Action(string body, string name, int line)
        {
            Body = body;
            Name = name;
            Line = line;
        }

        public string Body { get; private set; }
        public string Name { get; }
        public int Line { get; }

        public void Append(string code)
        {
            Body = $"{Body}{code}";
            Update();

            Debug.Info($"    => Added the following code to the end of '{Name}':");
            Debug.Info($"       `{code}`");
        }

        public void Prepend(string code)
        {
            Body = $"{code}{Body}";
            Update();

            Debug.Info($"    => Added the following code to the beginning of '{Name}':");
            Debug.Info($"       `{code}`");
        }

        private void Update()
        {
            ASLMethod method = new(Body, Name, Line);
            _methods.SetFieldValue(Name, method);
        }
    }
}
