#pragma once

#include <stdint.h>

namespace Memory
{

struct IRequest
{
};

struct DerefRequest : public IRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t ResultPtr;
};

struct ReadRequest : public IRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t Buffer;
    uint32_t BufferLength;
};

struct WriteRequest : public IRequest
{
    uint64_t Address;
    uint64_t Offsets;
    uint32_t OffsetsLength;
    uint64_t Data;
    uint32_t DataLength;
};

} // namespace Memory
