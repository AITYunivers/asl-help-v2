using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AslHelp.Core.LiveSplitInterop.Settings;

public sealed partial class SettingsCreator
{
    [XmlType("Setting")]
    public struct XmlSetting
    {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string State;
        [XmlAttribute] public string Label;
        [XmlAttribute] public string Parent;
        [XmlAttribute] public string ToolTip;

        [XmlElement("Setting")] public XmlSetting[] Children;
    }

    public SettingsCreator FromXml(string path, bool defaultValue = true, string defaultParent = null)
    {
        using FileStream fs = File.OpenRead(path);
        XmlSerializer ser = new(typeof(XmlSetting[]), new XmlRootAttribute("Settings"));
        if (ser.Deserialize(fs) is not XmlSetting[] settings)
        {
            string msg = "Xml settings file was in an incorrect format.";
            throw new FormatException(msg);
        }

        IEnumerable<Setting> converted = EnumerateXmlSetting(settings, defaultValue, null);
        return Create(converted, defaultParent);
    }

    private IEnumerable<Setting> EnumerateXmlSetting(XmlSetting[] nodes, bool defaultValue, string defaultParent)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            XmlSetting node = nodes[i];
            yield return new(
                node.Id,
                bool.TryParse(node.State, out bool pState) ? pState : defaultValue,
                node.Label,
                node.Parent ?? defaultParent,
                node.ToolTip);

            if (node.Children is { Length: > 0 } children)
            {
                foreach (Setting setting in EnumerateXmlSetting(children, defaultValue, node.Id))
                {
                    yield return setting;
                }
            }
        }
    }
}
