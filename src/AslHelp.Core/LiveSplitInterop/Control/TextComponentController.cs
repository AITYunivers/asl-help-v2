using System.Collections.Generic;

using AslHelp.Common.Exceptions;

using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop.Control;

public sealed class TextComponentController
{
    private readonly Dictionary<string, TextComponentWrapper> _textDisplays = new();

    public TextComponentWrapper this[string id]
    {
        get
        {
            if (_textDisplays.TryGetValue(id, out TextComponentWrapper td))
            {
                return td;
            }

            IList<ILayoutComponent> components = Timer.Layout.LayoutComponents;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i] is not { Component: TextComponent tc } component)
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

            value.Tag = id;
            _textDisplays[id] = value;
        }
    }

    public void Remove(string id)
    {
        if (!_textDisplays.TryGetValue(id, out TextComponentWrapper td))
        {
            return;
        }

        _ = Timer.Layout.LayoutComponents.Remove(td.LayoutComponent);
        _ = _textDisplays.Remove(id);
    }

    public void RemoveAll()
    {
        foreach (TextComponentWrapper td in _textDisplays.Values)
        {
            _ = Timer.Layout.LayoutComponents.Remove(td.LayoutComponent);
        }

        _textDisplays.Clear();
    }

    public TextComponentWrapper? Find(string text1 = "", string text2 = "")
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
            if (components[i] is not { Component: TextComponent tc } component)
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
