using System.CodeDom.Compiler;
using System.Text;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Exceptions;

/// <summary>
///     The exception that is thrown when the <see cref="TypeDefinitionFactory"/> encounters an error.
/// </summary>
internal class TypeDefinitionCompilerException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TypeDefinitionCompilerException"/> class
    ///     with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public TypeDefinitionCompilerException(string message)
        : base("asl-help compilation errors: " + message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TypeDefinitionCompilerException"/> class
    ///     with the specified <see cref="CompilerErrorCollection"/>.
    /// </summary>
    /// <param name="errors">The <see cref="CompilerErrorCollection"/> that contains the generated errors.</param>
    public TypeDefinitionCompilerException(CompilerErrorCollection errors)
        : base("asl-help compilation errors:" + GetMessage(errors)) { }

    /// <summary>
    ///     Throws a <see cref="TypeDefinitionCompilerException"/>
    ///     with the specified <see cref="CompilerErrorCollection"/> of generated errors.
    /// </summary>
    /// <param name="errors">The <see cref="CompilerErrorCollection"/> that contains the generated errors.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="TypeDefinitionCompilerException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Throw(CompilerErrorCollection errors)
    {
        throw new TypeDefinitionCompilerException(errors);
    }

    /// <summary>
    ///     The <see cref="TypeDefinitionCompilerException"/> that is thrown
    ///     when no type was defined in the source code.
    /// </summary>
    /// <exception cref="TypeDefinitionCompilerException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowIfNoType()
    {
        throw new TypeDefinitionCompilerException("The provided source code must contain a type.");
    }

    /// <summary>
    ///     The <see cref="TypeDefinitionCompilerException"/> that is thrown
    ///     when the first defined type was not a value type.
    /// </summary>
    /// <exception cref="TypeDefinitionCompilerException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowIfNotValueType()
    {
        throw new TypeDefinitionCompilerException("The first defined type was not a value type.");
    }

    /// <summary>
    ///     Builds a message from the specified <see cref="CompilerErrorCollection"/>.
    /// </summary>
    /// <param name="errors">The <see cref="CompilerErrorCollection"/> that contains the generated errors.</param>
    /// <returns>
    ///     The complete error message.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="errors"/> is <see langword="null"/>.
    /// </exception>
    private static string GetMessage(CompilerErrorCollection errors)
    {
        ThrowHelper.ThrowIfNull(errors);

        StringBuilder sb = new();

        foreach (CompilerError error in errors)
        {
            _ = sb
            .Append(Environment.NewLine)
            .Append("Line").Append(error.Line).Append(", ")
            .Append("Col").Append(error.Column).Append(": ")
            .Append(error.IsWarning ? "warning" : "error").Append(" ")
            .Append(error.ErrorNumber).Append(": ")
            .Append(error.ErrorText);
        }

        return sb.ToString();
    }
}
