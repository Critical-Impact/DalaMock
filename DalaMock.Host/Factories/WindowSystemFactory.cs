// <copyright file="WindowSystemFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DalaMock.Host.Factories;

using System.Collections.Generic;
using Autofac;
using Dalamud.Interface.Windowing;

public interface IWindowSystemFactory
{
    WindowSystem Create(string? imNamespace = null);
}

public class WindowSystemFactory : IWindowSystemFactory
{
    private readonly IComponentContext _context;
    private readonly Dictionary<string?, WindowSystem> _cache = new();

    public WindowSystemFactory(IComponentContext context)
    {
        _context = context;
    }

    public WindowSystem Create(string? imNamespace = null)
    {
        imNamespace ??= string.Empty;
        if (this._cache.TryGetValue(imNamespace, out var existingInstance))
        {
            return existingInstance;
        }

        var newInstance = this._context.Resolve<WindowSystem>(new NamedParameter("imNamespace", imNamespace));
        this._cache[imNamespace] = newInstance;

        return newInstance;
    }
}