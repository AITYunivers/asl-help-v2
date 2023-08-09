#include <windows.h>

#include <IO/Fwd.hxx>
#include <Memory/MemoryManager.hxx>

using namespace std;
using namespace IO;
using namespace IO::Logging;
using namespace Memory;

static unsigned long WINAPI Main(void*)
{
    const auto logger =
#ifdef _DEBUG
        LoggingService::Create<ConsoleLogger>();
#else
        nullptr;
#endif

    // auto pipe = NamedPipeServer::Init(L"\\\\.\\pipe\\asl-help-pipe", 512);
    // if (!pipe.has_value())
    // {
    //     DEBUG_LOG(logger, "Failed to initialize named pipe.");
    //     return 1;
    // }

    // if (!pipe->Connect())
    // {
    //     DEBUG_LOG(logger, "Failed to connect to named pipe.");
    //     return 2;
    // }

    // auto memory = MemoryManager(pipe.value());
    // while (true)
    // {
    //     auto cmd = pipe->TryRead<IO::PipeRequest>();
    //     if (!cmd.has_value())
    //     {
    //         DEBUG_LOG(logger, "Failed reading command.");
    //         break;
    //     }

    //     DEBUG_LOG(logger, "Received command: {}.", cmd);

    //     if (cmd == IO::PipeRequest::Close)
    //     {
    //         DEBUG_LOG(logger, "  => Closing pipe connection.");
    //         break;
    //     }

    //     auto response = memory.Handle(cmd.value());
    //     pipe->TryWrite<IO::PipeResponse>(response);
    // }

    // pipe->Dispose();

    return 0;
}

extern "C"
{
    __declspec(dllexport) uint32_t AslHelp_Native_EntryPoint(void*)
    {
        CreateThread(nullptr, 0, Main, nullptr, 0, nullptr);

        return 0;
    }
}
