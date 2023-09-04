using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl.Contracts;

/// <summary>
///     <see cref="IAslHelper"/> provides an interface
///     to implement ASL-related convenience methods.
/// </summary>
public partial interface IAslHelper
{
    /// <summary>
    ///     Gets an <see cref="ILogger"/> instance used to log debug information.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    ///     Gets an <see cref="TimerController"/> instance for controlling the current LiveSplit timer.
    /// </summary>
    TimerController Timer { get; }

    /// <summary>
    ///     Gets an <see cref="TextComponentController"/> instance for creating and controlling LiveSplit text components.
    /// </summary>
    TextComponentController Texts { get; }

    /// <summary>
    ///     Initializes an instance of the <see cref="FileWatcher"/> class with the specified file name.
    /// </summary>
    /// <param name="fileName">The name of the file to watch.</param>
    FileWatcher CreateFileWatcher(string fileName);

    /// <summary>
    ///     Gets the name of the game.
    /// </summary>
    /// <remarks>
    ///     This property can be set by the <see cref="IAslHelper.Initialization.SetGameName"/> method.
    /// </remarks>
    string GameName { get; }

    /// <summary>
    ///     Gets or sets the game process.
    /// </summary>
    /// <value>
    ///     This property can return <see langword="null"/> if the game process is not set or initialized.
    /// </value>
    Process? Game { get; set; }

    /// <summary>
    ///     Gets an <see cref="IMemoryManager"/> for interacting with <see cref="Game"/>'s memory.
    /// </summary>
    /// <value>
    ///     This property can return <see langword="null"/> if the game process is not set or initialized.
    /// </value>
    IMemoryManager? Memory { get; }

    /// <summary>
    ///     Gets an <see cref="IPointerFactory"/> for creating <see cref="IPointer"/> instances.
    /// </summary>
    /// <value>
    ///     This property can return <see langword="null"/> if the game process is not set or initialized.
    /// </value>
    IPointerFactory? Pointers { get; }

    /// <summary>
    ///     Checks whether the game's main module's memory size matches any of the specified values.
    /// </summary>
    /// <param name="moduleMemorySizes">The memory sizes to check against.</param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="IMemoryManager.MainModule"/>'s memory size matches any of the specified values;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    bool Reject(params uint[] moduleMemorySizes);

    /// <summary>
    ///     Checks whether the module with the specified name's memory size matches any of the specified values.
    /// </summary>
    /// <param name="moduleName">The name of the module to check.</param>
    /// <param name="moduleMemorySizes">The memory sizes to check against.</param>
    /// <returns>
    ///     <see langword="true"/> if the module's memory size matches any of the specified values;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    bool Reject(string moduleName, params uint[] moduleMemorySizes);

    /// <summary>
    ///     Checks whether the specified <see cref="Module"/>'s memory size matches any of the specified values.
    /// </summary>
    /// <param name="module">The module to check.</param>
    /// <param name="moduleMemorySizes">The memory sizes to check against.</param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Module"/>'s memory size matches any of the specified values;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    bool Reject(Module module, params uint[] moduleMemorySizes);

    /// <summary>
    ///     Releases resources when the ASL script's <c>exit</c> action is run.
    /// </summary>
    void OnExit();

    /// <summary>
    ///     Releases resources when the ASL script's <c>shutdown</c> action is run.
    /// </summary>
    void OnShutdown();
}
