using AslHelp.Core.Exceptions;

namespace AslHelp.Core.LiveSplitInterop.Settings;

public sealed partial class SettingsCreator
{
    public void Create(Dictionary<string, string> settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(kvp => new Setting(kvp.Key, defaultValue, kvp.Value, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(Dictionary<string, bool> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(kvp => new Setting(kvp.Key, kvp.Value, kvp.Key, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params string[] settings)
    {
        Create(settings, true, null);
    }

    public void Create(IEnumerable<string> settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(s => new Setting(s, defaultValue, s, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params (string, string)[] settings)
    {
        Create(settings, true, null);
    }

    public void Create(IEnumerable<(string, string)> settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, defaultValue, t.Item2, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params Tuple<string, string>[] settings)
    {
        Create(settings, true, null);
    }

    public void Create(IEnumerable<Tuple<string, string>> settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, defaultValue, t.Item2, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params (string, bool, string)[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<(string, bool, string)> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params Tuple<string, bool, string>[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<Tuple<string, bool, string>> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, defaultParent));
        Create(converted, defaultParent);
    }

    public void Create(params (string, bool, string, string)[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<(string, bool, string, string)> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, t.Item4));
        Create(converted, defaultParent);
    }

    public void Create(params Tuple<string, bool, string, string>[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<Tuple<string, bool, string, string>> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, t.Item4));
        Create(converted, defaultParent);
    }

    public void Create(params (string, bool, string, string, string)[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<(string, bool, string, string, string)> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5));
        Create(converted, defaultParent);
    }

    public void Create(params Tuple<string, bool, string, string, string>[] settings)
    {
        Create(settings, null);
    }

    public void Create(IEnumerable<Tuple<string, bool, string, string, string>> settings, string defaultParent = null)
    {
        IEnumerable<Setting> converted = settings?.Select(t => new Setting(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5));
        Create(converted, defaultParent);
    }

    public void Create(IEnumerable<Setting> settings, string defaultParent = null)
    {
        ThrowHelper.ThrowIfNull(Script.SettingsBuilder, paramName: "ASLScript.SettingsBuilder");
        ThrowHelper.ThrowIfNull(settings);

        foreach (Setting setting in settings)
        {
            ThrowHelper.ThrowIfNull(setting.Id, paramName: "ASLSetting.Id");
            ThrowHelper.ThrowIfNull(setting.Label, paramName: "ASLSetting.Label");

            Script.SettingsBuilder.Add(setting.Id, setting.State, setting.Label, setting.Parent ?? defaultParent);
            if (!string.IsNullOrWhiteSpace(setting.ToolTip))
            {
                Script.SettingsBuilder.SetToolTip(setting.Id, setting.ToolTip);
            }
        }
    }

    public void Create(dynamic[,] settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = EnumerateDynamic(settings, defaultValue, defaultParent);
        Create(converted, defaultParent);
    }

    private IEnumerable<Setting> EnumerateDynamic(dynamic[,] settings, bool defaultValue, string defaultParent)
    {
        (int outerCount, int innerCount) = (settings.GetLength(0), settings.GetLength(1));

        for (int i = 0; i < outerCount; i++)
        {
            yield return innerCount switch
            {
                0 => throw new ArgumentException(
                    $"Too few arguments provided in settings array.",
                    $"settings[{i}]"),
                1 => new(settings[i, 0], defaultValue, settings[i, 0], defaultParent, null),
                2 => new(settings[i, 0], defaultValue, settings[i, 1], defaultParent, null),
                3 => new(settings[i, 0], settings[i, 1], settings[i, 0], settings[i, 2], null),
                4 => new(settings[i, 0], settings[i, 1], settings[i, 2], settings[i, 3], null),
                5 => new(settings[i, 0], settings[i, 1], settings[i, 2], settings[i, 3], settings[i, 4]),
                _ => throw new ArgumentException(
                    $"Too few arguments provided in settings ({innerCount}).",
                    $"settings[{i}]")
            };
        }
    }

    public void Create(dynamic[][] settings, bool defaultValue = true, string defaultParent = null)
    {
        IEnumerable<Setting> converted = EnumerateDynamic(settings, defaultValue, defaultParent);
        Create(converted, defaultParent);
    }

    private IEnumerable<Setting> EnumerateDynamic(dynamic[][] settings, bool defaultValue, string defaultParent)
    {
        int count = settings.Length;

        for (int i = 0; i < count; i++)
        {
            dynamic[] setting = settings[i];
            ThrowHelper.ThrowIfNull(setting, paramName: $"settings[{i}]");

            yield return setting.Length switch
            {
                0 => throw new ArgumentException(
                    $"Too few arguments provided in settings array.",
                    $"settings[{i}]"),
                1 => new(setting[0], defaultValue, setting[0], defaultParent, null),
                2 => new(setting[0], defaultValue, setting[1], defaultParent, null),
                3 => new(setting[0], setting[1], setting[0], setting[2], null),
                4 => new(setting[0], setting[1], setting[2], setting[3], null),
                5 => new(setting[0], setting[1], setting[2], setting[3], setting[4]),
                _ => throw new ArgumentException(
                    $"Too many arguments provided in settings array ({setting.Length}).",
                    $"settings[{i}]")
            };
        }
    }

    public void CreateCustom(dynamic[,] settings, params int[] positions)
    {
        CreateCustom(settings, true, null, positions);
    }

    public void CreateCustom(dynamic[,] settings, string defaultParent, params int[] positions)
    {
        CreateCustom(settings, true, defaultParent, positions);
    }

    public void CreateCustom(dynamic[,] settings, bool defaultValue, string defaultParent, params int[] positions)
    {
        IEnumerable<Setting> converted = EnumerateCustom(settings, defaultValue, positions);
        Create(converted, defaultParent);
    }

    private IEnumerable<Setting> EnumerateCustom(dynamic[,] settings, bool defaultValue, params int[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int pos = positions[i];
            if (pos is not (>= 0 and <= 4))
            {
                string msg = $"All positions must be in range 0 - 4 (position {i} was {pos}).";
                ThrowHelper.Throw.ArgumentOutOfRange(nameof(positions), msg);
            }
        }

        (int outerCount, int innerCount) = (settings.GetLength(0), settings.GetLength(1));
        if (positions.Length != innerCount)
        {
            string msg = $"Amount of positions for settings ({positions.Length}) did not equal collection's inner elements' count ({innerCount}).";
            ThrowHelper.Throw.Argument(nameof(positions), msg);
        }

        for (int i = 0; i < outerCount; i++)
        {
            dynamic[] sorted = new dynamic[5];
            for (int j = 0; j < innerCount; j++)
            {
                sorted[positions[j]] = settings[i, j];
            }

            yield return new(
                sorted[0] ?? sorted[2],
                sorted[1] ?? defaultValue,
                sorted[2] ?? sorted[0],
                sorted[3],
                sorted[4]);
        }
    }
}
