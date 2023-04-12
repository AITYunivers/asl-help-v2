#include "pch.h"
#include "io/pipe.h"
#include "io/console.h"
#include "memory/operations.h"

void HandlePipeCommand(PipeRequestCommand cmd)
{
    if (cmd == PipeClose)
    {
        DisposeConsole();
        DisposePipe();

        return;
    }

    switch (cmd)
    {
    case PipeDeref: {
        i64 result;
        PipeResponseCode code = Deref(&result);

        WriteToPipe(&code, sizeof(PipeResponseCode));
        if (code == PipeSuccess)
        {
            WriteToPipe(&result, sizeof(i64));
        }

        break;
    }
    case PipeRead: {
        iptr result;
        i32 size;
        PipeResponseCode code = ReadValue(&result, &size);

        WriteToPipe(&code, sizeof(PipeResponseCode));
        if (code == PipeSuccess)
        {
            WriteToPipe(result, size);
        }

        break;
    }
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
                DisposePipe();

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
