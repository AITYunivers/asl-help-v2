#pragma once

namespace IO
{

enum class PipeResponse
{
    Success,

    ReceiveFailure,
    DerefFailure,
    ReadFailure,
    WriteFailure,

    PipeClosed,
    UnknownCommand
};

}
