using System;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop;

using OneOf;

namespace AslHelp.Unity.Memory.Pointers.Initialization;

public class MonoPointerFactory : PointerFactory, IMonoPointerFactory
{
    private readonly IMonoInteroperator _mono;

    public MonoPointerFactory(IMonoMemoryManager memory, IMonoInteroperator mono)
        : base(memory)
    {
        _mono = mono;
    }

    public Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        return new(_memory, 0);
    }
}
