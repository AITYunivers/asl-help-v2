using System.Drawing;
using AslHelp.ClickteamFusion.Memory.Ipc;
using LiveSplit.ComponentUtil;

using static ClickteamFusionMemoryFunctions;

public class CObjectCommon
{
    // Object Common
    public int StructureSize;
    public ushort MovementOffset;
    public ushort AnimationOffset;
    public ushort Version;
    public ushort CounterOffset;
    public ushort DataOffset;
    public BitDict Flags = new(0,
                               "Display In Front",
                               "Background",
                               "Backsave",
                               "Run Before Run In",
                               "Movements",
                               "Animations",
                               "Tab Stop",
                               "Window Process",
                               "Values",
                               "Sprites",
                               "Internal Backsave",
                               "Scrolling Independent",
                               "Quick Display",
                               "Never Kill",
                               "Never Sleep",
                               "Manual Sleep",
                               "Text",
                               "Don't Create at Start");
    public short[] Qualifiers = new short[0];
    public ushort ExtensionOffset;
    public ushort ValuesOffset;
    public ushort StringsOffset;
    public BitDict NewFlags = new(0,
                                  "Don't Save Backdrop",
                                  "Solid Backdrop",
                                  "Collision Box",
                                  "Visible at Start");
    public BitDict Preferences = new(0,
                                     "Backsave",
                                     "Scrolling Independent",
                                     "Quick Display",
                                     "Sleep",
                                     "Load On Call",
                                     "Global",
                                     "Back Effects",
                                     "Kill",
                                     "Ink Effects",
                                     "Transitions",
                                     "Fine Collisions");
    public string Identifier = string.Empty;
    public Color BackgroundColor;
    public int FadeInOffset;
    public int FadeOutOffset;

    // Parsed Offsets
    public CRunOCMovements CRunOCMovements;
    public CRunOCAnimations CRunOCAnimations;
    public CRunOCCounter CRunOCCounter;
    public CRunOCExtension CRunOCExtension;
    public CRunOCAltValues CRunOCAltValues;
    public CRunOCFade CRunOCFadeIn;
    public CRunOCFade CRunOCFadeOut;

    // Parsed Data Offset
    public CRunOCString CRunOCString;
    public CRunOCCounters CRunOCCounters;
    public CRunOCSubApp CRunOCSubApp;
}