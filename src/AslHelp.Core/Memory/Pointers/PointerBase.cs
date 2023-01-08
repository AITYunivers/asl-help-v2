using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Memory.Pointers;

public abstract class PointerBase<T> : IPointer<T>
{
    protected readonly IHelper _helper;

    private readonly PointerBase<nint> _parent;
    private readonly int _baseOffset;

    private readonly nint _base;
    private readonly int[] _offsets;

    private uint _tick;

    public PointerBase(IHelper helper, nint @base, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(helper);
        ThrowHelper.ThrowIfNull(offsets);

        _helper = helper;

        _base = @base;
        _offsets = offsets;

        _old = Default;
        _current = Default;
    }

    public PointerBase(IHelper helper, PointerBase<nint> parent, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(helper);
        ThrowHelper.ThrowIfNull(parent);
        ThrowHelper.ThrowIfNull(offsets);

        _helper = helper;

        _parent = parent;
        _baseOffset = baseOffset;
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
            if (Enabled && _tick != _helper.Tick)
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
            if (Enabled && _tick != _helper.Tick)
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
                return _helper.Deref(_base, _offsets);
            }
            else
            {
                return _helper.Deref(_parent.Current + _baseOffset, _offsets);
            }
        }
    }

    private void Update()
    {
        _tick = _helper.Tick;
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
            _helper.Log($"[{Name}] changed: [Old] {_old} -> [Current] {_current}");
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
