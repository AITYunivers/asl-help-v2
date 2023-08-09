#pragma once

#include <Windows.h>
#include <format>
#include <string>
#include <vector>

namespace IO::Logging
{

class ILogger
{
public:
    virtual ~ILogger() = default;

    template <typename... Args>
    constexpr bool Log(std::string& format, Args... args) const
    {
        auto message = std::format(format, std::make_format_args(args...));
        return LogImpl(message);
    }

protected:
    [[clang::noinline]]
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
