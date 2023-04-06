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

public interface IPointer<TOut> : IPointer
{
    new TOut Current { get; set; }
    new TOut Old { get; set; }
}
