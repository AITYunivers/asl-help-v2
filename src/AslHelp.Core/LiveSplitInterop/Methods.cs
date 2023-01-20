using AslHelp.Core.Reflection;
using LiveSplit.ASL;

namespace AslHelp.Core.LiveSplitInterop;

internal static class Methods
{
    private static ASLScript.Methods _methods;

    public static void Init(ASLScript script)
    {
        _methods = script.GetFieldValue<ASLScript.Methods>("_methods");
    }

    public static string CurrentMethod
    {
        get
        {
            foreach (string trace in Debug.Trace.Reverse())
            {
                if (trace.StartsWith("ASLScript.Do"))
                {
                    return trace[12..].ToLower();
                }

                if (trace.StartsWith("ASLScript.Run"))
                {
                    return trace[13..].ToLower();
                }
            }

            return null;
        }
    }

#pragma warning disable IDE1006
    public static Method startup { get; set; } = new("startup");
    public static Method shutdown { get; set; } = new("shutdown");
    public static Method init { get; set; } = new("init");
    public static Method exit { get; set; } = new("exit");
    public static Method update { get; set; } = new("update");
    public static Method start { get; set; } = new("start");
    public static Method split { get; set; } = new("split");
    public static Method reset { get; set; } = new("reset");
    public static Method gameTime { get; set; } = new("gameTime");
    public static Method isLoading { get; set; } = new("isLoading");
    public static Method onStart { get; set; } = new("onStart");
    public static Method onSplit { get; set; } = new("onSplit");
    public static Method onReset { get; set; } = new("onReset");
#pragma warning restore IDE1006

    internal struct Method
    {
        public Method(string name)
            : this("", name, 0) { }

        public Method(string body, string name, int line)
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
