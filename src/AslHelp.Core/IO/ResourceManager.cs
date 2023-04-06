using AslHelp.Core.Exceptions;

namespace AslHelp.Core.IO;

internal static class ResourceManager
{
    public static string UnpackResource(string resource, string directory)
    {
        string outputFile = $"{directory}/{resource}";

        //if (File.Exists(outputFile))
        //{
        //    return outputFile;
        //}

        using Stream source = GetResourceStream(resource);
        using FileStream destination = File.OpenWrite(outputFile);

        source.CopyTo(destination);

        return outputFile;
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
