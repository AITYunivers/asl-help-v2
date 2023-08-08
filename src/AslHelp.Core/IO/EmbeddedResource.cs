using System.IO;
using System.Reflection;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.IO;

internal static class EmbeddedResource
{
    public static void Unpack(string resource, string targetFile)
    {
        using Stream source = GetResourceStream(resource);
        using FileStream destination = File.OpenWrite(targetFile);

        source.CopyTo(destination);
    }

    public static Stream GetResourceStream(string resource)
    {
        Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);

        if (resourceStream is null)
        {
            ThrowHelper.ThrowFileNotFoundException($"Unable to find the specified resource '{resource}'.");
        }

        return resourceStream;
    }

    public static bool TryInject(nuint processHandle, string resource, string unpackDirectory)
    {
        string targetFile = Path.GetFullPath(Path.Combine(unpackDirectory, resource));

        try
        {
            Unpack(resource, targetFile);
        }
        catch (IOException ex) when ((uint)ex.HResult == 0x80070020) { }

        return WinInteropWrapper.TryInjectDll(processHandle, targetFile);
    }
}
