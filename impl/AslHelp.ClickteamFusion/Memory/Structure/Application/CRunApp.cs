using System;
using System.Drawing;
using AslHelp.ClickteamFusion.Memory.Ipc;
using LiveSplit.ComponentUtil;

using static ClickteamFusionMemoryFunctions;

public class CRunApp
{
    private readonly nuint _appOffset = 0;
    private readonly IClickteamFusionMemoryManager _memoryManager;

    public CRunApp(nuint loadedCCN, IClickteamFusionMemoryManager memoryManager)
    {
        _appOffset = loadedCCN;
        _memoryManager = memoryManager;
    }

    private short ReadShort(params int[] offsets) 
    {
        return ClickteamFusionMemoryFunctions.ReadShort(_memoryManager, _appOffset, offsets);
    }

    private int ReadInt(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadInt(_memoryManager, _appOffset, offsets);
    }

    private string ReadASCII(int length, params int[] offsets)
    {
        return ReadString(-1, ReadStringType.ASCII, offsets);
    }

    private string ReadUnicode(int length, params int[] offsets)
    {
        return ReadString(-1, Header == "PAME" ? ReadStringType.ASCII : ReadStringType.UTF16, offsets);
    }

    private string ReadString(int length = -1, ReadStringType stringType = ReadStringType.AutoDetect, params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadString(_memoryManager, _appOffset, length, stringType, offsets);
    }

    private Color ReadColor(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadColor(_memoryManager, _appOffset, offsets);
    }

    private Rectangle ReadRectangle(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadRectangle(_memoryManager, _appOffset, offsets);
    }

    private CRunValue ReadCRunValue(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadCRunValue(_memoryManager, _appOffset, offsets);
    }

    private object?[] GetGlobalValues(params int[] offsets)
    {
        object?[] globalValues = new object?[GlobalValueCount];
        Array.Resize(ref offsets, offsets.Length + 1);
        for (int i = 0; i < globalValues.Length; i++)
        {
            offsets[^1] = i * 16;
            globalValues[i] = ClickteamFusionMemoryFunctions.ReadCRunValue(_memoryManager, _appOffset, offsets).Value;
        }

        return globalValues;
    }

    private string[] GetGlobalStrings(params int[] offsets)
    {
        string[] globalStrings = new string[GlobalStringCount];
        Array.Resize(ref offsets, offsets.Length + 2);
        for (int i = 0; i < globalStrings.Length; i++)
        {
            offsets[^2] = i * 4;
            globalStrings[i] = ReadUnicode(-1, offsets);
        }

        return globalStrings;
    }

    public string Header => ReadASCII(4, 0x0);
    public short Version => ReadShort(0x4);
    public short SubVersion => ReadShort(0x6);
    public int ProductVersion => ReadInt(0x8);
    public int ProductBuild => ReadInt(0xC);
    public BitDict Flags => new(ReadShort(0x14), 
                                "Maximized Border",
                                "No Heading",
                                "Panic",
                                "Machine Independent Speed",
                                "Stretch", "", "",
                                "Menu Hidden",
                                "Menu Bar",
                                "Maximized",
                                "Mix Samples",
                                "Fullscreen at Start",
                                "Togglable Fullscreen",
                                "Protected",
                                "Copyright",
                                "OneFile");
    public BitDict NewFlags => new(ReadShort(0x16), 
                                   "Samples Over Frames",
                                   "Relocate Files",
                                   "Run Frame",
                                   "Play Samples When Unfocused",
                                   "No Minimize Box",
                                   "No Maximize Box",
                                   "No Thick Frame",
                                   "Do Not Center Frame",
                                   "Don't Auto-Stop Screensaver",
                                   "Disable Close",
                                   "Hidden At Start",
                                   "XP Visual Theme Support",
                                   "VSync",
                                   "Run When Minimized",
                                   "MDI",
                                   "Run While Resizing");
    public short Mode => ReadShort(0x18);
    public BitDict OtherFlags => new(ReadShort(0x1A), 
                                     "Debugger Shortcuts",
                                     "Debugger Draw",
                                     "Debugger Draw VRam",
                                     "DontShareSubData",
                                     "Auto Image Filter",
                                     "Auto Sound Filter",
                                     "All In One",
                                     "Show Debugger",
                                     "Java Swing",
                                     "Java Applet", "", "", "", "",
                                     "Direct3D9or11",
                                     "Direct3D8or11");
    public Size WindowSize => new(ReadShort(0x1C), ReadShort(0x1E));
    public Color BorderColor => ReadColor(0x70);
    public int FrameCount => ReadInt(0x74);
    public int FrameRate => ReadInt(0x78);
    public string Name => ReadUnicode(-1, 0x80, 0x0);
    public string AppFileName => ReadUnicode(-1, 0x84, 0x0);
    public string EditorFileName => ReadUnicode(-1, 0x88, 0x0);
    public string Copyright => ReadUnicode(-1, 0x8C, 0x0);
    public string About => ReadUnicode(-1, 0x90, 0x0);
    public string TargetFileName => ReadUnicode(-1, 0x94, 0x0);
    public string TempPath => ReadUnicode(-1, 0x98, 0x0);
    public string HelpFileName => ReadUnicode(-1, 0xA4, 0x0);
    public int ObjectCount => ReadInt(0x190);
    public int CurrentFrame => ReadInt(0x1EC);
    public int LoadedFrame => ReadInt(0x1F0);
    public BitDict AppRunFlags => new(ReadShort(0x1FA), 
                                      "Menu Initialized",
                                      "Menu Images Loaded",
                                      "In-Game Loop",
                                      "Pause Before Modal Loop");
    //public Player[] Players => ;
    public int GlobalValueCount => ReadInt(0x264);
    public object?[] GlobalValues => GetGlobalValues(0x268);
    public int GlobalStringCount => ReadInt(0x278);
    public string[] GlobalStrings => GetGlobalStrings(0x27C);
}