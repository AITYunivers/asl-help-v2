#include "pipe.h"
#include "console.h"

BOOL InitPipe(void)
{
    s_pipe = CreateNamedPipe(
        PIPE_NAME,
        PIPE_ACCESS_DUPLEX,
        PIPE_MODE_MESSAGE,
        PIPE_UNLIMITED_INSTANCES,
        BUFSIZE,
        BUFSIZE,
        NMPWAIT_USE_DEFAULT_WAIT,
        NULL);

    if (s_pipe == INVALID_HANDLE_VALUE)
    {
        return FALSE;
    }

    u32 mode = PIPE_READMODE_MESSAGE;
    if (!SetNamedPipeHandleState(s_pipe, &mode, NULL, NULL))
    {
        return FALSE;
    }

    return TRUE;
}

bool PipeIsValid(void)
{
    return s_pipe != NULL && s_pipe != INVALID_HANDLE_VALUE;
}

bool ConnectPipe(void)
{
    return ConnectNamedPipe(s_pipe, NULL);
}

bool ReadFromPipe(void* buffer, u32 bufferLen)
{
    u32 read;
    if (!ReadFile(s_pipe, buffer, bufferLen, &read, NULL))
        return FALSE;

    Log("Read", "Expected: %03d,  Read: %03d", bufferLen, read);

    return read == bufferLen;
}

bool WriteToPipe(void* data, u32 dataLen)
{
    u32 written;
    if (!WriteFile(s_pipe, data, dataLen, &written, NULL))
    {
        return FALSE;
    }

    Log("Write", "Expected: %03d, Wrote: %03d", dataLen, written);

    return written == dataLen;
}

bool DisconnectPipe(void)
{
    return DisconnectNamedPipe(s_pipe);
}

bool DisposePipe(void)
{
    if (!s_pipe)
        return TRUE;

    if (DisconnectPipe() && CloseHandle(s_pipe))
    {
        s_pipe = NULL;
        return TRUE;
    }
    else
    {
        return FALSE;
    }
}
