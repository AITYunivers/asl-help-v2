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
    bool ReadBigEndian { get; set; }

    IPointer SetName(string name);
    IPointer SetLogChange();
    IPointer SetUpdateOnFail();

    void Reset();
}

public interface IPointer<T> : IPointer
    where T : unmanaged
{

}
