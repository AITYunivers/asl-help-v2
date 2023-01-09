public partial class Basic
{
    public bool Write<T>(T value, nint address, params int[] offsets) where T : unmanaged
    {
        return true;
    }
}
