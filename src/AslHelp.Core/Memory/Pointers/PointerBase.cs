﻿using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers;

public abstract class PointerBase<T> : IPointer<T>
{
    protected readonly IMemoryManager _manager;

    private readonly PointerBase<nint> _parent;
    private readonly int _baseOffset;

    private readonly nint _base;
    private readonly int[] _offsets;

    private uint _tick;

    public PointerBase(IMemoryManager manager, nint @base, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(offsets);

        _manager = manager;

        _base = @base;
        _offsets = offsets;

        _old = Default;
        _current = Default;
    }

    public PointerBase(IMemoryManager manager, PointerBase<nint> parent, int firstOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(parent);
        ThrowHelper.ThrowIfNull(offsets);

        _manager = manager;

        _parent = parent;
        _baseOffset = firstOffset;
        _offsets = offsets;

        _old = Default;
        _current = Default;
    }

    protected abstract T Default { get; }

    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
    public bool LogChange { get; set; }
    public bool UpdateOnFail { get; set; }

    private T _current;
    public T Current
    {
        get
        {
            if (Enabled && _tick != _manager.Tick)
            {
                Update();
            }

            return _current;
        }
        set => _current = value;
    }

    object IPointer.Current
    {
        get => Current;
        set => Current = (T)value;
    }

    private T _old;
    public T Old
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

    object IPointer.Old
    {
        get => Old;
        set => Old = (T)value;
    }

    public bool Changed => CheckChanged(_old, _current);

    protected nint Address
    {
        get
        {
            if (_parent is null)
            {
                return _manager.Deref(_base, _offsets);
            }
            else
            {
                return _manager.Deref(_parent.Current + _baseOffset, _offsets);
            }
        }
    }

    private void Update()
    {
        _tick = _manager.Tick;
        _old = _current;

        if (!TryUpdate(out T result))
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

    public void Reset()
    {
        _old = Default;
        _current = Default;
        _tick = 0;
    }

    protected abstract bool TryUpdate(out T result);
    protected abstract bool CheckChanged(T old, T current);
    protected abstract bool Write(T value);
    public abstract override string ToString();

    protected string OffsetsToString()
    {
        return $"0x{_base.ToString("X")}, {string.Join(", ", _offsets.Select(o => $"0x{o:X}"))}";
    }
}
