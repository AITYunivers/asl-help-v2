#include <windows.h>

#include <IO/Fwd.hxx>
#include <Memory/MemoryRequestHandler.hxx>

using namespace std;
using namespace IO;
using namespace IO::Logging;
using namespace Memory;

static DWORD WINAPI Main(LPVOID)
{
#ifdef _DEBUG
    AllocConsole();
#endif

    const auto logger =
#ifdef _DEBUG
        LoggingService::Create<ConsoleLogger>();
#else
        nullptr;
#endif

    auto pipe = NamedPipeServer::Init(L"\\\\.\\pipe\\asl-help-pipe", 512);
    if (!pipe.has_value())
    {
        DEBUG_LOG(logger, "Failed to initialize named pipe.");
        return 1;
    }

    while (true)
    {
        if (!pipe->Connect())
        {
            continue;
        }

        DEBUG_LOG(logger, "Connected to pipe.");

        auto memory = MemoryRequestHandler(pipe.value());
        while (true)
        {
            auto cmd = pipe->TryRead<IO::PipeRequest>();
            if (!cmd.has_value())
            {
                DEBUG_LOG(logger, "Failed reading command ({}).", GetLastError());

                if (GetLastError() == ERROR_BROKEN_PIPE)
                {
                    break;
                }

                continue;
            }

            DEBUG_LOG(logger, "Received command: {}.", static_cast<int>(cmd.value()));

            if (cmd == IO::PipeRequest::Close)
            {
                DEBUG_LOG(logger, "  => Closing pipe connection.");
                break;
            }

            memory.HandleNextRequest(cmd.value());
        }

        pipe->Disconnect();
    }

    pipe->Dispose();

#ifdef _DEBUG
    FreeConsole();
#endif

    return 0;
}

BOOL WINAPI DllMain(HINSTANCE, DWORD fdwReason, LPVOID)
{
    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
        CreateThread(nullptr, 0, Main, nullptr, 0, nullptr);
        break;
    }

    return TRUE;
}
