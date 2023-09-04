using System.IO;
using System.Reflection;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.IO;

public static class EmbeddedResource
{
    public static void Unpack(string resource, string targetFile)
    {
        using Stream source = GetResourceStream(resource);
        using FileStream destination = File.OpenWrite(targetFile);

        source.CopyTo(destination);
    }

    public static Stream GetResourceStream(string resource)
    {
        return GetResourceStream(resource, Assembly.GetCallingAssembly());
    }

    public static Stream GetResourceStream(string resource, Assembly assembly)
    {
        System.Console.WriteLine(assembly.FullName);
        Stream? resourceStream = assembly.GetManifestResourceStream(resource);

        if (resourceStream is null)
        {
            string msg = $"Unable to find the specified resource '{resource}'.";
            ThrowHelper.ThrowFileNotFoundException(msg);
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
