namespace AslHelp.Core.Helping.Asl.Contracts;

public partial interface IAslHelper
{
#pragma warning disable IDE1006
    public interface InitStage
#pragma warning restore IDE1006
    {
        InitStage Init { get; }

        InitStage GameName(string gameName);

        InitStage GenerateCode();

        InitStage WithInjection();
        InitStage WithInjection(int pipeConnectionTimeout);

        IAslHelper Complete();
    }
}
