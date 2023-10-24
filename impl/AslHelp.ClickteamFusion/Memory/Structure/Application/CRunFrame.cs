using System.Drawing;
using AslHelp.ClickteamFusion.Memory.Ipc;
using LiveSplit.ComponentUtil;

using static ClickteamFusionMemoryFunctions;

public class CRunFrame
{
    private readonly nuint _frameOffset = 0;
    private readonly IClickteamFusionMemoryManager _memoryManager;

    public CRunFrame(nuint loadedCCN, IClickteamFusionMemoryManager memoryManager)
    {
        _frameOffset = loadedCCN + 0x4;
        _memoryManager = memoryManager;
    }

    private short ReadShort(params int[] offsets) 
    {
        return ClickteamFusionMemoryFunctions.ReadShort(_memoryManager, _frameOffset, offsets);
    }

    private int ReadInt(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadInt(_memoryManager, _frameOffset, offsets);
    }

    private string ReadASCII(params int[] offsets)
    {
        return ReadString(-1, ReadStringType.ASCII, offsets);
    }

    private string ReadUnicode(params int[] offsets)
    {
        return ReadString(-1, ReadStringType.UTF16, offsets);
    }

    private string ReadString(int length = -1, ReadStringType stringType = ReadStringType.AutoDetect, params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadString(_memoryManager, _frameOffset, length, stringType, offsets);
    }

    private Color ReadColor(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadColor(_memoryManager, _frameOffset, offsets);
    }

    private Rectangle ReadRectangle(params int[] offsets)
    {
        return ClickteamFusionMemoryFunctions.ReadRectangle(_memoryManager, _frameOffset, offsets);
    }

    public Size Size => new(ReadInt(0x0), ReadInt(0x4));
    public Color BackgroundColor => ReadColor(0x8);
    public BitDict Flags => new(ReadInt(0xC), 
                                "Display Name",
                                "Grab Display",
                                "Keep Display", "", "",
                                "Total Collision Mask",
                                "Password", "",
                                "Resize At Start",
                                "Do Not Center",
                                "Force Load On Call",
                                "No Surface", "", "", "",
                                "Timed Movements");
    public string Name => ReadUnicode(0x10, 0x0);
    public Point CameraPosition => new(ReadInt(0x1C), ReadInt(0x20));
    public Point OldCameraPosition => new(ReadInt(0x24), ReadInt(0x28));
    public int LayerCount => ReadInt(0x2C);
    //public Layer[] Layers => ;
    public Rectangle CameraBounds => ReadRectangle(0x34);
}