using AslHelp.Core.Memory;

namespace AslHelp.Core.Collections;

public class ModuleCache : CachedEnumerable<string, Module>
{
    private readonly Process _process;

    public ModuleCache(Process process)
    {
        _process = process;
    }

    public override IEnumerator<Module> GetEnumerator()
    {
        foreach (Module module in _process.Modules())
        {
            yield return module;
        }
    }

    protected override string GetKey(Module value)
    {
        return value.Name;
    }

    protected override bool CompareKeys(string searchedKey, string itemKey)
    {
        return searchedKey.Equals(itemKey, StringComparison.OrdinalIgnoreCase);
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"The given module '{key}' was not present in the process.";
    }
}
