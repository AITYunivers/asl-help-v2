namespace AslHelp.Core.Memory;

internal static unsafe partial class WinApi
{
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern void* GetModuleHandleW(
        ushort* lpModuleName);

    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern void* GetProcAddress(
        void* hModule,
        sbyte* lpProcName);
}
