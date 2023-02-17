namespace AslHelp.Core.LiveSplitInterop.Settings;

public readonly record struct Setting(
    string Id,
    bool State,
    string Label,
    string Parent,
    string ToolTip)
{
    public Setting(string id, bool state, string label, string parent)
        : this(id, state, label, parent, null) { }
}
