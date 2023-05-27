using System.Linq;
using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers;

public abstract class PointerBase<T> : IPointer<T>
{
    protected readonly IMemoryManager _manager;

    private readonly IPointer<nint> _parent;
    private readonly int _baseOffset;

    private readonly nint _baseAddress;
    private readonly int[] _offsets;

    private uint _tick;

    public PointerBase(IMemoryManager manager, nint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(offsets);

        _manager = manager;

        _baseAddress = baseAddress;
        _offsets = offsets;

        _old = Default;
        _current = Default;
    }

    public PointerBase(IMemoryManager manager, IPointer<nint> parent, int nextOffset, params int[] remainingOffsets)
    {
        ThrowHelper.ThrowIfNull(manager);
        ThrowHelper.ThrowIfNull(parent);
        ThrowHelper.ThrowIfLessThan(nextOffset, 0);
        ThrowHelper.ThrowIfNull(remainingOffsets);

        _manager = manager;

        _parent = parent;
        _baseOffset = nextOffset;
        _offsets = remainingOffsets;

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
                return _manager.Deref(_baseAddress, _offsets);
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

    public IPointer SetName(string name)
    {
        Name = name;
        return this;
    }

    public IPointer SetLogChange()
    {
        LogChange = true;
        return this;
    }

    public IPointer SetUpdateOnFail()
    {
        UpdateOnFail = true;
        return this;
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
        return $"0x{_baseAddress.ToString("X")}, {string.Join(", ", _offsets.Select(o => $"0x{o:X}"))}";
    }
}
