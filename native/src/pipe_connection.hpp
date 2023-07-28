#include <string>

#include <Windows.h>

constexpr LPCWSTR PIPE_NAME = L"\\\\.\\pipe\\AslHelp";
constexpr DWORD BUF_SIZE = 512;
constexpr DWORD PIPE_MODE_MESSAGE = PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT;

namespace AslHelp
{
namespace Native
{

class PipeConnection
{
public:
    PipeConnection(std::string const& pipeName)
        : _pipeName(pipeName)
        , _isConnected(false)
    {
    }

    ~PipeConnection()
    {
        if (_isConnected)
        {
            disconnect();
        }
    }

    [[nodiscard]]
    bool connect()
    {
        if (_isConnected)
        {
            return true;
        }

        auto pipe = CreateNamedPipe(
            PIPE_NAME,
            PIPE_ACCESS_DUPLEX,
            PIPE_MODE_MESSAGE,
            1,
            BUF_SIZE,
            BUF_SIZE,
            NMPWAIT_USE_DEFAULT_WAIT,
            nullptr);

        if (pipe == INVALID_HANDLE_VALUE)
        {
            return false;
        }

        unsigned long mode = PIPE_READMODE_MESSAGE;
        if (!SetNamedPipeHandleState(pipe, &mode, nullptr, nullptr))
        {
            return false;
        }

        _isConnected = true;
        return true;
    }

    bool disconnect()
    {

    }

    [[nodiscard]]
    std::string name() const
    {
        return _pipeName;
    }

    [[nodiscard]]
    bool is_connected() const
    {
        return _isConnected;
    }

    template<typename T>
    bool read(T& data)
    {

    }

    template<typename T>
    bool write(T const& data);

private:
    std::string const _pipeName;
    bool _isConnected;
};

} // namespace Native
} // namespace AslHelp
