using AslHelp.Core.Reflection;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    public interface IDefine
    {
        ITypeDefinition Define(string code, params string[] references);
    }
}
