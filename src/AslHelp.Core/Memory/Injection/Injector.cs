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
            throw new IOException($"File '{modulePath}' does not exist.");
        }

        nint libraryAlloc = process.AllocateRemoteString(modulePath);

        return true;
    }
}
