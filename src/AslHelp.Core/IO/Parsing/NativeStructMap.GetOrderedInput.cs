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

public sealed partial class NativeStructMap
{
    private class ParsedJsonInput : OrderedDictionary<string, Struct>
    {
        public Dictionary<string, Memory.SignatureScanning.Signature> Signatures { get; } = new();

        protected override string GetKeyForItem(Struct item)
        {
            return item.Name;
        }
    }

    private static ParsedJsonInput GetOrderedInput(Assembly asm, string engine, string major, string minor)
    {
        using Stream resource = EmbeddedResource.GetResourceStream($"AslHelp.{engine}.Structs.{major}-{minor}.json", asm);

        Root? root = JsonSerializer.Deserialize<Root>(resource, new JsonSerializerOptions
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

        ParsedJsonInput pji =
            root.Inheritance is { Major.Length: > 0, Minor.Length: > 0 } inherit
            ? GetOrderedInput(asm, engine, inherit.Major, inherit.Minor)
            : new();

        if (root.Structs is { Length: > 0 } structs)
        {
            foreach (Struct @struct in structs)
            {
                pji[@struct.Name] = @struct;
            }
        }

        if (root.Signatures is { Length: > 0 } signatures)
        {
            foreach (Signature signature in signatures)
            {
                pji.Signatures[signature.Name] = new(signature.Offset, signature.Pattern);
            }
        }

        return pji;
    }

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

    private record Struct(
        string Name,
        string? Super,
        Field[] Fields);

    private record Field(
        string Type,
        string Name,
        int? Alignment);
}
