using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Commands;

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
        ThrowHelper.ThrowIfNull(offsets);

        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.MainModule;
    }

    protected MakePointerCommandBase(string moduleName, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(offsets);

        _moduleName = moduleName;
        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.ModuleName;
    }

    protected MakePointerCommandBase(Module module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module);
        ThrowHelper.ThrowIfNull(offsets);

        _module = module;
        _baseOffset = baseOffset;
        _offsets = offsets;

        _command = CommandType.Module;
    }

    protected MakePointerCommandBase(nint baseAddress, int[] offsets)
    {
        ThrowHelper.ThrowIfAddressInvalid(baseAddress, "Attempted to construct pointer from null address.");
        ThrowHelper.ThrowIfNull(offsets);

        _baseAddress = baseAddress;
        _offsets = offsets;

        _command = CommandType.Absolute;
    }

    protected bool TryGetBaseAddress(IMemoryManager manager, out nint baseAddress)
    {
        if (_command == CommandType.Absolute)
        {
            baseAddress = _baseAddress;
            return true;
        }

        Module module = _command switch
        {
            CommandType.Module => _module,
            CommandType.MainModule => manager.MainModule,
            CommandType.ModuleName => manager.Modules.FirstOrDefault(m => m.Name == _moduleName),
            _ => throw new InvalidOperationException()
        };

        if (module is not null && module.Base > 0 && module.MemorySize > 0)
        {
            if (_baseOffset > module.MemorySize)
            {
                string msg = "Base offset was larger than module's memory size.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            baseAddress = module.Base + _baseOffset;
            return true;
        }
        else
        {
            baseAddress = default;
            return false;
        }
    }

    public abstract bool TryExecute(IMemoryManager manager, out IPointer pointer);
}
