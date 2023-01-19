using AslHelp.Core.Reflection;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIOBase
    : IMemoryReader
{
    private readonly IProcessMemoryManager _manager;





    public string ReadString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
