using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
#pragma warning disable IDE1006
    public interface Initialization
#pragma warning restore IDE1006
    {
        SettingsCreator Settings { get; }

        IAslHelper Init();

        IAslHelper.Initialization SetGameName(string gameName);
        IAslHelper.Initialization DoCodeGeneration(bool generateCode = true);
        IAslHelper.Initialization DoInjection(int pipeConnectionTimeout = 3000);

        IAslHelper.Initialization LogToFile(string fileName, int maxLines = 4096, int linesToErase = 512);

        IAslHelper.Initialization AlertRealTime(string? message = null);
        IAslHelper.Initialization AlertGameTime(string? message = null);
        IAslHelper.Initialization AlertLoadless(string? message = null);

        ITypeDefinition Define(string code, params string[] references);
    }
}
