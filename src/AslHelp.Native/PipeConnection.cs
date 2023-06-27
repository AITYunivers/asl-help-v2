using System.Diagnostics;
using System.IO.Pipes;
using System.Threading.Tasks;

using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class PipeConnection
{
    public const string Name = "asl-help-pipe";

    private static NamedPipeServerStream? _pipe;

    public static async Task Main()
    {
        _pipe = new(Name);
        await _pipe.WaitForConnectionAsync();

        while (true)
        {
            if (!_pipe.TryRead(out PipeRequest cmd))
            {
                Debug.WriteLine("Failed reading command!");
                break;
            }

            if (cmd == PipeRequest.Close)
            {
                Debug.WriteLine("Closing pipe connection.");
                break;
            }

            HandleRequest(cmd);
        }

        _pipe.Disconnect();
        _pipe.Dispose();
        _pipe = null;
    }

    private static void HandleRequest(PipeRequest cmd)
    {
        if (_pipe is null)
        {
            return;
        }

        if (!_pipe.IsConnected)
        {
            _pipe.Write(PipeResponse.PipeClosed);
            return;
        }

        _pipe.Write(cmd switch
        {
            PipeRequest.Deref => _pipe.TryRead(out DerefRequest request) ? Deref(request) : PipeResponse.ReceiveFailure,
            PipeRequest.Read => _pipe.TryRead(out ReadRequest request) ? Read(request) : PipeResponse.ReceiveFailure,
            PipeRequest.Write => _pipe.TryRead(out WriteRequest request) ? Write(request) : PipeResponse.ReceiveFailure,
            _ => PipeResponse.UnknownCommand
        });
    }
}
