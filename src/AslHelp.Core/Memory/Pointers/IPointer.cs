namespace AslHelp.Core.Memory.Pointers;

public interface IPointer
{
    object Current { get; set; }
    object Old { get; set; }
    bool Changed { get; }

    string Name { get; set; }
    bool Enabled { get; set; }
    bool LogChange { get; set; }
    bool UpdateOnFail { get; set; }

    void Reset();
}

public interface IPointer<T>
    : IPointer
{
    new T Current { get; set; }
    new T Old { get; set; }
}
