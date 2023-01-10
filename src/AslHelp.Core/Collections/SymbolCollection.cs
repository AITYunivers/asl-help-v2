using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Collections;

public sealed class SymbolCollection
    : IReadOnlyDictionary<string, DebugSymbol>
{
    private readonly Module _module;
    private readonly Process _process;

    public SymbolCollection(Module module, Process process)
    {
        _module = module;
        _process = process;
    }

    private Dictionary<string, DebugSymbol> _cache;
    private Dictionary<string, DebugSymbol> Cache => _cache ??= _module.Symbols(_process);

    public DebugSymbol this[string key] => Cache[key.ToLowerInvariant()];

    public int Count => Cache.Count;

    public IEnumerable<string> Keys => Cache.Keys;
    public IEnumerable<DebugSymbol> Values => Cache.Values;

    public bool ContainsKey(string key)
    {
        return Cache.ContainsKey(key.ToLowerInvariant());
    }

    public bool TryGetValue(string key, out DebugSymbol value)
    {
        return Cache.TryGetValue(key.ToLowerInvariant(), out value);
    }

    public IEnumerator<KeyValuePair<string, DebugSymbol>> GetEnumerator()
    {
        return Cache.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Cache.GetEnumerator();
    }
}
