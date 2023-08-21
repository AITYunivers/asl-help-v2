using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Collections;

namespace AslHelp.Core.IO.Parsing;

internal sealed class NativeStructMap : OrderedDictionary<string, NativeStruct>
{
    public Dictionary<string, Memory.SignatureScanning.Signature> Signatures { get; } = new();

    protected override string GetKeyForItem(NativeStruct item)
    {
        return item.Name;
    }

    private record Root(
        Inheritance? Inheritance,
        Signature[]? Signatures,
        Struct[]? Structs);

    private record Inheritance(
        string Major,
        string Minor);

    private record Signature(
        string Name,
        int Offset,
        string Pattern);

    private record Struct(
        string Name,
        string? Super,
        Field[] Fields);

    private record Field(
        string Type,
        string Name,
        int? Alignment);

    public static NativeStructMap Parse(string engine, string major, string minor, bool is64Bit, Assembly asm)
    {
        using Stream source = EmbeddedResource.GetResourceStream($"AslHelp.{engine}.Memory.Native.{major}-{minor}.jsonc", asm);

        Root? root = JsonSerializer.Deserialize<Root>(source, new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true
        });
        if (root is null)
        {
            const string msg = "Provided resource was a JSON 'null' literal.";
            ThrowHelper.ThrowFormatException(msg);
        }

        if (root.Inheritance is null && root.Structs is null)
        {
            const string msg = "One of 'inherit' or 'structs' must be provided.";
            ThrowHelper.ThrowFormatException(msg);
        }

        NativeStructMap nsm =
            root.Inheritance is { Major.Length: > 0, Minor.Length: > 0 } inherit
            ? Parse(engine, inherit.Major, inherit.Minor, is64Bit, asm)
            : new();

        if (root.Structs is not { Length: > 0 } structs)
        {
            return nsm;
        }

        foreach (Struct s in structs)
        {
            NativeStruct ns = new()
            {
                Name = s.Name,
                Super = s.Super
            };

            NativeFieldParser parser =
                s.Super is not null && nsm.TryGetValue(s.Super, out var super)
                ? new(nsm, super, is64Bit)
                : new(nsm, is64Bit);

            foreach (Field f in s.Fields)
            {
                var result = parser.ParseNext(f.Type, f.Alignment);
                ns[f.Name] = new(result.BitField)
                {
                    Name = f.Name,
                    Type = result.TypeName,
                    Offset = result.Offset,
                    Size = result.Size,
                    Alignment = result.Alignment
                };
            }

            nsm[s.Name] = ns;
        }

        if (root.Signatures is Signature[] signatures)
        {
            foreach (Signature sig in signatures)
            {
                nsm.Signatures[sig.Name] = new(sig.Offset, sig.Pattern);
            }
        }

        return nsm;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (NativeStruct ns in this)
        {
            if (ns.Super is string super)
            {
                sb.AppendLine($"{ns.Name} : {super} // 0x{ns.Size:X3} (0x{ns.SelfAlignedSize:X3})");
            }
            else
            {
                sb.AppendLine($"{ns.Name} // 0x{ns.Size:X3} (0x{ns.SelfAlignedSize:X3})");
            }

            foreach (NativeField nf in ns.Fields)
            {
                if (nf.BitMask > 0)
                {
                    sb.AppendLine($"    {nf.Type,-32} {nf.Name,-32} " +
                                  $"// 0x{nf.Offset:X3} " +
                                  $"(0x{nf.Size:X3}, 0b{Convert.ToString(nf.BitMask, 2).PadLeft(8, '0')})");
                }
                else
                {
                    sb.AppendLine($"    {nf.Type,-32} {nf.Name,-32} // 0x{nf.Offset:X3} (0x{nf.Size:X3})");
                }
            }
        }

        return sb.ToString();
    }
}
