#pragma once

#include <memory>

#include "DerefRequest.hxx"
#include "ReadRequest.hxx"
#include "WriteRequest.hxx"

#include "../Singleton.hxx"
#include "../IO/Fwd.hxx"

namespace Memory
{

class MemoryManager
{
public:
    IO::PipeResponse Handle(const IO::PipeRequest& request) const
    {
        switch (request)
        {
        case IO::PipeRequest::Deref:
        {
            DerefRequest request;
            if (Singleton<IO::NamedPipeServer>::Instance().TryRead<DerefRequest>(&request))
            {
                return Deref(request);
            }

            break;
        }
        case IO::PipeRequest::Read:
        {
            ReadRequest request;
            if (Singleton<IO::NamedPipeServer>::Instance().TryRead<ReadRequest>(&request))
            {
                return Read(request);
            }

            break;
        }
        case IO::PipeRequest::Write:
        {
            WriteRequest request;
            if (Singleton<IO::NamedPipeServer>::Instance().TryRead<WriteRequest>(&request))
            {
                return Write(request);
            }

            break;
        }
        default:
        {
            return IO::PipeResponse::UnknownCommand;
        }
        }

        return IO::PipeResponse::ReceiveFailure;
    }

private:
    IO::PipeResponse Deref(DerefRequest request) const
    {
        DEBUG_LOG("  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (auto i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        *(uintptr_t**)request.Address = deref;

        DEBUG_LOG("    => Success.");
        DEBUG_LOG("       Result: 0x%p.", deref);

        return IO::PipeResponse::Success;
    }

    IO::PipeResponse Read(ReadRequest request) const
    {
        DEBUG_LOG("  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (auto i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        DEBUG_LOG("    => Success.");
        DEBUG_LOG("       Result: 0x%p.", deref);

        DEBUG_LOG("  => Reading data (%d bytes)...", request.BufferLength);

        __try
        {
            memcpy((void*)request.Buffer, deref, request.BufferLength);
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG("    => Failure. Cannot dereference result address.");
            return IO::PipeResponse::ReadFailure;
        }

        DEBUG_LOG("    => Success.");
        return IO::PipeResponse::Success;
    }

    IO::PipeResponse Write(WriteRequest request) const
    {
        DEBUG_LOG("  => Dereferencing offsets...");

        auto deref = (uintptr_t*)request.Address;
        auto offsets = reinterpret_cast<const int32_t*>(request.Offsets);

        __try
        {
            for (auto i = 0; i < request.OffsetsLength; ++i)
            {
                deref = (uintptr_t*)*deref;
                if (deref == nullptr)
                {
                    DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
                    return IO::PipeResponse::DerefFailure;
                }

                deref += offsets[i];
            }
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG("    => Failure. Cannot dereference null pointer.");
            return IO::PipeResponse::DerefFailure;
        }

        DEBUG_LOG("    => Success.");
        DEBUG_LOG("       Result: 0x%p.", deref);

        DEBUG_LOG("  => Writing data (%d bytes)...", request.BufferLength);

        __try
        {
            memcpy(deref, (void*)request.Data, request.DataLength);
        }
        __except (EXCEPTION_EXECUTE_HANDLER)
        {
            DEBUG_LOG("    => Failure. Cannot dereference result address.");
            return IO::PipeResponse::WriteFailure;
        }

        DEBUG_LOG("    => Success.");
        return IO::PipeResponse::Success;
    }
};

}
