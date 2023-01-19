namespace AslHelp.Core.Reflection;

public static class ReflectionExtensions
{
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        return (T)obj.GetType().GetRuntimeFields().FirstOrDefault(fi => fi.Name == fieldName)?.GetValue(obj);
    }

    public static void SetFieldValue<T>(this object obj, string fieldName, T value)
    {
        obj.GetType().GetRuntimeFields().FirstOrDefault(fi => fi.Name == fieldName)?.SetValue(obj, value);
    }

    public static T GetPropertyValue<T>(this object obj, string propertyName)
    {
        return (T)obj.GetType().GetRuntimeProperties().FirstOrDefault(pi => pi.Name == propertyName)?.GetValue(obj);
    }

    public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
    {
        obj.GetType().GetRuntimeProperties().FirstOrDefault(pi => pi.Name == propertyName)?.SetValue(obj, value);
    }

    public static MethodInfo GetMethod(this object obj, string methodName)
    {
        return obj.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == methodName);
    }

    public static Assembly CurrentAssembly
    {
        get
        {
            StackFrame[] frames = new StackTrace().GetFrames();

            foreach (StackFrame frame in frames)
            {
                Type decl = frame.GetMethod().DeclaringType;

                if (decl?.Name == "CompiledScript")
                {
                    return decl.Assembly;
                }
            }

            return null;
        }
    }
}
