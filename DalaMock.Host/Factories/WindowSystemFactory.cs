namespace DalaMock.Host.Factories;

using System.Collections.Generic;

using Autofac;

using DalaMock.Shared.Interfaces;

using Dalamud.Interface.Windowing;

public interface IWindowSystemFactory
{
    IWindowSystem Create(string? imNamespace = null);
}

public class WindowSystemFactory : IWindowSystemFactory
{
    private readonly IComponentContext context;
    private readonly Dictionary<string?, IWindowSystem> cache = new();

    public WindowSystemFactory(IComponentContext context)
    {
        this.context = context;
    }

    public IWindowSystem Create(string? imNamespace = null)
    {
        imNamespace ??= string.Empty;
        if (this.cache.TryGetValue(imNamespace, out var existingInstance))
        {
            return existingInstance;
        }

        var newInstance = this.context.Resolve<IWindowSystem>(new NamedParameter("imNamespace", imNamespace));
        this.cache[imNamespace] = newInstance;

        return newInstance;
    }
}
