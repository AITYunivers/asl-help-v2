using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    public interface IReject
    {
        bool Reject(params uint[] moduleMemorySizes);
        bool Reject(string moduleName, params uint[] moduleMemorySizes);
        bool Reject(Module module, params uint[] moduleMemorySizes);
    }
}
