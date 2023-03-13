using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

internal interface IMakePointerCommand
{
    bool TryExecute(IMemoryManager manager, out IPointer pointer);
}

internal enum CommandType
{
    MainModule,
    ModuleName,
    Module,
    Absolute
}

internal abstract class MakePointerCommandBase
    : IMakePointerCommand
{
    protected readonly int _baseOffset;
    protected readonly string _moduleName;
    protected readonly Module _module;
    protected readonly nint _baseAddress;

    protected readonly int[] _offsets;
    protected readonly CommandType _command;

    protected MakePointerCommandBase(int baseOffset, int[] offsets)
    {
        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.MainModule;
    }

    protected MakePointerCommandBase(string moduleName, int baseOffset, params int[] offsets)
    {
        _moduleName = moduleName;
        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.ModuleName;
    }

    protected MakePointerCommandBase(Module module, int baseOffset, params int[] offsets)
    {
        _module = module;
        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.Module;
    }

    protected MakePointerCommandBase(nint baseAddress, int[] offsets)
    {
        _baseAddress = baseAddress;
        _offsets = offsets;

        _command = CommandType.Absolute;
    }

    public abstract bool TryExecute(IMemoryManager manager, out IPointer pointer);
}

internal sealed class MakePointerCommand<T>
    : MakePointerCommandBase
    where T : unmanaged
{
    public MakePointerCommand(int baseOffset, int[] offsets)
        : base(baseOffset, offsets) { }

    public MakePointerCommand(string moduleName, int baseOffset, params int[] offsets)
        : base(moduleName, baseOffset, offsets) { }

    public MakePointerCommand(Module module, int baseOffset, params int[] offsets)
        : base(module, baseOffset, offsets) { }

    public MakePointerCommand(nint baseAddress, int[] offsets)
        : base(baseAddress, offsets) { }

    public override bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (_command == CommandType.Absolute)
        {
            pointer = new Pointer<T>(manager, _baseAddress, _offsets);
            return true;
        }

        Module module = _command switch
        {
            CommandType.Module => _module,
            CommandType.MainModule => manager.MainModule,
            CommandType.ModuleName => manager.Modules.FirstOrDefault(m => m.Name == _moduleName),
            _ => throw new InvalidOperationException()
        };

        if (module is not null && module.Base > 0)
        {
            pointer = new Pointer<T>(manager, module.Base + _baseOffset, _offsets);
            return true;
        }
        else
        {
            pointer = null;
            return false;
        }
    }
}

internal interface IMakeStringPointerCommand
    : IMakePointerCommand
{

}

internal interface IMakeSpanPointerCommand<T>
    : IMakePointerCommand
{

}

public class LazyPointerFactory
    : ILazyPointerFactory,
    IParentStage,
    IChildStage,
    IChildOrNext
{
    ILazyPointerFactory ILazyPointerFactory.Make<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
