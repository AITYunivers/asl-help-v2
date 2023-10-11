using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Unity.Collections;

namespace AslHelp.Unity.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    private bool TryReadNetFx20Dictionary<TKey, TValue>(nuint address, [NotNullWhen(true)] out IReadOnlyDictionary<TKey, TValue>? result)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        if (!TryReadArray(out int[]? table, address + (PointerSize * 2U)))
        {
            goto ReturnFalse;
        }

        if (!TryReadArray(out Link[]? linkSlots, address + (PointerSize * 3U)))
        {
            goto ReturnFalse;
        }

        if (!TryReadArray(out TKey[]? keySlots, address + (PointerSize * 4U)))
        {
            goto ReturnFalse;
        }

        if (!TryReadArray(out TValue[]? valueSlots, address + (PointerSize * 5U)))
        {
            goto ReturnFalse;
        }

        if (!TryRead(out int count, address + (PointerSize * 6U) + (sizeof(int) * 2)))
        {
            goto ReturnFalse;
        }

        result = new NetFx20Dictionary<TKey, TValue>(count, table, linkSlots, keySlots, valueSlots);
        return true;

    ReturnFalse:
        result = null;
        return false;
    }

    private bool TryReadNetFx40Dictionary<TKey, TValue>(nuint address, [NotNullWhen(true)] out IReadOnlyDictionary<TKey, TValue>? result)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        if (!TryReadArray(out int[]? buckets, address + (PointerSize * 2U)))
        {
            goto ReturnFalse;
        }

        if (!Is64Bit && (IsNativeInt<TKey>() || IsNativeInt<TValue>()))
        {
            const string msg = "Reading a Dictionary containing IntPtr or UIntPtr is not supported for 32-bit processes.";
            ThrowHelper.ThrowNotSupportedException(msg);
        }

        if (!TryReadArray(out NetFx40Dictionary<TKey, TValue>.Entry[]? entries, address + (PointerSize * 3U)))
        {
            goto ReturnFalse;
        }

        if (!TryRead(out int count, address + (PointerSize * 8U)))
        {
            goto ReturnFalse;
        }

        result = new NetFx40Dictionary<TKey, TValue>(count, buckets, entries);
        return true;

    ReturnFalse:
        result = null;
        return false;
    }
}
