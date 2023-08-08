#pragma once

#include <Windows.h>
#include <__expected/expected.h>
#include <expected>
#include <memory>
#include <optional>
#include <string>

namespace IO
{

class NamedPipeServer
{
public:
    explicit NamedPipeServer(HANDLE hPipe)
        : _hPipe(hPipe)
        , _isConnected(false)
    {
    }

    [[nodiscard]]
    static std::optional<IO::NamedPipeServer> Init(std::wstring name, const unsigned long bufferSize = 1024)
    {
        auto hPipe =
            CreateNamedPipe(name.c_str(), PIPE_ACCESS_DUPLEX, PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
                            PIPE_UNLIMITED_INSTANCES, bufferSize, bufferSize, NMPWAIT_USE_DEFAULT_WAIT, nullptr);

        if (hPipe == INVALID_HANDLE_VALUE || hPipe == nullptr)
        {
            return std::nullopt;
        }

        unsigned long mode = PIPE_READMODE_MESSAGE;
        if (!SetNamedPipeHandleState(hPipe, &mode, nullptr, nullptr))
        {
            return std::nullopt;
        }

        return IO::NamedPipeServer{hPipe};
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
    std::optional<T> TryRead() const
    {
        T ret;
        unsigned long bytesRead;
        if (!ReadFile(_hPipe, &ret, sizeof(T), &bytesRead, nullptr))
        {
            return std::nullopt;
        }

        if (bytesRead != sizeof(T))
        {
            return std::nullopt;
        }

        return ret;
    }

    template <typename T>
    bool TryWrite(const T value) const
    {
        unsigned long bytesWritten;
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

} // namespace IO
