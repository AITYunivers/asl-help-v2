namespace AslHelp.Core.LiveSplitInterop.Settings;

public readonly struct Setting
{
    public Setting(string id, bool state, string label, string parent)
        : this(id, state, label, parent, null) { }

    public Setting(string id, bool state, string label, string parent, string toolTip)
    {
        Id = id;
        State = state;
        Label = label;
        Parent = parent;
        ToolTip = toolTip;
    }

    public string Id { get; }
    public bool State { get; }
    public string Label { get; }
    public string Parent { get; }
    public string ToolTip { get; }

    public void Deconstruct(out string id, out bool state, out string label, out string parent, out string toolTip)
    {
        id = Id;
        state = State;
        label = Label;
        parent = Parent;
        toolTip = ToolTip;
    }
}
