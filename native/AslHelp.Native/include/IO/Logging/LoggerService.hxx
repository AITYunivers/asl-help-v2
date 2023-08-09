#pragma once

#include <memory>

#include "Loggers.hxx"

namespace IO::Logging
{

class LoggingService
{
public:
    template <typename T>
        requires std::is_base_of_v<ILogger, T>
    static std::unique_ptr<T> Create()
    {
        return std::make_unique<T>();
    }
};

} // namespace IO::Logging

#ifdef _DEBUG
#define DEBUG_LOG(logger, format, ...) logger->Log("[asl-help] [Pipe] " format "\n" __VA_OPT__(, ) __VA_ARGS__)
#else
#define DEBUG_LOG(logger, format, ...)
#endif
