using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Pointers;

public interface IPointer
{
    object? Current { get; set; }
    object? Old { get; set; }
    bool Changed { get; }

    bool Enabled { get; set; }
    bool LogChange { get; set; }
    bool UpdateOnFail { get; set; }

    IPointer SetName(string name);
    IPointer SetLogChange(bool logChange = true);
    IPointer SetUpdateOnFail(bool updateOnFail = true);

    nuint Deref();
    bool Write([NotNullWhen(true)] object? value);
    void Reset();
}

public interface IPointer<T> : IPointer
{
    new T? Current { get; set; }
    new T? Old { get; set; }

    bool Write([NotNullWhen(true)] T? value);
}
