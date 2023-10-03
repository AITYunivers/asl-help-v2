using System;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop;

using OneOf;

namespace AslHelp.Mono.Memory.Pointers.Initialization;

public class MonoPointerFactory : PointerFactory, IMonoPointerFactory
{
    private readonly IMonoMemoryManager _memory;
    private readonly IMonoManager _mono;

    public MonoPointerFactory(IMonoMemoryManager memory, IMonoManager mono)
        : base(memory)
    {
        _memory = memory;
        _mono = mono;

        _ = Make<int>(className: "", "", next: "", "");
    }

    public Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        nuint image = _mono.GetImage("Assembly-CSharp");
        return Make<T>(image, className, staticFieldName, next);
    }

    public Pointer<T> Make<T>(nuint image, string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        throw new NotImplementedException();
    }
}
