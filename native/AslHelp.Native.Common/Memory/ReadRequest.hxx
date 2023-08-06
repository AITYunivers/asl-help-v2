#pragma once

#include <stdint.h>

namespace Memory
{

struct ReadRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t Buffer;
    uint32_t BufferLength;
};

}
