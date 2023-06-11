using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AslHelp.Native;

public static class EntryPoint
{
    [UnmanagedCallersOnly(EntryPoint = nameof(DllMain))]
    public static unsafe uint DllMain(void* _)
    {
        Task.Run(AslHelpPipe.Main);

        return 0;
    }
}
