public partial class Basic
{
    public bool WriteSpan<T>(T[] values, nint address, params int[] offsets) where T : unmanaged
    {
        return true;
    }
}
