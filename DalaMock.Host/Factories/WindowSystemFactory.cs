namespace DalaMock.Host.Factories;

using System.Collections.Generic;

using Autofac;

using DalaMock.Shared.Interfaces;

using Dalamud.Interface.Windowing;

/// <inheritdoc />
public class WindowSystemFactory : IWindowSystemFactory
{
    private readonly Dictionary<string?, IWindowSystem> cache = new();

    public WindowSystemFactory()
    {
    }

    public IWindowSystem Create(string? imNamespace = null)
    {
        imNamespace ??= string.Empty;
        if (this.cache.TryGetValue(imNamespace, out var existingInstance))
        {
            return existingInstance;
        }

        var newInstance = new WindowSystem(imNamespace);
        this.cache[imNamespace] = newInstance;

        return newInstance;
    }
}
