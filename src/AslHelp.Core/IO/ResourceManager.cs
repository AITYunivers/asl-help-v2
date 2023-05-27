using System.IO;
using System.Reflection;
using AslHelp.Common.Exceptions;

namespace AslHelp.Core.IO;

internal static class ResourceManager
{
    public static void UnpackResource(string resource, string targetFile)
    {
        using Stream source = GetResourceStream(resource);
        using FileStream destination = File.OpenWrite(targetFile);

        source.CopyTo(destination);
    }

    public static Stream GetResourceStream(string resourceName)
    {
        Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

        if (resourceStream is null)
        {
            ThrowHelper.ThrowFileNotFoundException(resourceName, "Unable to find the specified resource.");
        }

        return resourceStream;
    }
}
