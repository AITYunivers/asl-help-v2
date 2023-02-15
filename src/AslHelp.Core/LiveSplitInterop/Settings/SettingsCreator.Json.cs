using System.Text.Json;

namespace AslHelp.Core.LiveSplitInterop.Settings;

public sealed partial class SettingsCreator
{
    private record struct JsonSetting(
        string Id,
        bool State,
        string Label,
        string Parent,
        string ToolTip,
        JsonSetting[] Children);

    public void CreateFromJson(string path, bool defaultValue = true, string defaultParent = null)
    {
        using FileStream fs = File.OpenRead(path);

        try
        {
            JsonSetting[] settings = JsonSerializer.Deserialize<JsonSetting[]>(fs);

            IEnumerable<Setting> converted = EnumerateJsonSettings(settings, defaultValue, defaultParent);
            Create(converted, defaultParent);
        }
        catch (JsonException)
        {
            fs.Position = default;
            dynamic[][] settings = JsonSerializer.Deserialize<dynamic[][]>(fs);

            IEnumerable<Setting> converted = EnumerateDynamicJsonSettings(settings, defaultValue, defaultParent);
            Create(converted, defaultParent);
        }
    }

    private IEnumerable<Setting> EnumerateJsonSettings(JsonSetting[] nodes, bool defaultValue, string defaultParent)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            JsonSetting node = nodes[i];
            yield return new(
                node.Id,
                node.State,
                node.Label,
                node.Parent ?? defaultParent,
                node.ToolTip);

            if (node.Children is { Length: > 0 } children)
            {
                foreach (Setting setting in EnumerateJsonSettings(children, defaultValue, node.Id))
                {
                    yield return setting;
                }
            }
        }
    }

    private IEnumerable<Setting> EnumerateDynamicJsonSettings(dynamic[][] nodes, bool defaultValue, string defaultParent)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            dynamic[] node = nodes[i];

            if (node[^1] is dynamic[][] { Length: > 0 } children)
            {
                yield return node.Length switch
                {
                    <= 1 => throw new FormatException("Too few arguments provided for Json setting."),
                    2 => new((string)node[0], defaultValue, (string)node[0], defaultParent, null),
                    3 => new((string)node[0], defaultValue, (string)node[1], defaultParent, null),
                    4 => new((string)node[0], (bool)node[1], (string)node[0], (string)node[2], null),
                    5 => new((string)node[0], (bool)node[1], (string)node[2], (string)node[3], null),
                    6 => new((string)node[0], (bool)node[1], (string)node[2], (string)node[3], (string)node[4]),
                    _ => throw new FormatException("Too many arguments provided for Json setting.")
                };

                foreach (Setting setting in EnumerateDynamicJsonSettings(children, defaultValue, (string)node[0]))
                {
                    yield return setting;
                }
            }
            else
            {
                yield return node.Length switch
                {
                    <= 0 => throw new FormatException("Too few arguments provided for Json setting."),
                    1 => new((string)node[0], defaultValue, (string)node[0], defaultParent, null),
                    2 => new((string)node[0], defaultValue, (string)node[1], defaultParent, null),
                    3 => new((string)node[0], (bool)node[1], (string)node[0], (string)node[2], null),
                    4 => new((string)node[0], (bool)node[1], (string)node[2], (string)node[3], null),
                    5 => new((string)node[0], (bool)node[1], (string)node[2], (string)node[3], (string)node[4]),
                    _ => throw new FormatException("Too many arguments provided for Json setting.")
                };
            }
        }
    }
}
