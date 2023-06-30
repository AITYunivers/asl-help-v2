using AslHelp.Common.Memory;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase
{

    public string ReadString(uint baseOffset, params int[] offsets)
    {
        return ReadString(IOR.DefaultStringReadLength, ReadStringType.AutoDetect, baseOffset, offsets);
    }
}
