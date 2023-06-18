using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop.Control;

public sealed class TextComponentWrapper
{
    private const string TextComponentName = "LiveSplit.Text.dll";

    private readonly TextComponentSettings _settings;

    internal TextComponentWrapper()
        : this(ComponentManager.LoadLayoutComponent(TextComponentName, Timer.State)) { }

    internal TextComponentWrapper(ILayoutComponent component)
    {
        LayoutComponent = component;
        _settings = ((TextComponent)component.Component).Settings;
        Timer.Layout.LayoutComponents.Add(component);
    }

    internal ILayoutComponent LayoutComponent { get; }

    internal object? Tag
    {
        get => _settings.Tag;
        set => _settings.Tag = value;
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
