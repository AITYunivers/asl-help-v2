#include "console.h"

bool InitConsole(void)
{
    AllocConsole();
    s_console = GetStdHandle(STD_OUTPUT_HANDLE);

    return TRUE;
}

bool Log(const char* output)
{
    if (!s_console)
        return FALSE;

    return WriteFile(s_console, output, lstrlenA(output), NULL, NULL);
}

bool LogDw(const char* format, u32 value)
{
    if (!s_console)
        return FALSE;

    const u32 len = lstrlenA(format) + 18;
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


bool LogLastErr(const char* format)
{
    if (!s_console)
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

bool DisposeConsole(void)
{
    if (!s_console)
        return TRUE;

    return FreeConsole() && CloseHandle(s_console);
}
