using System.Runtime.InteropServices;

using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    public static unsafe PipeResponse Read(ReadRequest request)
    {
        Log("  => [Read] Dereferencing offsets...");

        nuint result = (nuint)request.BaseAddress;
        int* offsets = (int*)request.Offsets;

        for (int i = 0; i < request.OffsetsLength; i++)
        {
            result = *(nuint*)result;
            if (result == 0)
            {
                Log("    => [Read] Failure. Cannot dereference null pointer.");
                return PipeResponse.DerefFailure;
            }

            result += (nuint)offsets[i];
        }

        Log($"    => [Read] Success.");
        Log($"              Result: 0x{result:X}.");

        Log($"  => [Read] Reading data ({request.BufferLength} bytes)...");

        NativeMemory.Copy((void*)result, (void*)request.Buffer, request.BufferLength);

        Log("    => [Read] Success.");

        return PipeResponse.Success;
    }
}
