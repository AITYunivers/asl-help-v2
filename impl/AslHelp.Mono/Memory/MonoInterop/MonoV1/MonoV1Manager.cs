using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Mono.Memory.MonoInterop.MonoV1;

internal class MonoV1Manager : MonoManager
{
    private protected override IMonoInitializer Initializer { get; } = new MonoV1Initializer();

    public override MonoClass FindClass(string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass FindClass(string @namespace, string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoImage FindImage(string imageName)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass GetParentClass(MonoClass monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent)
    {
        throw new System.NotImplementedException();
    }
}
