using System.Collections.Generic;
using System.Diagnostics;

using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Unity;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop;

public partial class Unity
{
    private MonoImageCache? _images;
    public MonoImageCache? Images
    {
        get
        {
            if (_images is not null)
            {
                return _images;
            }

            if (Mono is not null)
            {
                _images = new(Mono);
            }

            return _images;
        }
    }

    private IMonoManager? _mono;
    public IMonoManager? Mono
    {
        get
        {
            if (_mono is not null)
            {
                return _mono;
            }

            if (Memory is IMonoMemoryManager memory)
            {
                Debug.Info("Initializing Mono...");

                _mono = InitializeMono(memory);

                Debug.Info("  => Done.");
            }

            return _mono;
        }
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new MonoExternalMemoryManager(process, Logger);
    }

    private static IMonoManager InitializeMono(IMonoMemoryManager memory)
    {
        if (memory.MonoModule.Name == "mono.dll")
        {
            return new MonoV1Manager(memory);
        }
        else if (memory.MonoModule.Name == "mono-2.0-bdwgc.dll")
        {
            return new MonoV2Manager(memory);
        }
        else
        {
            throw new System.NotImplementedException($"Mono version {memory.MonoModule.Name} is not supported.");
        }
    }
}

public class MonoImageCache : LazyDictionary<string, MonoImage>
{
    private readonly IMonoManager _mono;

    public MonoImageCache(IMonoManager mono)
    {
        _mono = mono;
    }

    public override IEnumerator<MonoImage> GetEnumerator()
    {
        foreach (nuint image in _mono.GetImages())
        {
            // Debug.Warn($"{(ulong)image:X}");
            yield return new(image, _mono);
        }
    }

    protected override string GetKey(MonoImage value)
    {
        return $"{value.Name}";
    }
}
