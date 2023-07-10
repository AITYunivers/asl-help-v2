using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
#pragma warning disable IDE1006
    public interface Versioning
#pragma warning restore IDE1006
    {
        uint GetMemorySize();
        uint GetMemorySize(string moduleName);
        uint GetMemorySize(Module module);

        string GetMD5Hash();
        string GetMD5Hash(string moduleName);
        string GetMD5Hash(Module module);

        string GetSHA1Hash();
        string GetSHA1Hash(string moduleName);
        string GetSHA1Hash(Module module);

        string GetSHA256Hash();
        string GetSHA256Hash(string moduleName);
        string GetSHA256Hash(Module module);

        string GetSHA512Hash();
        string GetSHA512Hash(string moduleName);
        string GetSHA512Hash(Module module);
    }
}
