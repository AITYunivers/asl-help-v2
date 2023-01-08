using System.CodeDom.Compiler;
using AslHelp.Core.Exceptions;
using Microsoft.CSharp;

namespace AslHelp.Core.Reflection;

internal static class TypeDefinitionFactory
{
    private readonly static CSharpCodeProvider _codeProvider = new();

    public static TypeDefinition CreateFromSource(string source, params string[] references)
    {
        CompilerParameters parameters = new()
        {
            GenerateInMemory = true,
            CompilerOptions = "/optimize"
        };

        parameters.ReferencedAssemblies.AddRange(references);

        CompilerResults asm = _codeProvider.CompileAssemblyFromSource(parameters, source);

        if (asm.Errors.HasErrors)
        {
            TypeDefinitionCompilerException.Throw(asm.Errors);
        }

        Type[] types = asm.CompiledAssembly.GetTypes();

        if (types.Length == 0)
        {
            TypeDefinitionCompilerException.ThrowIfNoType();
        }

        if (types.Length > 1)
        {
            TypeDefinitionCompilerException.ThrowIfMoreThanOneType();
        }

        Type target = types[0];

        if (!target.IsValueType)
        {
            TypeDefinitionCompilerException.ThrowIfNotValueType();
        }

        return new(target);
    }

    public static void Dispose()
    {
        _codeProvider.Dispose();
    }
}
