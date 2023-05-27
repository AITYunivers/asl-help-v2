using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using AslHelp.Common.Resources;
using AslHelp.Common.Extensions;
using AslHelp.Common.Pipes;

namespace AslHelp.Native;

public static class EntryPoint
{
    [UnmanagedCallersOnly(EntryPoint = nameof(DllMain))]
    public static unsafe uint DllMain(void* _)
    {
        Thread backgroundThread = new(ThreadMain)
        {
            IsBackground = true
        };

        backgroundThread.Start();

        return 0;
    }

    private static void ThreadMain()
    {
        _pipe = new(Pipes.AslHelpPipeName);
        _pipe.WaitForConnection();

        while (true)
        {
            PipeRequestCode cmd = _pipe.Read<PipeRequestCode>();

            if (cmd == PipeRequestCode.ClosePipe)
            {
                break;
            }
        }

        _pipe.Disconnect();
        _pipe.Dispose();
        _pipe = null;
    }

    private static NamedPipeServerStream? _pipe;
}
