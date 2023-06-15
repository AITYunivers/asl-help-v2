namespace AslHelp.Core.Memory.Pointers;

public interface IPointer
{

}

public interface IPointer<T> : IPointer
    where T : unmanaged
{

}
