using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Mono.Memory.MonoInterop;

public partial class MonoManager
{
    // FIXME: Wait, none of this even works because of class name overlaps between images...

    private readonly Dictionary<string, nuint> _classAddressCache = new();
    private readonly Dictionary<string, MonoClass> _classCache = new();

    public MonoClass FindClass(nuint image, string className)
    {
        if (!TryFindClass(image, className, out MonoClass? monoClass))
        {
            string msg = $"The given class '{className}' was not found in the image.";
            ThrowHelper.ThrowKeyNotFoundException(msg);
        }

        return monoClass;
    }

    public bool TryFindClass(nuint image, string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        if (_classCache.TryGetValue(className, out MonoClass? cachedClass))
        {
            monoClass = cachedClass;
            return true;
        }

        if (_classAddressCache.TryGetValue(className, out nuint cachedAddress))
        {
            monoClass = new(
                cachedAddress,
                className,
                GetClassNamespace(cachedAddress));

            _classCache[className] = monoClass;
            return true;
        }

        foreach (nuint klass in EnumerateClasses(image))
        {
            string klassName = GetClassName(klass);
            if (klassName == className)
            {
                monoClass = new(
                    klass,
                    klassName,
                    GetClassNamespace(klass));

                _classCache[klassName] = monoClass;
                return true;
            }

            _classAddressCache[klassName] = klass;
        }

        monoClass = null;
        return false;
    }

    private readonly Dictionary<string, nuint> _classNsAddressCache = new();
    private readonly Dictionary<string, MonoClass> _classNsCache = new();

    public MonoClass FindClass(nuint image, string @namespace, string className)
    {
        if (!TryFindClass(image, @namespace, className, out MonoClass? monoClass))
        {
            string msg = $"The given class '{@namespace}.{className}' was not found in the image.";
            ThrowHelper.ThrowKeyNotFoundException(msg);
        }

        return monoClass;
    }

    public bool TryFindClass(nuint image, string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        string fullName = $"{@namespace}.{className}";
        if (_classNsCache.TryGetValue(fullName, out MonoClass? cachedClass))
        {
            monoClass = cachedClass;
            return true;
        }

        if (_classNsAddressCache.TryGetValue(fullName, out nuint cachedAddress))
        {
            monoClass = new MonoClass(
                cachedAddress,
                className,
                @namespace);

            _classNsCache[fullName] = monoClass;
            return true;
        }

        foreach (nuint klass in EnumerateClasses(image))
        {
            string klassName = GetClassName(klass);
            string klassNamespace = GetClassNamespace(klass);
            string fullKlassName = $"{klassNamespace}.{klassName}";

            if (fullKlassName == fullName)
            {
                monoClass = new MonoClass(
                    klass,
                    klassName,
                    klassNamespace);

                _classNsCache[fullKlassName] = monoClass;
                return true;
            }

            _classNsAddressCache[fullKlassName] = klass;
        }

        monoClass = null;
        return false;
    }

    protected abstract IEnumerable<nuint> EnumerateClasses(nuint image);

    protected abstract string GetClassName(nuint klass);
    protected abstract string GetClassNamespace(nuint klass);
}
