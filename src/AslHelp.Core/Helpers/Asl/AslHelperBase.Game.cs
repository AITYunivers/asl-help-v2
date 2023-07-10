using System.Diagnostics;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    private string? _gameName;
    public string GameName => _gameName ?? Game?.ProcessName ?? "Auto Splitter";

    public abstract Process? Game { get; set; }
}
