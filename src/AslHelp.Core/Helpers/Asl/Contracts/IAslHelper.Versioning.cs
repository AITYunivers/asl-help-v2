using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    /// <summary>
    ///     <see cref="Versioning"/> provides an interface
    ///     to implement ASL versioning methods.
    /// </summary>
#pragma warning disable IDE1006
    public interface Versioning
#pragma warning restore IDE1006
    {
        /// <summary>
        ///     Retrieves the memory size of the game's main module.
        /// </summary>
        uint GetMemorySize();

        /// <summary>
        ///     Retrieves the memory size of the module with the specified name.
        /// </summary>
        uint GetMemorySize(string moduleName);

        /// <summary>
        ///     Retrieves the memory size of the specified <see cref="Module"/>.
        /// </summary>
        uint GetMemorySize(Module module);

        /// <summary>
        ///     Computes the MD5 hash of the game's main module.
        /// </summary>
        string GetMD5Hash();

        /// <summary>
        ///     Computes the MD5 hash of the module with the specified name.
        /// </summary>
        string GetMD5Hash(string moduleName);

        /// <summary>
        ///     Computes the MD5 hash of the specified <see cref="Module"/>.
        /// </summary>
        string GetMD5Hash(Module module);

        /// <summary>
        ///     Computes the SHA1 hash of the game's main module.
        /// </summary>
        string GetSHA1Hash();

        /// <summary>
        ///     Computes the SHA1 hash of the module with the specified name.
        /// </summary>
        string GetSHA1Hash(string moduleName);

        /// <summary>
        ///     Computes the SHA1 hash of the specified <see cref="Module"/>.
        /// </summary>
        string GetSHA1Hash(Module module);

        /// <summary>
        ///     Computes the SHA256 hash of the game's main module.
        /// </summary>
        string GetSHA256Hash();

        /// <summary>
        ///     Computes the SHA256 hash of the module with the specified name.
        /// </summary>
        string GetSHA256Hash(string moduleName);

        /// <summary>
        ///     Computes the SHA256 hash of the specified <see cref="Module"/>.
        /// </summary>
        string GetSHA256Hash(Module module);

        /// <summary>
        ///     Computes the SHA512 hash of the game's main module.
        /// </summary>
        string GetSHA512Hash();

        /// <summary>
        ///     Computes the SHA512 hash of the module with the specified name.
        /// </summary>
        string GetSHA512Hash(string moduleName);

        /// <summary>
        ///     Computes the SHA512 hash of the specified <see cref="Module"/>.
        /// </summary>
        string GetSHA512Hash(Module module);
    }
}
