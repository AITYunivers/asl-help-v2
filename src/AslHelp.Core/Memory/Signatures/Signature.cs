using AslHelp.Core.Exceptions;
using AslHelp.Core.Extensions;

namespace AslHelp.Core.Memory.Signatures;

public readonly ref struct Signature
{
    public Signature(params string[] pattern)
        : this(0, pattern) { }

    public unsafe Signature(int offset, params string[] pattern)
    {
        ThrowHelper.ThrowIfNullOrEmpty(pattern);

        string signature = pattern.Concat().RemoveWhiteSpace();

        if (signature.Length % 2 != 0)
        {
            ThrowHelper.ThrowAE(nameof(signature), "Signature was not in the expected format. All bytes must be fully specified.");
        }

        int bytes = signature.Length / 2;
        int length = (bytes + 7) & ~7;

        Span<byte> values = new byte[length];
        Span<byte> masks = new byte[length];

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
                    Pure = false;
                }
            }
        }

        Offset = offset;
        Length = bytes;

        Values = MemoryMarshal.Cast<byte, ulong>(values);
        Masks = MemoryMarshal.Cast<byte, ulong>(masks);
    }

    public Signature(params byte[] pattern)
        : this(0, pattern) { }

    public Signature(int offset, params byte[] pattern)
    {
        ThrowHelper.ThrowIfNullOrEmpty(pattern);

        Offset = offset;
        Length = pattern.Length;
        Pure = true;

        int length = (pattern.Length + 7) & ~7;
        Array.Resize(ref pattern, length);

        Values = MemoryMarshal.Cast<byte, ulong>(pattern);
    }

    public int Offset { get; }
    public int Length { get; }

    public ReadOnlySpan<ulong> Values { get; }
    public ReadOnlySpan<ulong> Masks { get; }

    public bool Pure { get; }
}
