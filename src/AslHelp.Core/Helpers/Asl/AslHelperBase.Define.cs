using AslHelp.Core.Reflection;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public ITypeDefinition Define(string code, params string[] references)
    {
        return TypeDefinitionFactory.CreateFromSource(code, references);
    }
}
