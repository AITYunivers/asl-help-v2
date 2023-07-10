using System.Runtime.InteropServices;

using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    public static unsafe PipeResponse Read(ReadRequest request)
    {
        Log("  => Dereferencing offsets...");

        nuint result = (nuint)request.BaseAddress;
        int* offsets = (int*)request.Offsets;

        for (int i = 0; i < request.OffsetsLength; i++)
        {
            result = *(nuint*)result;
            if (result == 0)
            {
                Log("    => Failure. Cannot dereference null pointer.");
                return PipeResponse.DerefFailure;
            }

            result += (nuint)offsets[i];
        }

        Log($"    => Success.");
        Log($"       Result: 0x{result:X}.");

        Log($"  => Reading data ({request.BufferLength} bytes)...");

        try
        {
            NativeMemory.Copy((void*)result, (void*)request.Buffer, request.BufferLength);
        }
        catch
        {
            Log("    => Failure. Cannot dereference result address.");
            return PipeResponse.ReadFailure;
        }

        Log("    => Success.");
        return PipeResponse.Success;
    }
}
