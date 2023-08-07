#pragma once

#include <memory>
#include <stdint.h>

#include "IO/Fwd.hxx"
#include "IO/Logging/Loggers.hxx"
#include "RequestTypes.hxx"

namespace Memory
{

class MemoryManager
{
public:
    IO::PipeResponse Handle(const IO::PipeRequest& cmd) const
    {
        switch (cmd)
        {
        case IO::PipeRequest::Deref:
            return MemOp<DerefRequest>();
        case IO::PipeRequest::Read:
            return MemOp<ReadRequest>();
        case IO::PipeRequest::Write:
            return MemOp<WriteRequest>();
        default:
            return IO::PipeResponse::UnknownCommand;
        }
    }

private:
    template <typename T>
        requires std::is_base_of_v<IRequest, T>
    IO::PipeResponse MemOp() const
    {
        auto request = _pipeServer.TryRead<T>();
        if (request.has_value())
        {
            return MemOp(request.value());
        }

        return IO::PipeResponse::ReceiveFailure;
    }

    IO::PipeResponse MemOp(DerefRequest request) const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (uint32_t i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        *(uintptr_t**)request.Address = deref;

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        return IO::PipeResponse::Success;
    }

    IO::PipeResponse MemOp(ReadRequest request) const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (uint32_t i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        DEBUG_LOG(_logger, "  => Reading data ({} bytes)...", request.BufferLength);

        __try
        {
            memcpy((void*)request.Buffer, deref, request.BufferLength);
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference result address.");
            return IO::PipeResponse::ReadFailure;
        }

        DEBUG_LOG(_logger, "    => Success.");
        return IO::PipeResponse::Success;
    }

    IO::PipeResponse MemOp(WriteRequest request) const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (uint32_t i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        DEBUG_LOG(_logger, "  => Writing data ({} bytes)...", request.DataLength);

        __try
        {
            memcpy(deref, (void*)request.Data, request.DataLength);
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference result address.");
            return IO::PipeResponse::WriteFailure;
        }

        DEBUG_LOG(_logger, "    => Success.");
        return IO::PipeResponse::Success;
    }

private:
    const std::unique_ptr<IO::NamedPipeServer> _pipeServer;
    const std::unique_ptr<IO::Logging::ConsoleLogger> _logger;
};

} // namespace Memory
