using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.IO.Parsing;

public class NativeStructInformation
{
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

    public static NativeStructInformation Parse(string engine, string major, string minor)
    {
        var assembly = Assembly.GetCallingAssembly();
        return Parse(engine, major, minor, assembly);
    }

    private static NativeStructInformation Parse(string engine, string major, string minor, Assembly assembly)
    {
        using Stream stream = EmbeddedResource.GetResourceStream($"AslHelp.{engine}.Structs.{major}-{minor}.json", assembly);

        Root? root = JsonSerializer.Deserialize<Root>(stream);
        if (root is null)
        {
            const string msg = "Unable to parse the specified JSON file.";
            ThrowHelper.ThrowFormatException(msg);
        }

        if (root.Inheritance is not null)
        {
            // return Parse(engine, root.Inheritance.Major, root.Inheritance.Minor, assembly);
        }
    }
}
