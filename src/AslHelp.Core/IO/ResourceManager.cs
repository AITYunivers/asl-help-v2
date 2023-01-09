namespace AslHelp.Core.IO;

internal static class ResourceManager
{
    public static string Unpack(string resource, string directory = null)
    {
        directory = directory is null ? Directory.GetCurrentDirectory() : Path.GetFullPath(directory);

        if (!Directory.Exists(directory))
        {
            throw new IOException($"Directory '{directory}' does not exist.");
        }

        string file = Path.Combine(directory, resource);
        if (File.Exists(file))
        {
            return file;
        }

        using Stream source = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

        if (source is null)
        {
            throw new IOException($"Resource '{resource}' does not exist.");
        }

        using Stream destination = File.OpenWrite(file);

        source.CopyTo(destination);

        return file;
    }
}
