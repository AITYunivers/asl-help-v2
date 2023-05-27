using System.Diagnostics;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping.Asl.Contracts;

/// <summary>
///     The <see cref="IAslHelper"/> interface
///     exposes public members for interacting with various parts related to auto splitting.
/// </summary>
public partial interface IAslHelper
{
    /// <summary>
    ///     Gets the name of the target process.
    /// </summary>
    string GameName { get; }

    /// <summary>
    ///     Gets an <see cref="ILogger"/> instance for debug logging.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    ///     Gets a <see cref="TextComponentController"/> for creating and interacting with LiveSplit text components.
    /// </summary>
    /// <remarks>
    ///     <see cref="Texts"/> should not be available in <c>'shutdown'</c>.
    /// </remarks>
    TextComponentController Texts { get; }

    /// <summary>
    ///     Gets a <see cref="TimerController"/> for conveniently interacting with LiveSplit's timer.
    /// </summary>
    TimerController Timer { get; }

    /// <summary>
    ///     Gets a <see cref="SettingsCreator"/> for creating auto splitter settings.
    /// </summary>
    /// <remarks>
    ///     <see cref="Settings"/> should only be available in <c>'startup'</c>.
    /// </remarks>
    SettingsCreator Settings { get; }

    /// <summary>
    ///     Gets the target <see cref="Process"/> that the auto splitter is attached to.
    /// </summary>
    /// <remarks>
    ///     <see cref="Game"/> should not be available in <c>'startup'</c>, <c>'exit'</c>, and <c>'shutdown'</c>.
    /// </remarks>
    Process Game { get; set; }

    /// <summary>
    ///     Gets an <see cref="IMemoryManager"/> instance for reading from and writing to the target process' memory.
    /// </summary>
    /// <remarks>
    ///     <see cref="Memory"/> should only be available when the <see cref="IAslHelper"/> is attached to a game.<br/>
    ///     <see cref="Memory"/> should not be available in <c>'startup'</c>, <c>'exit'</c>, and <c>'shutdown'</c>.
    /// </remarks>
    IMemoryManager Memory { get; }

    /// <summary>
    ///     Gets a <see cref="PointerFactory"/> for creating <see cref="IPointer"/> instances.
    /// </summary>
    /// <remarks>
    ///     <see cref="Pointers"/> should only be available when the <see cref="IAslHelper"/> is attached to a game.<br/>
    ///     <see cref="Pointers"/> should not be available in <c>'startup'</c>, <c>'exit'</c>, and <c>'shutdown'</c>.
    /// </remarks>
    PointerFactory Pointers { get; }

    /// <summary>
    ///     Gets or sets an <see cref="IPointer"/> instance by name.
    /// </summary>
    /// <param name="name">The unique identifier of the <see cref="IPointer"/> instance to get or set.</param>
    IPointer this[string name] { get; set; }

    /// <summary>
    ///     Assigns the values of all <see cref="IPointer"/> instances added to the <see cref="IAslHelper"/>
    ///     through <see cref="this[string]"/> to the auto splitter script's <c>current</c> container.
    /// </summary>
    /// <returns>
    ///     The current instance.
    /// </returns>
    IAslHelper MapPointerValuesToCurrent();

    /// <summary>
    ///     Creates and starts a <see cref="FileLogger"/> at the specified <paramref name="filePath"/>,
    ///     assigning or adding it to <see cref="Logger"/>.
    /// </summary>
    /// <param name="filePath">The fully qualified file path of the log file.</param>
    /// <returns>
    ///     The current instance.
    /// </returns>
    /// <remarks>
    ///     The <see cref="FileLogger"/> will be created with a maximum of 4096 lines
    ///     and will erase the first 512 lines once the maximum is reached.
    /// </remarks>
    IAslHelper CreateFileLogger(string filePath);

    /// <summary>
    ///     Creates and starts a <see cref="FileLogger"/> at the specified <paramref name="filePath"/>,
    ///     assigning or adding it to <see cref="Logger"/>.
    /// </summary>
    /// <param name="filePath">The fully qualified file path of the log file.</param>
    /// <returns>
    ///     The current instance.
    /// </returns>
    /// <remarks>
    ///     The <see cref="FileLogger"/> will be created with a maximum of <paramref name="maxLines"/> lines
    ///     and will erase the first 512 lines once the maximum is reached.
    /// </remarks>
    IAslHelper CreateFileLogger(string filePath, int maxLines);

    /// <summary>
    ///     Creates and starts a <see cref="FileLogger"/> at the specified <paramref name="filePath"/>,
    ///     assigning or adding it to <see cref="Logger"/>.
    /// </summary>
    /// <param name="filePath">The fully qualified file path of the log file.</param>
    /// <returns>
    ///     The current instance.
    /// </returns>
    /// <remarks>
    ///     The <see cref="FileLogger"/> will be created with a maximum of <paramref name="maxLines"/> lines
    ///     and will erase the first <paramref name="linesToErase"/> lines once the maximum is reached.
    /// </remarks>
    IAslHelper CreateFileLogger(string filePath, int maxLines, int linesToErase);

    /// <summary>
    ///     Creates and starts a <see cref="FileWatcher"/> of the file at the specified <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">The fully qualified file path of the file to watch.</param>
    FileWatcher CreateFileWatcher(string filePath);

    /// <summary>
    ///     Releases resources when the auto splitter's <c>'exit'</c> action runs.
    /// </summary>
    /// <returns>
    ///     The current instance.
    /// </returns>
    IAslHelper Exit();

    /// <summary>
    ///     Disposes of all resources related to the <see cref="IAslHelper"/> when the auto splitter's <c>'shutdown'</c> action runs.
    /// </summary>
    /// <returns>
    ///     The current instance.
    /// </returns>
    IAslHelper Shutdown();
}
