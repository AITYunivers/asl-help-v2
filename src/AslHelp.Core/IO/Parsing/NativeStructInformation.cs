using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Collections;

namespace AslHelp.Core.IO.Parsing;

public class StructMap : OrderedDictionary<string, NativeStructInformation.Struct>
{
    protected override string GetKeyForItem(NativeStructInformation.Struct item)
    {
        return item.Name;
    }
}

public class NativeStructInformation
{
    public StructMap Structs { get; } = new();
    public Dictionary<string, Memory.SignatureScanning.Signature> Signatures { get; } = new();

    private record Root(
        [property: JsonPropertyName("inherits")] Inheritance? Inheritance,
        Signature[]? Signatures,
        Struct[]? Structs);

    private record Inheritance(
        string Major,
        string Minor);

    private record Signature(
        string Name,
        int Offset,
        string Pattern);

    public record Struct(
        string Name,
        string? Super,
        Field[] Fields);

    public record Field(
        string Type,
        string Name,
        int? Alignment);

    public static NativeStructInformation Parse(string engine, int major, int minor)
    {
        return Parse(engine, major.ToString(), minor.ToString());
    }

    public static NativeStructInformation Parse(string engine, string major, string minor)
    {
        Assembly asm = Assembly.GetCallingAssembly();
        return Parse(engine, major, minor, asm);
    }

    private static NativeStructInformation Parse(string engine, string major, string minor, Assembly asm)
    {
        using Stream stream = EmbeddedResource.GetResourceStream($"AslHelp.{engine}.Structs.{major}-{minor}.json", asm);

        Root? root = JsonSerializer.Deserialize<Root>(stream);
        if (root is null)
        {
            const string msg = "JSON data was a 'null' literal.";
            ThrowHelper.ThrowFormatException(msg);
        }

        NativeStructInformation nsi = new();
        if (root.Inheritance is not null)
        {
            nsi = Parse(engine, root.Inheritance.Major, root.Inheritance.Minor, asm);
        }

        if (root.Structs is { Length: > 0 } structs)
        {
            foreach (Struct s in structs)
            {
                nsi.Structs[s.Name] = s;
            }
        }

        if (root.Signatures is { Length: > 0 } signatures)
        {
            foreach (Signature s in signatures)
            {
                nsi.Signatures[s.Name] = new(s.Offset, s.Pattern);
            }
        }

        return nsi;
    }
}
