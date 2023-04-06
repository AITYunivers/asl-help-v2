namespace AslHelp.Core.IO;

internal class Trace : IEnumerable<string>
{
    public int Depth => new StackTrace().FrameCount;

    public IEnumerator<string> GetEnumerator()
    {
        StackFrame[] frames = new StackTrace().GetFrames();

        for (int i = 0; i < frames.Length; i++)
        {
            MethodBase method = frames[i].GetMethod();
            Type decl = method.DeclaringType;
            string ret = decl is null ? method.Name : $"{decl.Name}.{method.Name}";

            yield return ret;
        }
    }

    public bool ContainsAny(params string[] methodNames)
    {
        return this.Any(t => methodNames.Any(m => m.Equals(t, StringComparison.OrdinalIgnoreCase)));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
