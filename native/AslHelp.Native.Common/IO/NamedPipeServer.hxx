#pragma once

#include <string>
#include <Windows.h>

namespace IO
{

class NamedPipeServer
{
public:
    NamedPipeServer() = default;
    ~NamedPipeServer()
    {
        Dispose();
    }

    [[nodiscard]]
    bool Init(std::wstring name, const unsigned long bufferSize = 1024)
    {
        _hPipe = CreateNamedPipe(
            name.c_str(),
            PIPE_ACCESS_DUPLEX,
            PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
            PIPE_UNLIMITED_INSTANCES,
            bufferSize,
            bufferSize,
            NMPWAIT_USE_DEFAULT_WAIT,
            nullptr);

        if (_hPipe == INVALID_HANDLE_VALUE || _hPipe == nullptr)
        {
            return false;
        }

        unsigned long mode = PIPE_READMODE_MESSAGE;
        return SetNamedPipeHandleState(_hPipe, &mode, nullptr, nullptr);
    }

    [[nodiscard]]
    bool Connect()
    {
        if (ConnectNamedPipe(_hPipe, nullptr))
        {
            _isConnected = true;
            return true;
        }

        return false;
    }

    bool IsConnected() const
    {
        return _isConnected;
    }

    bool IsValid() const
    {
        return _hPipe != INVALID_HANDLE_VALUE && _hPipe != nullptr && _isConnected;
    }

    template <typename T>
    [[nodiscard]]
    bool TryRead(T* value) const
    {
        DWORD bytesRead;
        if (!ReadFile(_hPipe, value, sizeof(T), &bytesRead, nullptr))
        {
            return false;
        }

        return bytesRead == sizeof(T);
    }

    template <typename T>
    [[nodiscard]]
    bool TryWrite(const T value) const
    {
        DWORD bytesWritten;
        if (!WriteFile(_hPipe, &value, sizeof(T), &bytesWritten, nullptr))
        {
            return false;
        }

        return bytesWritten == sizeof(T);
    }

    bool Disconnect()
    {
        if (!_isConnected)
        {
            return true;
        }

        if (DisconnectNamedPipe(_hPipe))
        {
            _isConnected = false;
            return true;
        }

        return false;
    }

    bool Dispose()
    {
        if (!IsValid())
        {
            return true;
        }

        if (Disconnect() && CloseHandle(_hPipe))
        {
            _hPipe = nullptr;
            return true;
        }

        return false;
    }

private:
    HANDLE _hPipe;

    bool _isConnected;
};

}
