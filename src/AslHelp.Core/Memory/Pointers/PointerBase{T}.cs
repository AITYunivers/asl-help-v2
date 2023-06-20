using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Core.Memory.Pointers;

public abstract class PointerBase<T> : IPointer<T>
{
    protected readonly IMemoryManager _manager;

    private readonly IPointer<nuint>? _parent;
    private readonly uint _baseOffset;

    private readonly nuint _baseAddress;
    private readonly int[] _offsets;

    private uint _tick;

    public PointerBase(IMemoryManager manager, nuint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(offsets);

        _manager = manager;

        _baseAddress = baseAddress;
        _offsets = offsets;

        _old = Default;
        _current = Default;
    }

    public PointerBase(IMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(parent);
        ThrowHelper.ThrowIfLessThan(nextOffset, 0);
        ThrowHelper.ThrowIfNull(remainingOffsets);

        _manager = manager;

        _parent = parent;
        _baseOffset = (uint)nextOffset;
        _offsets = remainingOffsets;

        _old = Default;
        _current = Default;
    }

    protected abstract T? Default { get; }

    public string? Name { get; set; }
    public bool Enabled { get; set; } = true;
    public bool LogChange { get; set; }
    public bool UpdateOnFail { get; set; }

    private T? _current;
    public T? Current
    {
        get
        {
            if (_tick != _manager.Tick)
            {
                Update();
            }

            return _current;
        }
        set => _current = value;
    }

    object? IPointer.Current
    {
        get => Current;
        set => Current = (T?)value;
    }

    private T? _old;
    public T? Old
    {
        get
        {
            if (Enabled && _tick != _manager.Tick)
            {
                Update();
            }

            return _old;
        }
        set => _old = value;
    }

    object? IPointer.Old
    {
        get => Old;
        set => Old = (T?)value;
    }

    public bool Changed => HasChanged(_old, _current);

    public nuint Deref()
    {
        if (_parent is null)
        {
            return _manager.Deref(_baseAddress, _offsets);
        }
        else
        {
            return _manager.Deref(_parent.Current + _baseOffset, _offsets);
        }
    }

    bool IPointer.Write(object? value)
    {
        return Write((T?)value);
    }

    public bool Write(T? value)
    {
        return value is not null && Write(value, Deref());
    }

    private void Update()
    {
        if (!Enabled)
        {
            return;
        }

        _tick = _manager.Tick;
        _old = _current;

        if (!TryUpdate(out T? result, Deref()))
        {
            if (!UpdateOnFail)
            {
                return;
            }

            _current = Default;
        }
        else
        {
            _current = result;
        }

        if (LogChange && Changed)
        {
            _manager.Log($"[{Name}] changed: [Old] {_old} -> [Current] {_current}");
        }
    }

    public IPointer SetName(string name)
    {
        Name = name;
        return this;
    }

    public IPointer SetLogChange(bool logChange = true)
    {
        LogChange = logChange;
        return this;
    }

    public IPointer SetUpdateOnFail(bool updateOnFail = true)
    {
        UpdateOnFail = updateOnFail;
        return this;
    }

    public void Reset()
    {
        _old = Default;
        _current = Default;
        _tick = 0;
    }

    protected abstract bool TryUpdate([NotNullWhen(true)] out T? result, nuint address);
    protected abstract bool Write(T value, nuint address);
    protected abstract bool HasChanged(T? old, T? current);
    public abstract override string ToString();

    protected string OffsetsToString()
    {
        return $"0x{((nint)_baseAddress).ToString("X")}, {string.Join(", ", _offsets.Select(o => $"0x{o:X}"))}";
    }
}
