using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Common.Results;
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

    private static Result<ParsedJsonInput, ParseError> GetOrderedInput(
        Assembly assembly,
        string engine,
        string major,
        string minor)
    {
        string resourceName = $"AslHelp.{engine}.Structs.{major}-{minor}.json";
        using Stream? resource = assembly.GetManifestResourceStream(resourceName);
        if (resource is null)
        {
            return new(
                IsSuccess: false,
                Error: new(ParseError.EmbeddedResourceNotFound, $"The specified embedded resource '{resourceName}' could not be found."));
        }

        Root? root = JsonSerializer.Deserialize<Root>(resource, new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true
        });

        if (root is null)
        {
            return new(
                IsSuccess: false,
                Error: new(ParseError.InvalidJson, "Root object is null."));
        }

        if (root.Inheritance is null && root.Structs is null)
        {
            return new(
                IsSuccess: false,
                Error: new(ParseError.InvalidJson, "One of 'inherit' or 'structs' must be provided."));
        }

        ParsedJsonInput pji;
        if (root.Inheritance is { Major.Length: > 0, Minor.Length: > 0 } inherit)
        {
            var inheritResult = GetOrderedInput(assembly, engine, inherit.Major, inherit.Minor);
            if (!inheritResult.IsSuccess)
            {
                return inheritResult;
            }

            pji = inheritResult.Value;
        }
        else
        {
            pji = new();
        }

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

        return new(
            IsSuccess: true,
            Value: pji);
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
        uint? Alignment);
}
