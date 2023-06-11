using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AslHelp.Common.Extensions;
using AslHelp.Native.Commands;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    public const string Name = "asl-help-pipe";

    private static NamedPipeServerStream? _pipe;

    public static async Task Main()
    {
        _pipe = new(Name);
        await _pipe.WaitForConnectionAsync();

        while (true)
        {
            if (!_pipe.TryRead(out PipeRequestCode cmd))
            {
                break;
            }

            if (cmd == PipeRequestCode.ClosePipe)
            {
                break;
            }

            HandleRequest(cmd);
        }

        _pipe.Disconnect();
        _pipe.Dispose();
        _pipe = null;
    }

    private static void HandleRequest(PipeRequestCode cmd)
    {
        switch (cmd)
        {
            case PipeRequestCode.Deref:
            {
                PipeResponseCode response = Deref(out nint result);
                _pipe.Write(response);

                if (response == PipeResponseCode.Success)
                {
                    _pipe.Write<long>(result);
                }

                break;
            }
        }
    }

    private static unsafe bool TryInitConsole()
    {
        //const uint STD_OUTPUT_HANDLE = unchecked((uint)-11);

        if (AllocConsole() == 0)
        {
            return false;
        }

        // Redirect?

        return true;

        [DllImport("kernel32", ExactSpelling = true)]
        static extern int AllocConsole();

        //[DllImport("kernel32", ExactSpelling = true)]
        //static extern void* GetStdHandle(uint nStdHandle);
    }
}
