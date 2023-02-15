using System.CodeDom.Compiler;
using System.Text;

namespace AslHelp.Core.Exceptions;

internal class TypeDefinitionCompilerException : Exception
{
    public TypeDefinitionCompilerException(string message)
        : base("asl-help compilation errors: " + message) { }

    public TypeDefinitionCompilerException(CompilerErrorCollection errors)
        : base("asl-help compilation errors:" + GetMessage(errors)) { }

    public static void Throw(CompilerErrorCollection errors)
    {
        throw new TypeDefinitionCompilerException(errors);
    }

    public static void ThrowIfNoType()
    {
        throw new TypeDefinitionCompilerException("The provided source code must contain a type.");
    }

    public static void ThrowIfMoreThanOneType()
    {
        throw new TypeDefinitionCompilerException("The provided source code cannot contain more than one type.");
    }

    public static void ThrowIfNotValueType()
    {
        throw new TypeDefinitionCompilerException("The first defined type was not a value type.");
    }

    private static string GetMessage(CompilerErrorCollection errors)
    {
        if (errors == null)
        {
            throw new ArgumentNullException(nameof(errors));
        }

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
