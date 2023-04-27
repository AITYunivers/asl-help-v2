using AslHelp.Core.Memory;

namespace AslHelp.Core.Collections;

public sealed class ModuleCache : CachedEnumerable<string, Module>
{
    private readonly int _processId;
    private readonly nint _processHandle;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModuleCache"/> class
    ///     with the specified <see cref="Process"/>.
    /// </summary>
    /// <param name="process">The target <see cref="Process"/> whose modules are to be enumerated.</param>
    public ModuleCache(Process process)
        : this(process.Id, process.Handle) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModuleCache"/> class
    ///     with the specified process ID and handle.
    /// </summary>
    /// <param name="processId">
    ///     The <see cref="Process.Id"/> of the target <see cref="Process"/>
    ///     whose modules are to be enumerated.
    /// </param>
    /// <param name="processHandle">
    ///     The <see cref="Process.Handle"/> of the target <see cref="Process"/>
    ///     whose modules are to be enumerated.
    /// </param>
    public ModuleCache(int processId, nint processHandle)
        : base(StringComparer.OrdinalIgnoreCase)
    {
        _processId = processId;
        _processHandle = processHandle;
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="ModuleCache"/>.
    /// </summary>
    public override IEnumerator<Module> GetEnumerator()
    {
        foreach (Module module in Native.ModulesTh32(_processHandle, _processId))
        {
            yield return module;
        }
    }

    protected override string GetKey(Module value)
    {
        return value.Name;
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"The given module '{key}' was not present in the process.";
    }
}
