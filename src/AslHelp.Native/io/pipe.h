#pragma once

#include "../pch.h"

#define PIPE_NAME L"\\\\.\\pipe\\asl-help-pipe"
#define BUFSIZE 512
#define GENERIC_READWRITE GENERIC_READ | GENERIC_WRITE
#define PIPE_MODE_MESSAGE PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT

HANDLE pipe;

BOOL InitPipe(void);
BOOL PipeValid(void);
BOOL ConnectPipe(void);
BOOL ReadValue(void* buffer, DWORD bufferLen);
BOOL WriteValue(void* data, DWORD dataLen);
BOOL DisconnectPipe(void);
BOOL DisposePipe(void);

typedef enum
{
    ClosePipe,

    Deref,
    Read,
    ReadSpan,
    Write,
    WriteSpan
} PipeRequestCode;

typedef enum
{
    Success,
    Failure
} PipeResponseCode;
