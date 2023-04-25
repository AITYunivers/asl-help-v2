using AslHelp.Core.Reflection;
using CommunityToolkit.HighPerformance;
using LiveSplit.ASL;

namespace AslHelp.Core.LiveSplitInterop;

internal static class Actions
{
    private static ASLScript.Methods _methods;

    public static void Init(ASLScript script)
    {
        _methods = script.GetFieldValue<ASLScript.Methods>("_methods");
    }

    public static unsafe string CurrentAction
    {
        get
        {
            ReadOnlySpan<char> prefix = "ASLScript.".AsSpan();

            foreach (string trace in Debug.Trace)
            {
                ReadOnlySpan<char> sTrace = trace.AsSpan();

                int i = sTrace.IndexOf(prefix);
                if (i == -1)
                {
                    continue;
                }

                for (i += prefix.Length + 2; i < sTrace.Length; i++)
                {
                    if (char.IsUpper(sTrace[i]))
                    {
                        break;
                    }
                }

                return trace[i..].ToLower();
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
