using System.Runtime.InteropServices;

namespace AslHelp.Core.Memory.Native.Structs;

/// <summary>
///     Describes an entry from a list of the modules belonging to the specified process.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/ns-tlhelp32-moduleentry32w">MODULEENTRY32W structure (tlhelp32.h)</see></i>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable IDE1006
internal unsafe struct MODULEENTRY32W
{
    /// <summary>
    ///     The size of the structure, in bytes.
    /// </summary>
    public uint dwSize;

    /// <summary>
    ///     This member is no longer used, and is always set to one.
    /// </summary>
    public uint th32ModuleID;

    /// <summary>
    ///     The identifier of the process whose modules are to be examined.
    /// </summary>
    public uint th32ProcessID;

    /// <summary>
    ///     The load count of the module, which is not generally meaningful, and usually equal to 0xFFFF.
    /// </summary>
    public uint GlblcntUsage;

    /// <summary>
    ///     The load count of the module (same as <see cref="GlblcntUsage"/>), which is not generally meaningful, and usually equal to 0xFFFF.
    /// </summary>
    public uint ProccntUsage;

    /// <summary>
    ///     The base address of the module in the context of the owning process.
    /// </summary>
    public byte* modBaseAddr;

    /// <summary>
    ///     The size of the module, in bytes.
    /// </summary>
    public uint modBaseSize;

    /// <summary>
    ///     A handle to the module in the context of the owning process.
    /// </summary>
    public void* hModule;

    /// <summary>
    ///     The module name.
    /// </summary>
    public fixed ushort szModule[256];

    /// <summary>
    ///     The module path.
    /// </summary>
    public fixed ushort szExePath[260];

    public static uint Size => (uint)sizeof(MODULEENTRY32W);
}
#pragma warning restore IDE1006
