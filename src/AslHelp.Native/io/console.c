#include "console.h"

bool InitConsole(void)
{
    AllocConsole();
    s_console = GetStdHandle(STD_OUTPUT_HANDLE);

    return TRUE;
}

void Log(const char* cmd, const char* format, ...)
{
    if (!s_console)
        return;

    size_t cmdLen = strlen(cmd);
    char* cmdConverted;

    if (cmdLen >= 7)
    {
        if (!(cmdConverted = malloc(cmdLen + 1)))
            return;

        strcpy(cmdConverted, cmd);
    }
    else
    {
        if (!(cmdConverted = malloc(8)))
            return;

        i32 padLen = 7 - cmdLen, padLeft = padLen / 2, padRight = padLen - padLeft;

        for (i32 i = 0; i < padLeft; ++i)
            cmdConverted[i] = ' ';

        strncpy(cmdConverted + padLeft, cmd, cmdLen);

        for (i32 i = 0; i < padRight; ++i)
            cmdConverted[padLeft + cmdLen + i] = ' ';

        cmdConverted[7] = '\0';
    }

    strupr(cmdConverted);

    va_list args;
    va_start(args, format);
    size_t logMessageLength = vsnprintf(NULL, 0, format, args);
    va_end(args);

    char* logMessage = malloc(logMessageLength + 64);
    if (!logMessage)
        return;

    snprintf(logMessage, logMessageLength + 64, "Pipe :: [%s] ", cmdConverted);
    va_start(args, format);
    vsnprintf(logMessage + strlen(logMessage), logMessageLength + 64 - strlen(logMessage), format, args);
    va_end(args);

    strcat(logMessage, "\n");
    bool success = WriteFile(s_console, logMessage, strlen(logMessage), NULL, NULL);

    free(logMessage);
    free(cmdConverted);
}

bool DisposeConsole(void)
{
    if (!s_console)
        return TRUE;

    if (FreeConsole() && CloseHandle(s_console))
    {
        s_console = NULL;
        return TRUE;
    }
    else
    {
        return FALSE;
    }
}
