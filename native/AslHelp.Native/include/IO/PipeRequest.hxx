#pragma once

namespace IO
{

enum class PipeRequest
{
    Close,
    Deref,
    Read,
    Write
};

} // namespace IO
