#pragma once

#include "../pch.h"

#define PIPE_NAME L"\\\\.\\pipe\\asl-help-pipe"
#define BUFSIZE 512
#define GENERIC_READWRITE GENERIC_READ | GENERIC_WRITE
#define PIPE_MODE_MESSAGE PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT

void* s_pipe;

bool InitPipe(void);
bool PipeIsValid(void);
bool ConnectPipe(void);
bool ReadFromPipe(void* buffer, u32 bufferLen);
bool WriteToPipe(void* data, u32 dataLen);
bool DisconnectPipe(void);
bool DisposePipe(void);

typedef enum PipeRequestCommand
{
    PipeClose,

    PipeDeref,
    PipeRead,
    PipeReadSpan,
    PipeWrite,
    PipeWriteSpan
} PipeRequestCommand;

typedef enum PipeResponseCode
{
    PipeSuccess,

    PipeDerefFailure
} PipeResponseCode;
