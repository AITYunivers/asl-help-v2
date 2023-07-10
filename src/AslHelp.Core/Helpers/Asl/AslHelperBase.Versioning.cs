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
        EnsureInInit();

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMemorySize(module);
    }

    public uint GetMemorySize(string moduleName)
    {
        EnsureInInit();

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMemorySize(module);
    }

    public uint GetMemorySize(Module module)
    {
        EnsureInInit();

        return module.MemorySize;
    }

    public string GetMD5Hash()
    {
        EnsureInInit();

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMD5Hash(module);
    }

    public string GetMD5Hash(string moduleName)
    {
        EnsureInInit();

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMD5Hash(module);
    }

    public string GetMD5Hash(Module module)
    {
        using MD5 md5 = MD5.Create();
        return GetHash(module, md5);
    }

    public string GetSHA1Hash()
    {
        EnsureInInit();

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA1Hash(module);
    }

    public string GetSHA1Hash(string moduleName)
    {
        EnsureInInit();

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA1Hash(module);
    }

    public string GetSHA1Hash(Module module)
    {
        using SHA1 sha1 = SHA1.Create();
        return GetHash(module, sha1);
    }

    public string GetSHA256Hash()
    {
        EnsureInInit();

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA256Hash(module);
    }

    public string GetSHA256Hash(string moduleName)
    {
        EnsureInInit();

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA256Hash(module);
    }

    public string GetSHA256Hash(Module module)
    {
        using SHA256 sha256 = SHA256.Create();
        return GetHash(module, sha256);
    }

    public string GetSHA512Hash()
    {
        EnsureInInit();

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA512Hash(module);
    }

    public string GetSHA512Hash(string moduleName)
    {
        EnsureInInit();

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA512Hash(module);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureInInit([CallerMemberName] string caller = "")
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = $"Attempted to get versioning information in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg, caller);
        }
    }
}
