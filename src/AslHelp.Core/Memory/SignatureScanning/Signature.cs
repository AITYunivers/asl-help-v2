using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;

namespace AslHelp.Core.Memory.SignatureScanning;

public class Signature
{
    public Signature(params string[] pattern)
        : this(0, pattern) { }

    public unsafe Signature(int offset, params string[] pattern)
    {
        ThrowHelper.ThrowIfNullOrEmpty(pattern);

        // We could potentially avoid this step and do concatenation, removal of whitespace,
        // and parsing all in the same step. That would avoid two allocations.
        string signature = pattern.Concat().RemoveWhiteSpace();

        if (signature.Length % 2 != 0)
        {
            const string msg = "Signature was not in the expected format. All bytes must be fully specified.";
            ThrowHelper.ThrowArgumentException(nameof(pattern), msg);
        }

        int bytes = signature.Length / 2;
        int length = (bytes + 7) & ~7;

        // It would probably be better to allocate the ulong arrays directly instead of creating
        // two byte spans and then converting them.
        Span<byte> values = stackalloc byte[length];
        Span<byte> masks = stackalloc byte[length];

        fixed (char* pSignature = signature)
        {
            for (int i = 0, j = 0; i < signature.Length; i += 2, j++)
            {
                char cUpper = signature[i + 0], cLower = signature[i + 1];

                byte upper = cUpper.ToHexValue();
                bool hasUpper = upper != 0xFF;

                byte lower = cLower.ToHexValue();
                bool hasLower = lower != 0xFF;

                (values[j], masks[j]) = (hasUpper, hasLower) switch
                {
                    (true, true) => ((byte)((upper << 4) + lower), (byte)0xFF),
                    (true, false) => ((byte)(upper << 4), (byte)0xF0),
                    (false, true) => (lower, (byte)0x0F),
                    (false, false) => ((byte)0x00, (byte)0x00)
                };

                if (!hasUpper || !hasLower)
                {
                    HasMasks = true;
                }
            }
        }

        Offset = offset;
        Length = bytes;

        Values = MemoryMarshal.Cast<byte, ulong>(values).ToArray();
        Masks = MemoryMarshal.Cast<byte, ulong>(masks).ToArray();
    }

    public Signature(params byte[] pattern)
        : this(0, pattern) { }

    public Signature(int offset, params byte[] pattern)
    {
        ThrowHelper.ThrowIfNullOrEmpty(pattern);

        Offset = offset;
        Length = pattern.Length;

        // There has to be a better way to do this.
        int length = (pattern.Length + 7) & ~7;
        Array.Resize(ref pattern, length);

        Values = MemoryMarshal.Cast<byte, ulong>(pattern).ToArray();
    }

    public int Offset { get; }
    public int Length { get; }

    public ulong[] Values { get; }
    public ulong[]? Masks { get; }

    [MemberNotNullWhen(true, nameof(Masks))]
    public bool HasMasks { get; }
}
