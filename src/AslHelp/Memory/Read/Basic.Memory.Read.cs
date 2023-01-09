public partial class Basic
{
    public bool TryRead<T>(out T result, nint address, params int[] offsets) where T : unmanaged
    {
        result = default;
        return true;
    }
}
