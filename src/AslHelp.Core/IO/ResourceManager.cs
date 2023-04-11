using AslHelp.Core.Exceptions;

namespace AslHelp.Core.IO;

internal static class ResourceManager
{
    public static void UnpackResource(string resource, string targetFileName)
    {
        using Stream source = GetResourceStream(resource);
        using FileStream destination = File.OpenWrite(targetFileName);

        source.CopyTo(destination);
    }

    public static Stream GetResourceStream(string resourceName)
    {
        Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (resourceStream is null)
        {
            ThrowHelper.Throw.FileNotFound(resourceName, "Unable to find the specified resource.");
        }

        return resourceStream;
    }
}
