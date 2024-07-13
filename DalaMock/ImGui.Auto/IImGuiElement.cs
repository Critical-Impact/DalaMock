using System;
using System.Reflection;

namespace DalaMock.Core.Imgui.Auto;

public interface IImGuiElement
{
    String Name { get; set; }

    String Id { get; set; }

    MethodInfo? GetMethodInfo { get; set; }

    MethodInfo? SetMethodInfo { get; set; }

    String Group { get; set; }

    void Draw(object? obj);

    Type GetBackingType();
}