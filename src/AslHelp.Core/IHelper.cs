using LiveSplit.ComponentUtil;

namespace AslHelp.Core;

public interface IHelper : IDisposable
{
    uint Tick { get; }

    nint Deref(nint baseAddress, params int[] offsets);

    bool TryRead<T>(out T result, nint baseAddress, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(out T[] result, nint baseAddress, params int[] offsets) where T : unmanaged;
    bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    bool Write<T>(T value, nint baseAddress, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, nint baseAddress, params int[] offsets) where T : unmanaged;

    void Log(object output);
}
