#pragma once

#include <Windows.h>
#include <format>
#include <string>

namespace IO::Logging
{

class ILogger
{
public:
    virtual ~ILogger() = default;

    template <typename... Args>
    bool Log(const std::string& format, Args... args) const
    {
        auto message = std::format(format, std::forward<Args>(args)...);
        return LogImpl(message);
    }

protected:
    virtual bool LogImpl(const std::string& message) = 0;
};

class ConsoleLogger : public ILogger
{
public:
    ConsoleLogger()
        : _hConsole(GetStdHandle(STD_OUTPUT_HANDLE))
    {
    }

protected:
    bool LogImpl(const std::string& message) override
    {
        unsigned long bytesWritten;
        return WriteFile(_hConsole, message.c_str(), static_cast<unsigned long>(message.size()), &bytesWritten, nullptr)
            && bytesWritten == message.size();
    }

private:
    HANDLE _hConsole;
};

} // namespace IO::Logging
