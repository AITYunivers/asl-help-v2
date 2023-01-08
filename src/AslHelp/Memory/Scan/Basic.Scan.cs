using System.Runtime.CompilerServices;

public partial class Basic
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<nint> UnpureScanEnumerator(
        Span<byte> memory,
        int offset,
        ReadOnlySpan<ulong> values,
        ReadOnlySpan<ulong> masks)
    {
        const int UNROLLS = 8;

        int length = values.Length;
        int last = memory.Length - length - UNROLLS;
        ulong value0 = values[0], mask0 = masks[0];

        int index = 0, matchIndex;
        while (index < last)
        {
            if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 0]) & mask0) != value0)
            {
                if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 1]) & mask0) != value0)
                {
                    if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 2]) & mask0) != value0)
                    {
                        if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 3]) & mask0) != value0)
                        {
                            if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 4]) & mask0) != value0)
                            {
                                if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 5]) & mask0) != value0)
                                {
                                    if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 6]) & mask0) != value0)
                                    {
                                        if ((Unsafe.ReadUnaligned<ulong>(ref memory[index + 7]) & mask0) != value0)
                                        {
                                            index += 8;
                                            goto Next;
                                        }
                                        else
                                        {
                                            index += 7;
                                        }
                                    }
                                    else
                                    {
                                        index += 6;
                                    }
                                }
                                else
                                {
                                    index += 5;
                                }
                            }
                            else
                            {
                                index += 4;
                            }
                        }
                        else
                        {
                            index += 3;
                        }
                    }
                    else
                    {
                        index += 2;
                    }
                }
                else
                {
                    index += 1;
                }
            }

            matchIndex = index + sizeof(ulong);

            for (int j = 1; j < length; j++)
            {
                ulong expected = Unsafe.ReadUnaligned<ulong>(ref memory[matchIndex]) & masks[j];
                if (expected != values[j])
                {
                    goto Next;
                }

                matchIndex += sizeof(ulong);
            }

            yield return index + offset;

        Next:
            ;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<nint> PureScanEnumerator(
        Span<byte> memory,
        int offset,
        ReadOnlySpan<ulong> values)
    {
        const int UNROLLS = 8;

        int length = values.Length;
        int last = memory.Length - length - UNROLLS;
        ulong value0 = values[0];

        int index = 0, matchIndex;
        while (index < last)
        {
            if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 0]) != value0)
            {
                if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 1]) != value0)
                {
                    if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 2]) != value0)
                    {
                        if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 3]) != value0)
                        {
                            if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 4]) != value0)
                            {
                                if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 5]) != value0)
                                {
                                    if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 6]) != value0)
                                    {
                                        if (Unsafe.ReadUnaligned<ulong>(ref memory[index + 7]) != value0)
                                        {
                                            index += 8;
                                            goto Next;
                                        }
                                        else
                                        {
                                            index += 7;
                                        }
                                    }
                                    else
                                    {
                                        index += 6;
                                    }
                                }
                                else
                                {
                                    index += 5;
                                }
                            }
                            else
                            {
                                index += 4;
                            }
                        }
                        else
                        {
                            index += 3;
                        }
                    }
                    else
                    {
                        index += 2;
                    }
                }
                else
                {
                    index += 1;
                }
            }

            matchIndex = index + sizeof(ulong);

            for (int j = 1; j < length; j++)
            {
                ulong expected = Unsafe.ReadUnaligned<ulong>(ref memory[matchIndex]);
                if (expected != values[j])
                {
                    goto Next;
                }

                matchIndex += sizeof(ulong);
            }

            yield return index + offset;

        Next:
            ;
        }
    }
}
