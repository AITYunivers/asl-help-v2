using System.IO;
using System.Reflection;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Ipc;

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
            ThrowHelper.ThrowFileNotFoundException(resource, "Unable to find the specified resource.");
        }

        return resourceStream;
    }

    public static bool TryInjectDllFromEmbeddedResource(IMemoryInjector injector, string resource, string unpackDirectory, string? entryPoint = null)
    {
        string targetFile = Path.GetFullPath($"{unpackDirectory}/{resource}");

        if (injector.IsInjected(targetFile))
        {
            return true;
        }

        try
        {
            Unpack(resource, targetFile);
        }
        catch (IOException ex) when ((uint)ex.HResult == 0x80070020)
        {
            return false;
        }

        return injector.TryInjectDll(targetFile, entryPoint);
    }
}
