using System.Runtime.InteropServices;

using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    public static unsafe PipeResponse Write(WriteRequest request)
    {
        Log("  => [Write] Dereferencing offsets...");

        nuint result = (nuint)request.BaseAddress;
        int* offsets = (int*)request.Offsets;

        for (int i = 0; i < request.OffsetsLength; i++)
        {
            result = *(nuint*)result;
            if (result == 0)
            {
                Log("    => [Write] Failure. Cannot dereference null pointer.");
                return PipeResponse.DerefFailure;
            }

            result += (nuint)offsets[i];
        }

        Log($"    => [Write] Success.");
        Log($"               Result: 0x{result:X}.");

        Log($"  => [Write] Writing data ({request.DataLength} bytes)...");

        NativeMemory.Copy((void*)result, (void*)request.Data, request.DataLength);

        Log("    => [Write] Success.");

        return PipeResponse.Success;
    }
}
