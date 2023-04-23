namespace AslHelp.Core.IO;

internal class Trace : IReadOnlyCollection<string>
{
    private StackTrace Stack => new();
    private StackFrame[] Frames => Stack.GetFrames();

    public int Count => Stack.FrameCount;

    public bool ContainsAny(params string[] methodNames)
    {
        foreach (string methodName in this)
        {
            for (int i = 0; i < methodNames.Length; i++)
            {
                if (methodNames[i].Equals(methodName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public IEnumerator<string> GetEnumerator()
    {
        StackFrame[] frames = Frames;

        for (int i = 0; i < frames.Length; i++)
        {
            MethodBase method = frames[i].GetMethod();
            Type decl = method.DeclaringType;
            string ret = decl is null ? method.Name : $"{decl.Name}.{method.Name}";

            yield return ret;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
