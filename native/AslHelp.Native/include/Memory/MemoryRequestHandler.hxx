#pragma once

#include <stdint.h>
#include <vector>

#include "IO/Fwd.hxx"
#include "IO/Logging/LoggerService.hxx"
#include "RequestTypes.hxx"

namespace Memory
{

struct Request
{
    uint64_t Address;
    int32_t OffsetsLength;
    int32_t Offsets[128];
    uint32_t Bytes;
};

class MemoryRequestHandler
{
public:
    MemoryRequestHandler(const IO::NamedPipeServer& pipeServer)
        : _pipeServer(pipeServer)
    {
    }

    void HandleNextRequest(const IO::PipeRequest& cmd) const
    {
        switch (cmd)
        {
        case IO::PipeRequest::Deref: {
            HandleDerefRequest();
            break;
        }
        case IO::PipeRequest::Read: {
            HandleReadRequest();
            break;
        }
        case IO::PipeRequest::Write: {
            HandleWriteRequest();
            break;
        }
        default:
            _pipeServer.TryWrite(IO::PipeResponse::UnknownCommand);
            break;
        }
    }

private:
    void HandleDerefRequest() const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto request = _pipeServer.TryRead<Request>();
        if (!request.has_value())
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot read request.");
            _pipeServer.TryWrite(IO::PipeResponse::ReceiveFailure);

            return;
        }

        auto deref = DereferencePath(request->Address, request->Offsets, request->OffsetsLength);
        if (deref == 0)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            _pipeServer.TryWrite(IO::PipeResponse::DerefFailure);

            return;
        }

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        _pipeServer.TryWrite(IO::PipeResponse::Success);
        _pipeServer.TryWrite((uint64_t)deref);
    }

    void HandleReadRequest() const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto request = _pipeServer.TryRead<Request>();
        if (!request.has_value())
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot read request.");
            _pipeServer.TryWrite(IO::PipeResponse::ReceiveFailure);

            return;
        }

        auto deref = DereferencePath(request->Address, request->Offsets, request->OffsetsLength);
        if (deref == 0)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            _pipeServer.TryWrite(IO::PipeResponse::DerefFailure);

            return;
        }

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        DEBUG_LOG(_logger, "  => Reading data ({} bytes)...", request->Bytes);

        std::vector<uint8_t> buffer(request->Bytes);

        if (TryCpy(buffer.data(), (void*)deref, request->Bytes))
        {
            DEBUG_LOG(_logger, "    => Success.");

            _pipeServer.TryWrite(IO::PipeResponse::Success);
            _pipeServer.TryWrite(buffer);
        }
        else
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference result address.");

            _pipeServer.TryWrite(IO::PipeResponse::ReadFailure);
        }
    }

    void HandleWriteRequest() const
    {
        DEBUG_LOG(_logger, "  => Dereferencing offsets...");

        auto request = _pipeServer.TryRead<Request>();
        auto data = _pipeServer.TryRead<uint8_t>(request->Bytes);
        if (!request.has_value() || !data.has_value())
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot read request.");
            _pipeServer.TryWrite(IO::PipeResponse::ReceiveFailure);

            return;
        }

        auto deref = DereferencePath(request->Address, request->Offsets, request->OffsetsLength);
        if (deref == 0)
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference null pointer.");
            _pipeServer.TryWrite(IO::PipeResponse::DerefFailure);

            return;
        }

        DEBUG_LOG(_logger, "    => Success.");
        DEBUG_LOG(_logger, "       Result: 0x{:X}.", deref);

        DEBUG_LOG(_logger, "  => Writing data ({} bytes)...", request->Bytes);

        if (TryCpy(data->data(), (void*)deref, request->Bytes))
        {
            DEBUG_LOG(_logger, "    => Success.");

            _pipeServer.TryWrite(IO::PipeResponse::Success);
        }
        else
        {
            DEBUG_LOG(_logger, "    => Failure. Cannot dereference result address.");

            _pipeServer.TryWrite(IO::PipeResponse::WriteFailure);
        }
    }

    static bool TryCpy(void* dst, const void* src, size_t size)
    {
        __try
        {
            memcpy(dst, src, size);
            return true;
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            return false;
        }
    }

    static intptr_t DereferencePath(uint64_t baseAddress, const int32_t* offsets, int32_t offsetsLength)
    {
        auto deref = (intptr_t)baseAddress;

        __try
        {
            for (auto i = 0; i < offsetsLength; ++i)
            {
                deref = *(intptr_t*)deref;
                if (deref == 0)
                {
                    return 0;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            return 0;
        }

        return (intptr_t)deref;
    }

private:
    const IO::NamedPipeServer& _pipeServer;
    const std::unique_ptr<IO::Logging::ConsoleLogger> _logger
#ifdef _DEBUG
        = IO::Logging::LoggingService::Create<IO::Logging::ConsoleLogger>();
#else
        = nullptr;
#endif
};

} // namespace Memory
