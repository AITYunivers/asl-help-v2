#pragma once

#include <Windows.h>
#include <string>
#include <vector>
#include <wincon.h>

namespace IO::Logging
{

class ILogger
{
public:
    virtual ~ILogger() = default;

    template <typename... Args>
    bool Log(const std::string& format, Args... args) const
    {
        // auto message = std::vformat(format, std::make_format_args(args...));

        size_t size = snprintf(nullptr, 0, format.c_str(), args...) + 1;

        std::vector<char> buffer(size);
        sprintf_s(buffer.data(), size, format.c_str(), args...);

        return LogImpl(std::string(buffer.begin(), buffer.end()));
    }

protected:
    virtual bool LogImpl(const std::string& message) const = 0;
};

class ConsoleLogger : public ILogger
{
public:
    ConsoleLogger()
        : _hConsole(GetStdHandle(STD_OUTPUT_HANDLE))
    {
        AllocConsole();
    }

    ~ConsoleLogger() override
    {
        FreeConsole();
    }

protected:
    bool LogImpl(const std::string& message) const override
    {
        unsigned long bytesWritten;
        return WriteFile(_hConsole, message.c_str(), static_cast<unsigned long>(message.size()), &bytesWritten, nullptr)
            && bytesWritten == message.size();
    }

private:
    HANDLE _hConsole;
};

} // namespace IO::Logging
