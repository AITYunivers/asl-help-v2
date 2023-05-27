using System.Collections.Generic;
using AslHelp.Common.Exceptions;
using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop.Texts;

public sealed class TextComponentController
{
    private readonly Dictionary<string, TextDisplay> _textDisplays = new();

    public TextDisplay this[string id]
    {
        get
        {
            if (_textDisplays.TryGetValue(id, out TextDisplay td))
            {
                return td;
            }

            IList<ILayoutComponent> components = Timer.Layout.LayoutComponents;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                ILayoutComponent component = components[i];
                if (component.Component is not TextComponent tc)
                {
                    continue;
                }

                if (tc.Settings.Tag is string tag && tag == id)
                {
                    td = new(component);
                    _textDisplays[id] = td;

                    tc.Settings.Disposed += (_, _) => _textDisplays.Remove(id);

                    return td;
                }
            }

            td = new();
            _textDisplays[id] = td;

            return td;
        }
        set
        {
            if (value is null)
            {
                Remove(id);
                return;
            }

            value.SetTag(id);
            _textDisplays[id] = value;
        }
    }

    public void Remove(string id)
    {
        if (!_textDisplays.TryGetValue(id, out TextDisplay td))
        {
            return;
        }

        _ = Timer.Layout.LayoutComponents.Remove(td.LayoutComponent);
        _ = _textDisplays.Remove(id);
    }

    public void RemoveAll()
    {
        foreach (TextDisplay td in _textDisplays.Values)
        {
            _ = Timer.Layout.LayoutComponents.Remove(td.LayoutComponent);
        }

        _textDisplays.Clear();
    }

    public TextDisplay Find(string text1 = "", string text2 = "")
    {
        bool empty1 = string.IsNullOrEmpty(text1), empty2 = string.IsNullOrEmpty(text2);

        if (empty1 && empty2)
        {
            string msg = "At least one text must be provided.";
            ThrowHelper.ThrowArgumentException("text1_text2", msg);
        }

        IList<ILayoutComponent> components = Timer.Layout.LayoutComponents;
        for (int i = components.Count - 1; i >= 0; i--)
        {
            ILayoutComponent component = components[i];
            if (component.Component is not TextComponent tc)
            {
                continue;
            }

            TextComponentSettings settings = tc.Settings;

            if (!empty1 && settings.Text1 != text1)
            {
                continue;
            }

            if (!empty2 && settings.Text2 != text2)
            {
                continue;
            }

            return new(component);
        }

        return null;
    }
}
