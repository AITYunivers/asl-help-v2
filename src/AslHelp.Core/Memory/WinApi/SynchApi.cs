using System.Runtime.InteropServices;

namespace AslHelp.Core.Memory;

internal static unsafe partial class WinApi
{
    /// <summary>
    ///     Waits until the specified object is in the signaled state or the time-out interval elapses.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/synchapi/nf-synchapi-waitforsingleobject">WaitForSingleObject function (synchapi.h)</see></i>.
    /// </summary>
    /// <param name="hHandle">A handle to the object.</param>
    /// <param name="dwMilliseconds">The time-out interval, in milliseconds.</param>
    /// <returns>
    ///     The event that caused the function to return.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int WaitForSingleObject(
        void* hHandle,
        uint dwMilliseconds);
}
