using System;

namespace AslHelp.Core.Memory.Native.Enums;

/// <summary>
///     Provides process-specific access rights.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights">Process Security and Access Rights</see></i>
/// </remarks>
[Flags]
internal enum ProcessAccess : uint
{
    /// <summary>
    ///     Required to terminate a process.
    /// </summary>
    TERMINATE = 0x0001,

    /// <summary>
    ///     Required to create a thread in the process.
    /// </summary>
    CREATE_THREAD = 0x0002,

    /// <summary>
    ///
    /// </summary>
    SET_SESSIONID = 0x0004,

    /// <summary>
    ///     Required to perform an operation on the address space of a process.
    /// </summary>
    VM_OPERATION = 0x0008,

    /// <summary>
    ///     Required to read memory in a process.
    /// </summary>
    VM_READ = 0x0010,

    /// <summary>
    ///     Required to write to memory in a process.
    /// </summary>
    VM_WRITE = 0x0020,

    /// <summary>
    ///     Required to duplicate a handle.
    /// </summary>
    DUP_HANDLE = 0x0040,

    /// <summary>
    ///     Required to use this process as the parent process.
    /// </summary>
    CREATE_PROCESS = 0x0080,

    /// <summary>
    ///     Required to set memory limits.
    /// </summary>
    SET_QUOTA = 0x0100,

    /// <summary>
    ///     Required to set certain information about a process.
    /// </summary>
    SET_INFORMATION = 0x0200,

    /// <summary>
    ///     Required to retrieve certain information about a process.
    /// </summary>
    QUERY_INFORMATION = 0x0400,

    /// <summary>
    ///     Required to suspend or resume a process.
    /// </summary>
    SUSPEND_RESUME = 0x0800,

    /// <summary>
    ///     Required to retrieve certain information about a process.
    /// </summary>
    QUERY_LIMITED_INFORMATION = 0x1000,

    /// <summary>
    ///
    /// </summary>
    SET_LIMITED_INFORMATION = 0x2000
}
