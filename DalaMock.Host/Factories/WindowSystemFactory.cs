// <copyright file="WindowSystemFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    private readonly IComponentContext _context;
    private readonly Dictionary<string?, IWindowSystem> _cache = new();

    public WindowSystemFactory(IComponentContext context)
    {
        _context = context;
    }

    public IWindowSystem Create(string? imNamespace = null)
    {
        imNamespace ??= string.Empty;
        if (this._cache.TryGetValue(imNamespace, out var existingInstance))
        {
            return existingInstance;
        }

        var newInstance = this._context.Resolve<IWindowSystem>(new NamedParameter("imNamespace", imNamespace));
        this._cache[imNamespace] = newInstance;

        return newInstance;
    }
}