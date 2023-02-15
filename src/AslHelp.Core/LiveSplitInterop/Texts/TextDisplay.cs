using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop.Texts;

public sealed class TextDisplay
{
    private const string TextComponentName = "LiveSplit.Text.dll";

    private readonly TextComponentSettings _settings;

    internal TextDisplay()
        : this(ComponentManager.LoadLayoutComponent(TextComponentName, Timer.State)) { }

    internal TextDisplay(ILayoutComponent component)
    {
        LayoutComponent = component;
        _settings = ((TextComponent)component.Component).Settings;
        Timer.Layout.LayoutComponents.Add(component);
    }

    internal ILayoutComponent LayoutComponent { get; }

    internal void SetTag(string tag)
    {
        _settings.Tag = tag;
    }

    public dynamic Left
    {
        get => _settings.Text1;
        set => _settings.Text1 = value.ToString();
    }

    public dynamic Right
    {
        get => _settings.Text2;
        set => _settings.Text2 = value.ToString();
    }
}
