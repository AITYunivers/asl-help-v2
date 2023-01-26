#include "pipe.h"

BOOL InitPipe(void)
{
    pipe = CreateNamedPipe(PIPE_NAME, PIPE_ACCESS_DUPLEX, PIPE_MODE_MESSAGE, PIPE_UNLIMITED_INSTANCES, BUFSIZE, BUFSIZE,
                           NMPWAIT_USE_DEFAULT_WAIT, NULL);

    if (pipe == INVALID_HANDLE_VALUE)
    {
        return FALSE;
    }

    DWORD mode = PIPE_READMODE_MESSAGE;
    if (!SetNamedPipeHandleState(pipe, &mode, NULL, NULL))
    {
        return FALSE;
    }

    return TRUE;
}

BOOL PipeValid(void)
{
    return pipe != NULL && pipe != INVALID_HANDLE_VALUE;
}

BOOL ConnectPipe(void)
{
    return ConnectNamedPipe(pipe, NULL);
}

BOOL Read(void* buffer, DWORD bufferLen)
{
    DWORD read;

    if (!ReadFile(pipe, buffer, bufferLen, &read, NULL))
    {
        return FALSE;
    }

    char msg[256];
    sprintf_s(msg, 256, "Expected: %d, Read: %d\n", bufferLen, read);
    Log(msg);

    return read == bufferLen;
}

BOOL Write(void* data, DWORD dataLen)
{
    DWORD written;

    if (!WriteFile(pipe, data, dataLen, &written, NULL))
    {
        return FALSE;
    }

    char msg[256];
    sprintf_s(msg, 256, "Expected: %d, Wrote: %d\n", dataLen, written);
    Log(msg);

    return written == dataLen;
}

BOOL DisconnectPipe(void)
{
    return DisconnectNamedPipe(pipe);
}

BOOL DisposePipe(void)
{
    return DisconnectPipe() && CloseHandle(pipe);
}
