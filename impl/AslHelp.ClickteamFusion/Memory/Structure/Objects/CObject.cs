using System.Drawing;
using AslHelp.ClickteamFusion.Memory.Ipc;
using LiveSplit.ComponentUtil;

using static ClickteamFusionMemoryFunctions;

public class CObject
{
    // Object Header
    public short Number;
    public short Next;
    public int Size;
    public short FrmObjNumber;
    public short ObjInfoNumber;
    public short Type;
    public short CreationID;
    public string MovementName = string.Empty;
    public CObjectCommon? ObjectCommon;

    // Object
    public int XPosition;
    public int YPosition;
    public int XHotspot;
    public int YHotspot;
    public int Width;
    public int Height;
    public Rectangle DisplayRect;
    public BitDict ObjectFlags = new(0,
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
    public BitDict Flags = new(0,
                               "Destroyed",
                               "True Event",
                               "Real Sprite",
                               "Fade In",
                               "Fade Out",
                               "Owner Draw", "", "", "", "", "", "", "",
                               "No Collision",
                               "Float",
                               "String");
    public uint Layer;
    public BitDict CollisionFlags = new(0, "", "", "", "",
                                        "Backdrops", "", "",
                                        "On Collide",
                                        "Quick Collision",
                                        "Quick Border",
                                        "Quick Sprite",
                                        "Quick Extension");
    public string Identifier = string.Empty;

    // Animation/Movement Information
    public int CurrentPlayer;
    public int CurrentAnimation;
    public int CurrentFrame;
    public float XScale;
    public float YScale;
    public float Angle;
    public int CurrentDirection;
    public int CurrentSpeed;
    public int MinSpeed;
    public int MaxSpeed;
    public bool UpdateObject;

    // Movement Structure
    public int Acceleration;
    public int Deceleration;
    public int CollisionCount;
    public int StopSpeed;
    public int AccelValue;
    public int DecelValue;
    public BitDict EventFlags = new(0,
                                    "Goes In Playfield",
                                    "Goes Out Playfield",
                                    "Wrap");
    public bool Moving;
    public bool Wrapping;
    public bool Reverse;
    public bool Bouncing;
    public int CurrentMovement;

    // Additional Structure
    public CActiveObject? ActiveObject;
    public CSystemObject? SystemObject;
}