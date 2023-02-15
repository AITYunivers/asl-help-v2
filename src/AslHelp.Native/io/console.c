#include "console.h"

BOOL InitConsole(void)
{
    AllocConsole();
    console = GetStdHandle(STD_OUTPUT_HANDLE);

    return TRUE;
}

BOOL Log(const char* output)
{
    if (!console)
        return FALSE;

    return WriteFile(console, output, lstrlenA(output), NULL, NULL);
}

BOOL LogDw(const char* format, DWORD value)
{
    if (!console)
        return FALSE;

    const DWORD len = lstrlenA(format) + 18;
    const char* buf = malloc(len);

    if (buf == NULL)
    {
        return FALSE;
    }

    sprintf_s(buf, len, format, value);

    BOOL success = Log(buf);
    free(buf);

    return success;
}

BOOL LogLastErr(const char* format)
{
    if (!console)
        return FALSE;

    const DWORD len = lstrlenA(format) + 18;
    const char* buf = malloc(len);

    if (buf == NULL)
    {
        return FALSE;
    }

    sprintf_s(buf, len, format, GetLastError());

    BOOL success = Log(buf);
    free(buf);

    return success;
}

BOOL DisposeConsole(void)
{
    if (!console)
        return TRUE;

    return FreeConsole() && CloseHandle(console);
}
