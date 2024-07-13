// <copyright file="ImGuiGroupAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace DalaMock.Core.Imgui.Auto;

[AttributeUsage(AttributeTargets.Property)]
public class ImGuiGroupAttribute : Attribute
{
    public ImGuiGroupAttribute(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}