using AslHelp.Core.Reflection;

public partial class Basic
{
    public ITypeDefinition Define(string code, params string[] references)
    {
        return TypeDefinitionFactory.CreateFromSource(code, references);
    }
}
