using AslHelp.Core.Exceptions;

namespace AslHelp.Core.IO;

internal static class ResourceManager
{
    public static string UnpackResource(string resource, string directory)
    {
        using Stream source = GetResourceStream(resource);
        using FileStream destination = OpenWrite(directory, resource);

        source.CopyTo(destination);

        return destination.Name;
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

    public static FileStream OpenWrite(string fileName)
    {
        string path = Path.GetFullPath(fileName);
        return File.OpenWrite(path);
    }

    public static FileStream OpenWrite(string directory, string fileName)
    {
        string path = Path.GetFullPath(directory);
        if (!Directory.Exists(path))
        {
            ThrowHelper.Throw.DirectoryNotFound();
        }

        path = Path.Combine(path, fileName);

        return File.OpenWrite(path);
    }
}
