namespace AslHelp.Core.Helping.Asl;

internal interface IAslInitStage
{
    IAslInitStage InitForAsl();
    IHelper GenerateCode();
}
