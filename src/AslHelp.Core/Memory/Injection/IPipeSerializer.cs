namespace AslHelp.Core.Memory.Injection;

public interface IPipeSerializer<TRequest, TResponse>
{
    void Write(TRequest request, BinaryWriter writer);
    TResponse Read(BinaryReader reader);
}
