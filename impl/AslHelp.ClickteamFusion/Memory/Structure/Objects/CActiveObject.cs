using System.Drawing;
using AslHelp.ClickteamFusion.Memory.Ipc;
using LiveSplit.ComponentUtil;

using static ClickteamFusionMemoryFunctions;

public class CActiveObject
{
    // Animation Structure
    public bool CurrentAnimationStopped;
    public int CurrentAnimation;
    public int CurrentDirection;
    public int CurrentSpeed;
    public int MinSpeed;
    public int MaxSpeed;
    public int Repeats;
    public int Looping;
    public int CurrentFrame;
    public int CurrentAnimationFrameCount;

    // Sprite Structure
    public int Flash;
    public int Layer;
    public int ZOrder;
    public BitDict CreationFlags = new(0,
                                       "Rambo",
                                       "Recalculate Surface",
                                       "Private",
                                       "Inactive",
                                       "To Hide",
                                       "To Kill",
                                       "Reactivate",
                                       "Hidden",
                                       "Collision Box",
                                       "No Save",
                                       "Fill Back",
                                       "Disabled",
                                       "Inactive Internal",
                                       "Owner Draw",
                                       "Owner Save",
                                       "Fade",
                                       "Obstacle",
                                       "Platform", "",
                                       "Background",
                                       "Scale Resample",
                                       "Rotate Antialiased",
                                       "No Hotspot",
                                       "Owner Collision Mask",
                                       "Update Collision",
                                       "True Object");
    public Color BackgroundColor;
    public int Effect;
    public BitDict Flags = new(0,
                               "Hidden",
                               "Inactive",
                               "Sleeping",
                               "Scale Resample",
                               "Rotate Antialiased",
                               "Visible");
    public BitDict FadeCreateFlags = new (0, "", "", "", "", "", "", "",
                                          "Hidden");

    // Alterable Values
    public CRunValue[]? AlterableValues;
    public BitDict AlterableFlags = new(0,
                                        "Alterable Flag 1",
                                        "Alterable Flag 2",
                                        "Alterable Flag 3",
                                        "Alterable Flag 4",
                                        "Alterable Flag 5",
                                        "Alterable Flag 6",
                                        "Alterable Flag 7",
                                        "Alterable Flag 8",
                                        "Alterable Flag 9",
                                        "Alterable Flag 10",
                                        "Alterable Flag 11",
                                        "Alterable Flag 12",
                                        "Alterable Flag 13",
                                        "Alterable Flag 14",
                                        "Alterable Flag 15",
                                        "Alterable Flag 16",
                                        "Alterable Flag 17",
                                        "Alterable Flag 18",
                                        "Alterable Flag 19",
                                        "Alterable Flag 20",
                                        "Alterable Flag 21",
                                        "Alterable Flag 22",
                                        "Alterable Flag 23",
                                        "Alterable Flag 24",
                                        "Alterable Flag 25",
                                        "Alterable Flag 26",
                                        "Alterable Flag 27",
                                        "Alterable Flag 28",
                                        "Alterable Flag 29",
                                        "Alterable Flag 30",
                                        "Alterable Flag 31",
                                        "Alterable Flag 32");
    public string[]? AlterableStrings;
}