#include "pch.h"

DWORD WINAPI ThreadMain(LPVOID lpParameter)
{
}

BOOL APIENTRY DllMain(HINSTANCE hintDll, DWORD dwReason, LPVOID lpvReserved)
{
    switch (dwReason)
    {
    case DLL_PROCESS_ATTACH:
        CreateThread(NULL, 0, ThreadMain, NULL, 0, NULL);
        break;
    case DLL_PROCESS_DETACH:
        break;
    }
}
