#pragma once

#include <stdint.h>

namespace Memory
{

struct DerefRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t ResultPtr;
};

}
