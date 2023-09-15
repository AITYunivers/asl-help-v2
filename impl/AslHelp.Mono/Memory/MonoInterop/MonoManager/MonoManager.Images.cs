using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Mono.Memory.MonoInterop;

public partial class MonoManager
{
    private readonly Dictionary<string, nuint> _imageAddressCache = new();
    private readonly Dictionary<string, MonoImage> _imageCache = new();

    public MonoImage FindImage(string imageName)
    {
        if (!TryFindImage(imageName, out MonoImage? monoImage))
        {
            string msg = $"The given image '{imageName}' was not found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return monoImage;
    }

    // TODO: Can we make this prettier?
    public bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage)
    {
        if (_imageCache.TryGetValue(imageName, out MonoImage cachedImage))
        {
            monoImage = cachedImage;
            return true;
        }

        if (_imageAddressCache.TryGetValue(imageName, out nuint cachedImageAddress))
        {
            monoImage = new(
                this,
                cachedImageAddress,
                imageName,
                GetImagePath(cachedImageAddress));

            _imageCache[imageName] = monoImage;
            return true;
        }

        foreach (nuint image in EnumerateImages())
        {
            string name = GetImageName(image);
            if (name == imageName)
            {
                monoImage = new(
                    this,
                    image,
                    name,
                    GetImagePath(image));

                _imageCache[name] = monoImage;
                return true;
            }

            _imageAddressCache[name] = image;
        }

        monoImage = null;
        return false;
    }

    protected abstract IEnumerable<nuint> EnumerateImages();

    protected abstract string GetImageName(nuint image);
    protected abstract string GetImagePath(nuint image);
}
