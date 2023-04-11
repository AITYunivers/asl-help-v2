#include "operations.h"

PipeResponseCode Deref(iptr* result)
{
    i64 address;
    ReadFromPipe(&address, sizeof(i64));

    i32 offsetCount;
    ReadFromPipe(&offsetCount, sizeof(i32));

    i32 bytes = offsetCount * sizeof(i32);
    i32* offsets = malloc(bytes);
    ReadFromPipe(offsets, bytes);

    for (i32 i = 0; i < offsetCount; ++i)
    {
        address = *((iptr*)address);

        if (address == 0)
        {
            *result = 0;

            return PipeDerefFailure;
        }

        address += offsets[i];
    }

    *result = address;

    return PipeSuccess;
}

PipeResponseCode ReadValue(iptr* result, i32* size)
{
    iptr deref;
    PipeResponseCode code = Deref(&deref);

    ReadFromPipe(size, sizeof(i32));

    if (code == PipeSuccess)
    {
        *result = deref;
    }

    return code;
}
