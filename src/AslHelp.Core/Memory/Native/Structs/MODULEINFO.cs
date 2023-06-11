using System.Runtime.InteropServices;

namespace AslHelp.Core.Memory.Native.Structs;

/// <summary>
///     Contains the module load address, size, and entry point.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/ns-psapi-moduleinfo">MODULEINFO structure (psapi.h)</see></i>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct MODULEINFO
{
    /// <summary>
    ///     The load address of the module.
    /// </summary>
#pragma warning disable IDE1006
    public void* lpBaseOfDll;
#pragma warning restore IDE1006

    /// <summary>
    ///     The size of the linear space that the module occupies, in bytes.
    /// </summary>
    public uint SizeOfImage;

    /// <summary>
    ///     The entry point of the module.
    /// </summary>
    public void* EntryPoint;

    public static uint Size => (uint)sizeof(MODULEINFO);
}

