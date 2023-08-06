#include <iostream>
#include <windows.h>

#include <AslHelp.Native.Common/IO/Fwd.hxx>
#include <AslHelp.Native.Common/Memory/MemoryManager.hxx>

using namespace std;

static unsigned long WINAPI Main(void* _)
{
#ifdef DEBUG
    IO::DebugLogger::Init();
#endif

    auto pipe = Singleton<IO::NamedPipeServer>::Instance();
    auto memory = Singleton<Memory::MemoryManager>::Instance();

    if (!pipe.Init(L"\\\\.\\pipe\\asl-help-pipe", 512))
    {
        DEBUG_LOG("Failed to initialize named pipe.");
        return 1;
    }

    if (!pipe.Connect())
    {
        DEBUG_LOG("Failed to connect to named pipe.");
        return 2;
    }

    while (true)
    {
        IO::PipeRequest cmd;
        if (!pipe.TryRead<IO::PipeRequest>(&cmd))
        {
            DEBUG_LOG("Failed reading command.");
            break;
        }

        DEBUG_LOG("Received command: %s.", cmd);

        if (cmd == IO::PipeRequest::Close)
        {
            DEBUG_LOG("  => Closing pipe connection.");
            break;
        }

        pipe.TryWrite<IO::PipeResponse>(memory.Handle(cmd));
    }

    pipe.Dispose();

#ifdef DEBUG
    IO::DebugLogger::Dispose();
#endif

    return 0;
}

extern "C"
{
    __declspec(dllexport) uint32_t AslHelp_Native_EntryPoint(void* _)
    {
        CreateThread(nullptr, 0, Main, nullptr, 0, nullptr);

        return 0;
    }
}
