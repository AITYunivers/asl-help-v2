#pragma once

#include <stdint.h>

namespace Memory
{

struct WriteRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t Data;
    uint32_t DataLength;
};

}
