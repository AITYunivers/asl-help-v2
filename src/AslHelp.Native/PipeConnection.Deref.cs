using AslHelp.Common.Memory.Ipc;

namespace AslHelp.Native;

public static partial class PipeConnection
{
    public static unsafe PipeResponse Deref(DerefRequest request)
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

        *(nuint*)request.ResultPtr = result;
        return PipeResponse.Success;
    }
}
