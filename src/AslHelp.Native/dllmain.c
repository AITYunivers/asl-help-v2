#include "pch.h"
#include "io/pipe.h"
#include "io/console.h"

DWORD WINAPI ThreadMain(void* lpParameter)
{
    while (TRUE)
    {
        if (!PipeValid())
        {
            break;
        }

        if (!ConnectPipe())
        {
            continue;
        }

        while (TRUE)
        {
            PipeRequestCode code;
            ReadValue(&code, sizeof(PipeRequestCode));

            switch (code)
            {
            case Read: {
                uint8_t* addr;
                ReadValue(&addr, sizeof(void*));

                int offsetsLen;
                ReadValue(&offsetsLen, sizeof(int));

                int* offsets = (int*)malloc(offsetsLen * sizeof(int));
                ReadValue(offsets, offsetsLen * sizeof(int));

                for (int i = 0; i < offsetsLen; ++i)
                {
                    addr = (uint8_t*)(*(uintptr_t*)addr + offsets[i]);
                }

                int typeSize;
                ReadValue(&typeSize, sizeof(int));

                void* data = *addr;
                WriteValue(data, typeSize);

                free(offsets);

                break;
            }
            }
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
