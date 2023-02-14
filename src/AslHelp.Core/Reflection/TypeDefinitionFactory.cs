using AslHelp.Core.Exceptions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace AslHelp.Core.Reflection;

public static class TypeDefinitionFactory
{
    private static readonly CSharpCodeProvider _codeProvider = new();

    public static ITypeDefinition CreateFromSource(string source, params string[] references)
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

        return (ITypeDefinition)Activator.CreateInstance(typeof(TypeDefinition<>).MakeGenericType(target));
    }

    public static void Dispose()
    {
        _codeProvider.Dispose();
    }
}
