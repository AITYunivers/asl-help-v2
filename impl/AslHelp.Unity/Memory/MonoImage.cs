using AslHelp.Common.Exceptions;
using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Unity.Memory.MonoInterop;
using AslHelp.Unity.Memory.MonoInterop.Management;

using OneOf;

namespace AslHelp.Unity.Memory;

public class MonoImage(
    nuint address,
    MonoManager mono)
{
    private readonly MonoManager _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetImageName(Address);

    private string? _fileName;
    public string FileName => _fileName ??= _mono.GetImageFileName(Address);

    public LazyDictionary<string, MonoClass> Classes { get; } = new MonoClassCache(address, mono);

    public Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next)
        where T : unmanaged
    {
        MonoClass klass = Classes[className];

        MonoClass nextKlass;
        int staticFieldOffset;

        while (klass.Address > 0)
        {
            if (klass.Fields.TryGetValue(staticFieldName, out MonoField? staticField)
                && staticField.Type.Attributes.HasFlag(MonoFieldAttribute.STATIC))
            {
                staticFieldOffset = staticField.Offset;
                nextKlass = staticField.Type.Class;

                goto StaticFieldFound;
            }

            klass = klass.Parent;
        }

        string msg1 = $"Could not find static field '{staticFieldName}' in class '{className}' or any of its parents.";
        ThrowHelper.ThrowArgumentException(nameof(staticFieldName), msg1);

        return null;

    StaticFieldFound:
        nuint staticDataChunk = _mono.GetClassStaticDataChunk(klass.Address);
        nuint staticFieldAddress = staticDataChunk + (nuint)staticFieldOffset;

        int[] offsets = new int[next.Length];
        for (int i = 0; i < next.Length; i++)
        {
            if (next[i] is { Value: int offset })
            {
                offsets[i] = offset;
                continue;
            }

            if (next[i] is { Value: string name })
            {
                int dotIndex = name.IndexOf('.');
                if (dotIndex != -1)
                {
                    nextKlass = Classes[name[..dotIndex]];
                    name = name[(dotIndex + 1)..];
                }

                do
                {
                    if (nextKlass.Fields.TryGetValue(name, out MonoField? field))
                    {
                        offsets[i] = field.Offset;
                        nextKlass = field.Type.Class;

                        goto FieldFound;
                    }

                    nextKlass = nextKlass.Parent;
                } while (nextKlass.Address > 0);

                string msg2 = $"Could not find field '{name}' in class '{nextKlass.Name}' or any of its parents.";
                ThrowHelper.ThrowArgumentException(nameof(next), msg2);

            FieldFound:
                ;
            }
        }

        return new Pointer<T>(_mono._memory, staticFieldAddress, offsets);
    }

    public bool TryMake<T>(out Pointer<T>? pointer, string className, string staticFieldName, params OneOf<string, int>[] next)
        where T : unmanaged
    {
        try
        {
            pointer = Make<T>(className, staticFieldName, next);
            return true;
        }
        catch
        {
            pointer = null;
            return false;
        }
    }
}
