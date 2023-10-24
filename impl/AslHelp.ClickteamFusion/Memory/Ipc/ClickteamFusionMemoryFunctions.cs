using System;
using System.Drawing;
using System.Linq;

using AslHelp.ClickteamFusion.Memory.Ipc;

using LiveSplit.ComponentUtil;

public static class ClickteamFusionMemoryFunctions
{
    public static short ReadShort(IClickteamFusionMemoryManager memory, nuint baseOffset, params int[] offsets)
    {
        if (memory.TryRead(out short result, baseOffset, offsets) == true)
        {
            return result;
        }
        
        return 0;
    }

    public static int ReadInt(IClickteamFusionMemoryManager memory, nuint baseOffset, params int[] offsets)
    {
        if (memory.TryRead(out int result, baseOffset, offsets) == true)
        {
            return result;
        }
        
        return 0;
    }

    public static string ReadString(IClickteamFusionMemoryManager memory, nuint baseOffset, int length = -1, ReadStringType stringType = ReadStringType.AutoDetect, params int[] offsets)
    {
        if (memory.TryReadString(out string? result, length == -1 ? 256 : length, stringType, baseOffset, offsets) == true)
        {
            return result;
        }
        
        return "";
    }

    public static Color ReadColor(IClickteamFusionMemoryManager memory, nuint baseOffset, params int[] offsets)
    {
        if (memory.TryRead(out int result, baseOffset, offsets) == true)
        {
            byte[] values = BitConverter.GetBytes(result);
            return Color.FromArgb(values[3], values[0], values[1], values[2]);
        }
        
        return Color.White;
    }

    public static Rectangle ReadRectangle(IClickteamFusionMemoryManager memory, nuint baseOffset, params int[] offsets)
    {
        int[] topOffsets = offsets;
        topOffsets[^1] += 0x4;
        int[] rightOffsets = offsets;
        rightOffsets[^1] += 0x8;
        int[] bottomOffsets = offsets;
        bottomOffsets[^1] += 0xC;

        if (memory.TryRead(out int left,   baseOffset, offsets)       == true &&
            memory.TryRead(out int top,    baseOffset, topOffsets)    == true &&
            memory.TryRead(out int right,  baseOffset, rightOffsets)  == true &&
            memory.TryRead(out int bottom, baseOffset, bottomOffsets) == true)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }
        
        return new Rectangle(0, 0, 0, 0);
    }

    public static CRunValue ReadCRunValue(IClickteamFusionMemoryManager memory, nuint baseOffset, params int[] offsets)
    {
        int[] valueOffsets = (int[])offsets.Clone();
        valueOffsets[^1] += 0x8;

        if (memory.TryRead(out int type, baseOffset, offsets))
        {
            Debug.Info(string.Format("0x{0:x}", baseOffset) + "+" + string.Join(", ",
                          offsets.Select(x => string.Format("0x{0:x}", x)).ToArray()) + " = " + type);
            switch (type)
            {
                case 0:
                    if (memory.TryRead(out int valI, baseOffset, valueOffsets))
                    {
                        return new CRunValue(type, valI);
                    }

                    break;
                    
                case 1:
                    Array.Resize(ref valueOffsets, valueOffsets.Length + 1);
                    if (memory.TryReadString(out string? valS, 255, baseOffset, valueOffsets))
                    {
                        return new CRunValue(type, valS);
                    }
                    
                    break;
                    
                case 2:
                    if (memory.TryRead(out double valD, baseOffset, valueOffsets))
                    {
                        return new CRunValue(type, valD);
                    }
                    
                    break;
            }
        }
        
        return new CRunValue(type, "");
    }

    public class BitDict
    {
        private readonly int _flag;
        private readonly string[] _flagNames; 
        public int Count => _flagNames.Length;

        public BitDict(int flag, params string[] flagNames)
        {
            _flag = flag;
            _flagNames = flagNames;
        }

        public bool this[string key] => GetFlag(key);

        public bool GetFlag(string key)
        {
            if (key == "" || !_flagNames.Contains(key))
            {
                return false;
            }

            return (_flag & ((uint)Math.Pow(2, Array.IndexOf(_flagNames, key)))) != 0;
        }
    }

    public class CRunValue
    {
        public int ValueType;

        public int IntValue;
        public string StringValue = string.Empty;
        public double DoubleValue;

        public object? Value => ValueType switch
        {
            0 => IntValue,
            1 => StringValue,
            2 => DoubleValue,
            _ => null
        };

        public CRunValue(int valueType, object value)
        {
            ValueType = valueType;
            switch (ValueType)
            {
                case 0:
                    IntValue = (int)value;
                    break;
                case 1:
                    StringValue = (string)value;
                    break;
                case 2:
                    DoubleValue = (double)value;
                    break;
                default:
                    StringValue = value.ToString();
                    break;
            }
        }
    }
}