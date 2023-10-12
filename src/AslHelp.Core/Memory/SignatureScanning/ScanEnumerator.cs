using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AslHelp.Core.Memory.SignatureScanning;

public sealed unsafe class ScanEnumerator : IEnumerable<uint>, IEnumerator<uint>
{
    private const int UNROLLS = 8;

    private readonly byte[] _memory;
    private readonly ulong[] _values;
    private readonly ulong[]? _masks;

    private readonly int _length;
    private readonly int _end;

    private readonly uint[] _align = new uint[UNROLLS + 1];

    private uint _next;

    public ScanEnumerator(byte[] memory, Signature signature, uint alignment = 1)
    {
        _memory = memory;
        _values = signature.Values;
        _masks = signature.Masks;

        _length = signature.Values.Length;
        _end = memory.Length - _length - UNROLLS;

        for (uint i = 1; i <= UNROLLS; i++)
        {
            _align[i] = alignment * i;
        }
    }

    public uint Current { get; private set; }
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        int length = _length, end = _end;
        uint next = _next;

        fixed (byte* pMemory = _memory)
        fixed (uint* pAlign = _align)
        fixed (ulong* pValues = _values, pMasks = _masks)
        {
            ulong value0 = pValues[0], mask0 = pMasks[0];

            while (next < end)
            {
                if ((*(ulong*)(pMemory + next) & mask0) != value0)
                {
                    if ((*(ulong*)(pMemory + next + pAlign[1]) & mask0) != value0)
                    {
                        if ((*(ulong*)(pMemory + next + pAlign[2]) & mask0) != value0)
                        {
                            if ((*(ulong*)(pMemory + next + pAlign[3]) & mask0) != value0)
                            {
                                if ((*(ulong*)(pMemory + next + pAlign[4]) & mask0) != value0)
                                {
                                    if ((*(ulong*)(pMemory + next + pAlign[5]) & mask0) != value0)
                                    {
                                        if ((*(ulong*)(pMemory + next + pAlign[6]) & mask0) != value0)
                                        {
                                            if ((*(ulong*)(pMemory + next + pAlign[7]) & mask0) != value0)
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

                uint match = next + sizeof(ulong);

                for (int i = 1; i < length; i++)
                {
                    if ((*(ulong*)(pMemory + match) & pMasks[i]) != pValues[i])
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

    IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this;
    }

    public void Dispose() { }
}
