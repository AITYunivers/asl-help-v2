using System;

namespace AslHelp.Core.Memory;

/// <summary>
///     Specifies the type of the <see cref="SYMBOL_INFOW"/>.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/ns-dbghelp-symbol_infow#members">SYMBOL_INFOW structure (dbghelp.h)</see></i>.
/// </remarks>
[Flags]
internal enum SymFlags : uint
{
    /// <summary>
    ///     The <see cref="SYMBOL_INFOW.Value"/> member is used.
    /// </summary>
    VALUEPRESENT = 0x00001,

    /// <summary>
    ///     The symbol is a register. The <see cref="SYMBOL_INFOW.Register"/> member is used.
    /// </summary>
    REGISTER = 0x00008,

    /// <summary>
    ///     Offsets are register relative.
    /// </summary>
    REGREL = 0x00010,

    /// <summary>
    ///     Offsets are frame relative.
    /// </summary>
    FRAMEREL = 0x00020,

    /// <summary>
    ///     The symbol is a parameter.
    /// </summary>
    PARAMETER = 0x00040,

    /// <summary>
    ///     The symbol is a local variable.
    /// </summary>
    LOCAL = 0x00080,

    /// <summary>
    ///     The symbol is a constant.
    /// </summary>
    CONSTANT = 0x00100,

    /// <summary>
    ///     The symbol is from the export table.
    /// </summary>
    EXPORT = 0x00200,

    /// <summary>
    ///     The symbol is a forwarder.
    /// </summary>
    FORWARDER = 0x00400,

    /// <summary>
    ///     The symbol is a known function.
    /// </summary>
    FUNCTION = 0x00800,

    /// <summary>
    ///     The symbol is a virtual symbol created by the SymAddSymbol function.
    /// </summary>
    VIRTUAL = 0x01000,

    /// <summary>
    ///     The symbol is a thunk.
    /// </summary>
    THUNK = 0x02000,

    /// <summary>
    ///     The symbol is an offset into the TLS data area.
    /// </summary>
    TLSREL = 0x04000,

    /// <summary>
    ///     The symbol is a managed code slot.
    /// </summary>
    SLOT = 0x08000,

    /// <summary>
    ///     The symbol address is an offset relative to the beginning of the intermediate language block.
    /// </summary>
    ILREL = 0x10000,

    /// <summary>
    ///     The symbol is managed metadata.
    /// </summary>
    METADATA = 0x20000,

    /// <summary>
    ///     The symbol is a CLR token.
    /// </summary>
    CLR_TOKEN = 0x40000
}

