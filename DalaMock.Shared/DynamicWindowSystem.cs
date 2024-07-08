// <copyright file="DynamicWindowSystem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DalaMock.Shared;

using System;
using Dalamud.Interface.Windowing;

public class DynamicWindowSystem : WindowSystem
{
    public DynamicWindowSystem(string? imNamespace = null) : base(imNamespace)
    {
    }

    public new void Draw()
    {
#if DEBUG

        // This is to catch an error that occurs due to the font system not being initialized
        foreach (var window in this.Windows)
        {
            window.AllowClickthrough = false;
            window.AllowPinning = false;
        }

        try
        {
            base.Draw();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
#else
        base.Draw();
#endif
    }
}