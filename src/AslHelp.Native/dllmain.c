#include "pch.h"
#include "io/pipe.h"
#include "io/console.h"
#include "memory/operations.h"

void HandlePipeCommand(PipeRequestCommand cmd)
{
    switch (cmd)
    {
    case PipeDeref:
        OpDeref();
        break;
    case PipeRead:
    case PipeReadSpan:
        OpRead();
        break;
    case PipeWrite:
    case PipeWriteSpan:
        OpWrite();
        break;
    }
}

DWORD WINAPI ThreadMain(void* lpParameter)
{
    while (TRUE)
    {
        if (!PipeIsValid())
        {
            break;
        }

        if (!ConnectPipe())
        {
            continue;
        }

        // InitConsole();

        while (TRUE)
        {
            PipeRequestCommand cmd;
            ReadFromPipe(&cmd, sizeof(PipeRequestCommand));

            if (cmd == PipeClose)
            {
                DisposeConsole();
                DisconnectPipe();

                break;
            }

            HandlePipeCommand(cmd);
        }
    }

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hintDll, DWORD dwReason, LPVOID lpvReserved)
{
    switch (dwReason)
    {
    case DLL_PROCESS_ATTACH: {
        if (!InitPipe())
        {
            return FALSE;
        }

        CreateThread(NULL, 0, ThreadMain, NULL, 0, NULL);
        break;
    }
    case DLL_PROCESS_DETACH: {
        break;
    }
    }

    return TRUE;
}
