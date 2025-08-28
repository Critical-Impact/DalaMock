namespace DalaMock.Core.Imgui.Auto;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class ImGuiGroupAttribute : Attribute
{
    public ImGuiGroupAttribute(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}
