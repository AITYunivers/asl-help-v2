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

    if (offsets == NULL)
    {
        *result = 0;

        return PipeAllocFailure;
    }

    for (i32 i = 0; i < offsetCount; ++i)
    {
        address = *(iptr*)address;

        if (address == 0)
        {
            free(offsets);
            *result = 0;

            return PipeDerefFailure;
        }

        address += offsets[i];
    }

    free(offsets);
    *result = address;

    return PipeSuccess;
}

void OpDeref()
{
    iptr deref;
    PipeResponseCode code = Deref(&deref);

    WriteToPipe(&code, sizeof(PipeResponseCode));
    if (code == PipeSuccess)
    {
        i64 result = (i64)deref;
        WriteToPipe(&result, sizeof(i64));
    }
}

void OpRead()
{
    iptr deref;
    PipeResponseCode code = Deref(&deref);

    i32 bytes;
    ReadFromPipe(&bytes, sizeof(i32));

    WriteToPipe(&code, sizeof(PipeResponseCode));
    if (code == PipeSuccess)
    {
        WriteToPipe(deref, bytes);
    }
}

void OpWrite()
{
    iptr deref;
    PipeResponseCode code = Deref(&deref);

    i32 bytes;
    ReadFromPipe(&bytes, sizeof(i32));

    void* value = malloc(bytes);
    ReadFromPipe(value, bytes);

    if (value == NULL)
    {
        code = PipeAllocFailure;
    }
    else if (code == PipeSuccess)
    {
        memcpy(deref, value, bytes);
        free(value);
    }

    WriteToPipe(&code, sizeof(PipeResponseCode));
}
