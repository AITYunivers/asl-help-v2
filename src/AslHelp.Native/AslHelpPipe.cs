using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    public const string Name = "asl-help-pipe";

    private static NamedPipeServerStream? _pipe;

    [UnmanagedCallersOnly(EntryPoint = $"{nameof(AslHelpPipe)}.{nameof(EntryPoint)}")]
    public static unsafe uint EntryPoint(void* _)
    {
        Task.Run(Main);

        return 0;
    }

    private static async Task Main()
    {
        _pipe = new(Name);
        await _pipe.WaitForConnectionAsync();

        while (true)
        {
            if (!_pipe.TryRead(out PipeRequest cmd))
            {
                Log("Failed reading command!");
                break;
            }

            Log($"Received command: PipeRequest.{cmd}.");

            if (cmd == PipeRequest.Close)
            {
                Log("  => Closing pipe connection.");
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
            Log("  => Pipe is null. Command not handled.");
            return;
        }

        if (!_pipe.IsConnected)
        {
            _pipe.Write(PipeResponse.PipeClosed);

            Log("  => Pipe is not connected. Command not handled.");
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

    [Conditional("DEBUG")]
    private static void Log(object? output)
    {
        Debug.WriteLine($"[asl-help] [Pipe] {output}");
    }
}
