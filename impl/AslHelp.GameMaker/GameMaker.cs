using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

public partial class GameMaker : Basic
{
    public uint MurmurHash3(string input, uint seed)
    {
        ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(input);
        return MurmurHash3(ref bytes, seed);
    }

    public uint MurmurHash3(ref ReadOnlySpan<byte> bytes, uint seed)
    {
        uint hash = seed;

        int length = bytes.Length;
        ref byte refBytes = ref MemoryMarshal.GetReference(bytes);
        ref uint end = ref Unsafe.Add(ref Unsafe.As<byte, uint>(ref refBytes), bytes.Length >> 2);

        if (length >= 4)
        {
            do
            {
                uint k = Unsafe.ReadUnaligned<uint>(ref refBytes);

                hash ^= murmur32Scramble(k);
                hash = rotateLeft(hash, 13);
                hash = (hash * 5) + 0xE6546B64;

                refBytes = ref Unsafe.Add(ref refBytes, sizeof(uint));
            } while (Unsafe.IsAddressLessThan(ref Unsafe.As<byte, uint>(ref refBytes), ref end));
        }

        int remainder = length & 3;
        if (remainder > 0)
        {
            uint k = refBytes;

            if (remainder > 1)
            {
                k |= (uint)Unsafe.Add(ref refBytes, 1) << 8;

                if (remainder > 2)
                {
                    k |= (uint)Unsafe.Add(ref refBytes, 2) << 16;
                }
            }

            hash ^= murmur32Scramble(k);
        }

        hash ^= (uint)length;
        hash ^= hash >> 16;
        hash *= 0x85EBCA6B;
        hash ^= hash >> 13;
        hash *= 0xC2B2AE35;
        hash ^= hash >> 16;

        return hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint rotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint murmur32Scramble(uint k)
        {
            k *= 0xCC9E2D51;
            k = rotateLeft(k, 15);
            k *= 0x1B873593;

            return k;
        }
    }
}
