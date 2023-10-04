using System.Collections.Generic;
using System.Diagnostics;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

public partial class Basic
{
    private readonly Dictionary<string, IPointer> _pointerCache = new();

    protected IPointerFactory? _pointers;
    public override IPointerFactory? Pointers
    {
        get
        {
            if (_pointers is not null)
            {
                return _pointers;
            }

            if (Game is Process game)
            {
                _memory = InitializeMemory(game);
                _pointers = new PointerFactory(_memory);
            }

            return _pointers;
        }
    }
}
