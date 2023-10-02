using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper.Versioning
{
    public uint GetMemorySize()
    {
        EnsureInInitForVersioning();

        return GetMemorySize(Memory.MainModule);
    }

    public uint GetMemorySize(string moduleName)
    {
        EnsureInInitForVersioning();

        return GetMemorySize(Memory.Modules[moduleName]);
    }

    public uint GetMemorySize(Module module)
    {
        EnsureInInitForVersioning();

        return module.MemorySize;
    }

    public string GetMD5Hash()
    {
        EnsureInInitForVersioning();

        return GetMD5Hash(Memory.MainModule);
    }

    public string GetMD5Hash(string moduleName)
    {
        EnsureInInitForVersioning();

        return GetMD5Hash(Memory.Modules[moduleName]);
    }

    public string GetMD5Hash(Module module)
    {
        using MD5 md5 = MD5.Create();
        return GetHash(module, md5);
    }

    public string GetSHA1Hash()
    {
        EnsureInInitForVersioning();

        return GetSHA1Hash(Memory.MainModule);
    }

    public string GetSHA1Hash(string moduleName)
    {
        EnsureInInitForVersioning();

        return GetSHA1Hash(Memory.Modules[moduleName]);
    }

    public string GetSHA1Hash(Module module)
    {
        using SHA1 sha1 = SHA1.Create();
        return GetHash(module, sha1);
    }

    public string GetSHA256Hash()
    {
        EnsureInInitForVersioning();

        return GetSHA256Hash(Memory.MainModule);
    }

    public string GetSHA256Hash(string moduleName)
    {
        EnsureInInitForVersioning();

        return GetSHA256Hash(Memory.Modules[moduleName]);
    }

    public string GetSHA256Hash(Module module)
    {
        using SHA256 sha256 = SHA256.Create();
        return GetHash(module, sha256);
    }

    public string GetSHA512Hash()
    {
        EnsureInInitForVersioning();

        return GetSHA512Hash(Memory.MainModule);
    }

    public string GetSHA512Hash(string moduleName)
    {
        EnsureInInitForVersioning();

        return GetSHA512Hash(Memory.Modules[moduleName]);
    }

    public string GetSHA512Hash(Module module)
    {
        using SHA512 sha512 = SHA512.Create();
        return GetHash(module, sha512);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetHash(Module module, HashAlgorithm algorithm)
    {
        using FileStream reader = File.OpenRead(module.FileName);
        return string.Concat(algorithm.ComputeHash(reader).Select(b => $"{b:X2}"));
    }

    [MemberNotNull(nameof(Memory))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureInInitForVersioning()
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = $"Attempted to get versioning information in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (Memory is null)
        {
            string msg = "Attempted to access uninitialized memory.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }
    }
}
