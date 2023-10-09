using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using AslHelp.Common.Results;
using AslHelp.Core.Collections;

namespace AslHelp.Core.IO.Parsing;

public sealed partial class NativeStructMap : OrderedDictionary<string, NativeStruct>
{
    private NativeStructMap() { }

    public Dictionary<string, Memory.SignatureScanning.Signature> Signatures { get; } = new();

    protected override string GetKeyForItem(NativeStruct item)
    {
        return item.Name;
    }

    public static Result<NativeStructMap, ParseError> InitializeFromResource(
        string engine,
        int major,
        int minor,
        bool is64Bit)
    {
        return InitializeFromResource(engine, major.ToString(), minor.ToString(), is64Bit);
    }

    public static Result<NativeStructMap, ParseError> InitializeFromResource(
        string engine,
        string major,
        string minor,
        bool is64Bit)
    {
        var inputResult = GetOrderedInput(Assembly.GetCallingAssembly(), engine, major, minor);
        if (!inputResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: inputResult.Error);
        }

        ParsedJsonInput input = inputResult.Value;
        NativeStructMap structs = new();

        foreach (Struct s in input)
        {
            NativeStruct ns = new()
            {
                Name = s.Name,
                Super = s.Super
            };

            NativeFieldParser parser =
                s.Super is not null && structs.TryGetValue(s.Super, out var super)
                ? new(structs, super, is64Bit)
                : new(structs, is64Bit);

            foreach (Field f in s.Fields)
            {
                // TODO: Refactor `NativeFieldParser` to return a `Result` instead of throwing exceptions.
                try
                {
                    FieldParseResult result = parser.ParseNext(f.Type, f.Alignment);
                    ns[f.Name] = new(result.BitField)
                    {
                        Name = f.Name,
                        Type = result.TypeName,
                        Offset = result.Offset,
                        Size = result.Size,
                        Alignment = result.Alignment
                    };
                }
                catch (Exception ex)
                {
                    return new(
                        IsSuccess: false,
                        Error: new(ParseError.FieldParseException, $"'{s.Name}.{f.Name}' parse error: {ex.Message}"));
                }
            }

            structs[s.Name] = ns;
        }

        foreach (var signature in input.Signatures)
        {
            structs.Signatures[signature.Key] = signature.Value;
        }

        return new(
            IsSuccess: true,
            Value: structs);
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
