namespace AslHelp.Core.LiveSplitInterop.Settings;

public readonly record struct Setting(
    string? Id,
    bool State,
    string? Label,
    string? Parent,
    string? ToolTip = null);
