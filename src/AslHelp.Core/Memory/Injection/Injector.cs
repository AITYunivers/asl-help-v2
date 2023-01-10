using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Memory.Injection;

internal static unsafe class Injector
{
    public static bool TryInject(this Process process, string modulePath, out nint moduleBaseAddress)
    {
        foreach (Module module in process.ModulesTh32())
        {
            if (module.FilePath == modulePath)
            {
                moduleBaseAddress = module.Base;
                return true;
            }
        }

        modulePath = Path.GetFullPath(modulePath);
        if (!File.Exists(modulePath))
        {
            ThrowHelper.Throw.FileNotFound(modulePath, $"Unable to find the specified file.");
        }

        nint libraryAlloc = process.AllocateRemoteString(modulePath);

        moduleBaseAddress = 0;
        return true;
    }
}
