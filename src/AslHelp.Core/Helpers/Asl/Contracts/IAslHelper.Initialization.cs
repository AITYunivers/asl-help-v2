using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.Reflection;

using LiveSplit.Model;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    /// <summary>
    ///     <see cref="Initialization"/> provides an interface
    ///     to implement ASL initialization-related methods.
    /// </summary>
#pragma warning disable IDE1006
    public interface Initialization
#pragma warning restore IDE1006
    {
        /// <summary>
        ///     Gets a <see cref="SettingsCreator"/> instance for creating ASL settings.
        /// </summary>
        SettingsCreator Settings { get; }

        /// <summary>
        ///     Initializes the <see cref="IAslHelper"/> instance.
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="generateCode"></param>
        /// <param name="inject"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IAslHelper Init(string? gameName = null, bool generateCode = false);

        /// <summary>
        ///     Initializes a <see cref="FileLogger"/> instance with the specified file name,
        ///     maximum number of lines, and number of lines to erase when the maximum number of lines is reached.
        /// </summary>
        /// <param name="fileName">The name of the file to log to.</param>
        /// <param name="maxLines">The maximum number of lines to log.</param>
        /// <param name="linesToErase">The number of lines to erase when the maximum number of lines is reached.</param>
        /// <returns>
        ///     The current <see cref="IAslHelper.Initialization"/> instance.
        /// </returns>
        IAslHelper.Initialization LogToFile(string fileName, int maxLines = 4096, int linesToErase = 512);

        /// <summary>
        ///     Displays a message box when the timer's <see cref="LiveSplitState.CurrentTimingMethod"/>
        ///     does not match the target timing method.
        /// </summary>
        /// <param name="message">An optional different message to display in the message box.</param>
        /// <returns>
        ///     The current <see cref="IAslHelper.Initialization"/> instance.
        /// </returns>
        IAslHelper.Initialization AlertRealTime(string? message = null);

        /// <summary>
        ///     Displays a message box when the timer's <see cref="LiveSplitState.CurrentTimingMethod"/>
        ///     does not match the target timing method.
        /// </summary>
        /// <param name="message">An optional different message to display in the message box.</param>
        /// <returns>
        ///     The current <see cref="IAslHelper.Initialization"/> instance.
        /// </returns>
        IAslHelper.Initialization AlertGameTime(string? message = null);

        /// <summary>
        ///     Displays a message box when the timer's <see cref="LiveSplitState.CurrentTimingMethod"/>
        ///     does not match the target timing method.
        /// </summary>
        /// <param name="message">An optional different message to display in the message box.</param>
        /// <returns>
        ///     The current <see cref="IAslHelper.Initialization"/> instance.
        /// </returns>
        IAslHelper.Initialization AlertLoadless(string? message = null);

        /// <summary>
        ///     Defines a custom type from the provided source code.
        /// </summary>
        /// <param name="code">The source code of the type.</param>
        /// <param name="references">An optional amount of references that the source code depends on.</param>
        /// <returns>
        ///     The defined <see cref="ITypeDefinition"/> instance.
        /// </returns>
        ITypeDefinition Define(string code, params string[] references);
    }
}
