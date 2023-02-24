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

    public SettingsCreator FromJson(string path, bool defaultValue = true, string defaultParent = null)
    {
        using FileStream fs = File.OpenRead(path);

        try
        {
            JsonSetting[] settings = JsonSerializer.Deserialize<JsonSetting[]>(fs);

            IEnumerable<Setting> converted = EnumerateJsonSettings(settings, defaultValue, defaultParent);
            return Create(converted, defaultParent);
        }
        catch (JsonException)
        {
            fs.Position = default;
            JsonElement[][] settings = JsonSerializer.Deserialize<JsonElement[][]>(fs);

            IEnumerable<Setting> converted = EnumerateDynamicJsonSettings(settings, defaultValue, defaultParent);
            return Create(converted, defaultParent);
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

    private IEnumerable<Setting> EnumerateDynamicJsonSettings(JsonElement[][] nodes, bool defaultValue, string defaultParent)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            JsonElement[] node = nodes[i];

            if (node[^1].ValueKind == JsonValueKind.Array
                && node[^1].Deserialize<JsonElement[][]>() is { Length: > 0 } children)
            {
                yield return node.Length switch
                {
                    <= 1 => throw new FormatException("Too few arguments provided for Json setting."),
                    2 => new(node[0].GetString(), defaultValue, node[0].GetString(), defaultParent, null),
                    3 => new(node[0].GetString(), defaultValue, node[1].GetString(), defaultParent, null),
                    4 => new(node[0].GetString(), node[1].GetBoolean(), node[2].GetString(), defaultParent, null),
                    5 => new(node[0].GetString(), node[1].GetBoolean(), node[2].GetString(), defaultParent, node[3].GetString()),
                    _ => throw new FormatException("Too many arguments provided for Json setting.")
                };

                foreach (Setting setting in EnumerateDynamicJsonSettings(children, defaultValue, node[0].GetString()))
                {
                    yield return setting;
                }
            }
            else
            {
                yield return node.Length switch
                {
                    <= 0 => throw new FormatException("Too few arguments provided for Json setting."),
                    1 => new(node[0].GetString(), defaultValue, node[0].GetString(), defaultParent, null),
                    2 => new(node[0].GetString(), defaultValue, node[1].GetString(), defaultParent, null),
                    3 => new(node[0].GetString(), node[1].GetBoolean(), node[2].GetString(), defaultParent, null),
                    4 => new(node[0].GetString(), node[1].GetBoolean(), node[2].GetString(), defaultParent, node[3].GetString()),
                    _ => throw new FormatException("Too many arguments provided for Json setting.")
                };
            }
        }
    }
}
