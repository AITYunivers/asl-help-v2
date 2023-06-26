using System.Runtime.InteropServices;

using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class PipeConnection
{
    public static unsafe PipeResponse Read(ReadRequest request)
    {
        nuint result = (nuint)request.BaseAddress;
        int* offsets = (int*)request.Offsets;

        for (int i = 0; i < request.OffsetsLength; i++)
        {
            result = *(nuint*)result;
            if (result == 0)
            {
                return PipeResponse.DerefFailure;
            }

            result += (nuint)offsets[i];
        }

        NativeMemory.Copy((void*)request.Buffer, (void*)result, request.BufferLength);
        return PipeResponse.Success;
    }
}
