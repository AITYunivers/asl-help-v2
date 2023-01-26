#pragma once

#include "../pch.h"

#define PIPE_NAME L"\\\\.\\pipe\\MonoPipe"
#define BUFSIZE 512
#define GENERIC_READWRITE GENERIC_READ | GENERIC_WRITE
#define PIPE_MODE_MESSAGE PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT

HANDLE pipe;

BOOL InitPipe(void);
BOOL PipeValid(void);
BOOL ConnectPipe(void);
BOOL Read(void* buffer, DWORD bufferLen);
BOOL Write(void* data, DWORD dataLen);
BOOL DisconnectPipe(void);
BOOL DisposePipe(void);
