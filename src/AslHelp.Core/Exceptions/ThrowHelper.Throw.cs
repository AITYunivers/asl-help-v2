using System.ComponentModel;

namespace AslHelp.Core.Exceptions;

internal static partial class ThrowHelper
{
    public static class Throw
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Argument(string paramName, string message)
        {
            throw new ArgumentException(message, paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentNull(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentNull(string paramName, string message)
        {
            throw new ArgumentNullException(paramName, message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ArgumentOutOfRange(string paramName, string message)
        {
            throw new ArgumentOutOfRangeException(message, paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DirectoryNotFound()
        {
            throw new DirectoryNotFoundException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DirectoryNotFound(string message)
        {
            throw new DirectoryNotFoundException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void FileNotFound(string message)
        {
            throw new FileNotFoundException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void FileNotFound(string fileName, string message)
        {
            throw new FileNotFoundException(message, fileName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Format(string message)
        {
            throw new FormatException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InvalidData(string message)
        {
            throw new InvalidDataException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InvalidOperation(string message)
        {
            throw new InvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void KeyNotFound(string message)
        {
            throw new KeyNotFoundException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Win32()
        {
            throw new Win32Exception();
        }
    }
}
