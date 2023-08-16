using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.IO.Parsing;

public sealed class NativeStructMap : IReadOnlyDictionary<string, NativeStruct>
{
    internal readonly Dictionary<string, NativeStruct> Structs = new();

    public IEnumerator<KeyValuePair<string, NativeStruct>> GetEnumerator()
    {
        return Structs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => Structs.Count;

    public bool ContainsKey(string key)
    {
        return Structs.ContainsKey(key);
    }

    public bool TryGetValue(string key, [UnscopedRef] out NativeStruct value)
    {
        return Structs.TryGetValue(key, out value);
    }

    public NativeStruct this[string key] => Structs[key];

    public IEnumerable<string> Keys => Structs.Keys;
    public IEnumerable<NativeStruct> Values => Structs.Values;
}

public sealed class NativeStruct : IReadOnlyDictionary<string, NativeField>
{
    internal readonly Dictionary<string, NativeField> Fields = new();
    public IEnumerator<KeyValuePair<string, NativeField>> GetEnumerator()
    {
        return Fields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => Fields.Count;

    public bool ContainsKey(string key)
    {
        return Fields.ContainsKey(key);
    }

    public bool TryGetValue(string key, [UnscopedRef] out NativeField value)
    {
        return Fields.TryGetValue(key, out value);
    }

    public NativeField this[string key] => Fields[key];

    public IEnumerable<string> Keys => Fields.Keys;

    public IEnumerable<NativeField> Values => Fields.Values;
}

public sealed class NativeField
{

}
