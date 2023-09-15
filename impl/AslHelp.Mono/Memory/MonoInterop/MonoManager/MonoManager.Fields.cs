using System.Collections.Generic;

namespace AslHelp.Mono.Memory.MonoInterop;

public partial class MonoManager
{
    protected abstract IEnumerable<nuint> EnumerateFields(nuint klass);
}
