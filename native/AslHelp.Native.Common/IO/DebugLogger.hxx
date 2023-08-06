#pragma once

#include <Windows.h>

namespace IO
{

class DebugLogger
{
public:
    static bool Init()
    {
        if (!AllocConsole())
        {
            return false;
        }

        _hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        if (_hConsole == INVALID_HANDLE_VALUE || _hConsole == nullptr)
        {
            return false;
        }

        _isInitialized = true;
        return true;
    }

    template<typename... Args>
    static bool Log(const char* format, Args... args)
    {
        if (!_isInitialized)
        {
            return false;
        }

        char buffer[1024];
        sprintf_s(buffer, "[asl-help] [Pipe] ");
        sprintf_s(buffer + 19, format, args...);

        unsigned long len = strlen(buffer) + 1, written;

        sprintf_s(len - 1, "\n");

        return WriteConsole(_hConsole, buffer, len, &written, nullptr)
            && written == len;
    }

    static bool Dispose()
    {
        if (_isInitialized)
        {
            _isInitialized = false;
            return FreeConsole() && CloseHandle(_hConsole);
        }

        return true;
    }

private:
    static HANDLE _hConsole;

    static bool _isInitialized;
};

}
