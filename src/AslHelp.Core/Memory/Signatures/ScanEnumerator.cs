namespace AslHelp.Core.Memory.Signatures;

public unsafe ref struct ScanIterator
{
    private const int UNROLLS = 8;

    private readonly ReadOnlySpan<byte> _memory;
    private readonly ReadOnlySpan<ulong> _values;
    private readonly ReadOnlySpan<ulong> _masks;

    private readonly bool _hasMasks;
    private readonly int _length;
    private readonly int _end;

    private readonly Span<int> _align = new int[9];

    private int _next;

    public ScanIterator(ReadOnlySpan<byte> memory, Signature signature, int alignment = 1)
    {
        _memory = memory;
        _values = signature.Values;
        _masks = signature.Masks;

        _hasMasks = signature.HasMasks;
        _length = signature.Values.Length;
        _end = memory.Length - _length - UNROLLS;

        for (int i = 1; i <= 8; i++)
        {
            _align[i] = alignment * i;
        }
    }

    public int Current { get; private set; }

    public bool MoveNext()
    {
        return _hasMasks ? NextPattern() : NextBytes();
    }

    private unsafe bool NextPattern()
    {
        int length = _length, end = _end, next = _next;

        fixed (byte* pMemory = _memory)
        fixed (int* pAlign = _align)
        fixed (ulong* pValues = _values, pMasks = _masks)
        {
            ulong value0 = pValues[0], mask0 = pMasks[0];

            while (next < end)
            {
                if ((*(ulong*)pMemory[next] & mask0) != value0)
                {
                    if ((*(ulong*)pMemory[next + pAlign[1]] & mask0) != value0)
                    {
                        if ((*(ulong*)pMemory[next + pAlign[2]] & mask0) != value0)
                        {
                            if ((*(ulong*)pMemory[next + pAlign[3]] & mask0) != value0)
                            {
                                if ((*(ulong*)pMemory[next + pAlign[4]] & mask0) != value0)
                                {
                                    if ((*(ulong*)pMemory[next + pAlign[5]] & mask0) != value0)
                                    {
                                        if ((*(ulong*)pMemory[next + pAlign[6]] & mask0) != value0)
                                        {
                                            if ((*(ulong*)pMemory[next + pAlign[7]] & mask0) != value0)
                                            {
                                                next += pAlign[8];
                                                goto Next;
                                            }
                                            else
                                            {
                                                next += pAlign[7];
                                            }
                                        }
                                        else
                                        {
                                            next += pAlign[6];
                                        }
                                    }
                                    else
                                    {
                                        next += pAlign[5];
                                    }
                                }
                                else
                                {
                                    next += pAlign[4];
                                }
                            }
                            else
                            {
                                next += pAlign[3];
                            }
                        }
                        else
                        {
                            next += pAlign[2];
                        }
                    }
                    else
                    {
                        next += pAlign[1];
                    }
                }

                int match = next + sizeof(ulong);

                for (int i = 1; i < length; i++)
                {
                    if ((*(ulong*)pMemory[match] & pMasks[i]) != pValues[i])
                    {
                        goto Next;
                    }

                    match += sizeof(ulong);
                }

                Current = next;
                _next = next + pAlign[1];

                return true;

            Next:
                ;
            }

            return false;
        }
    }

    private unsafe bool NextBytes()
    {
        int length = _length, end = _end, next = _next;

        fixed (byte* pMemory = _memory)
        fixed (int* pAlign = _align)
        fixed (ulong* pValues = _values)
        {
            ulong value0 = pValues[0];

            while (next < end)
            {
                if (*(ulong*)pMemory[next] != value0)
                {
                    if (*(ulong*)pMemory[next + pAlign[1]] != value0)
                    {
                        if (*(ulong*)pMemory[next + pAlign[2]] != value0)
                        {
                            if (*(ulong*)pMemory[next + pAlign[3]] != value0)
                            {
                                if (*(ulong*)pMemory[next + pAlign[4]] != value0)
                                {
                                    if (*(ulong*)pMemory[next + pAlign[5]] != value0)
                                    {
                                        if (*(ulong*)pMemory[next + pAlign[6]] != value0)
                                        {
                                            if (*(ulong*)pMemory[next + pAlign[7]] != value0)
                                            {
                                                next += pAlign[8];
                                                goto Next;
                                            }
                                            else
                                            {
                                                next += pAlign[7];
                                            }
                                        }
                                        else
                                        {
                                            next += pAlign[6];
                                        }
                                    }
                                    else
                                    {
                                        next += pAlign[5];
                                    }
                                }
                                else
                                {
                                    next += pAlign[4];
                                }
                            }
                            else
                            {
                                next += pAlign[3];
                            }
                        }
                        else
                        {
                            next += pAlign[2];
                        }
                    }
                    else
                    {
                        next += pAlign[1];
                    }
                }

                int match = next + sizeof(ulong);

                for (int i = 1; i < length; i++)
                {
                    if (*(ulong*)pMemory[match] != pValues[i])
                    {
                        goto Next;
                    }

                    match += sizeof(ulong);
                }

                Current = next;
                _next = next + pAlign[1];

                return true;

            Next:
                ;
            }

            return false;
        }
    }

    public void Reset()
    {
        _next = 0;
    }

    public ScanIterator GetEnumerator()
    {
        return this;
    }
}
