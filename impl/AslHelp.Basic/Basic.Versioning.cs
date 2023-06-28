using System.IO;
using System.Linq;
using System.Security.Cryptography;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

public partial class Basic
{
    public uint GetMemorySize()
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetMemorySize] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetMemorySize] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMemorySize(module);
    }

    public uint GetMemorySize(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetMemorySize] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetMemorySize] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMemorySize(module);
    }

    public uint GetMemorySize(Module module)
    {
        return module.MemorySize;
    }

    public string GetMD5Hash()
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetMD5Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetMD5Hash] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetMD5Hash(module);
    }

    public string GetMD5Hash(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetMD5Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetMD5Hash] Module '{moduleName}' could not be found.";
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
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA1Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetSHA1Hash] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA1Hash(module);
    }

    public string GetSHA1Hash(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA1Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetSHA1Hash] Module '{moduleName}' could not be found.";
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
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA256Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetSHA256Hash] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA256Hash(module);
    }

    public string GetSHA256Hash(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA256Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetSHA256Hash] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA256Hash(module);
    }

    public string GetSHA256Hash(Module module)
    {
        using SHA256 sha256 = SHA256.Create();
        return GetHash(module, sha256);
    }

    public string GetSHA384Hash()
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA384Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetSHA384Hash] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA384Hash(module);
    }

    public string GetSHA384Hash(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA384Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetSHA384Hash] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA384Hash(module);
    }

    public string GetSHA384Hash(Module module)
    {
        using SHA384 sha384 = SHA384.Create();
        return GetHash(module, sha384);
    }

    public string GetSHA512Hash()
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA512Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[GetSHA512Hash] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA512Hash(module);
    }

    public string GetSHA512Hash(string moduleName)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[GetSHA512Hash] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[GetSHA512Hash] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return GetSHA512Hash(module);
    }

    public string GetSHA512Hash(Module module)
    {
        using SHA512 sha512 = SHA512.Create();
        return GetHash(module, sha512);
    }

    private string GetHash(Module module, HashAlgorithm algorithm)
    {
        using FileStream reader = File.OpenRead(module.FileName);
        return string.Concat(algorithm.ComputeHash(reader).Select(b => $"{b:X2}"));
    }
}
