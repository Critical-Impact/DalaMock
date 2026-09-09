using System;

using Dalamud;
using Dalamud.Game.Text;

namespace DalaMock.Sample.Windows;

using DalaMock.Shared.Interfaces;

using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;

public class MainWindow : Window
{
    private readonly IFont font1;
    private readonly Version? version;

    public MainWindow(IFont font)
        : base("SamplePlugin")
    {
        this.font1 = font;
        this.version = typeof(IServiceType).Assembly.GetName().Version;
    }

    public override void Draw()
    {
        ImGui.TextUnformatted("Hello, world!");

        if (this.version != null)
        {
            ImGui.Text($"Mocking Dalamud Version {this.version}");
        }

        ImGui.Text("A sample window");
        ImGui.Text("Icon Font");
        using (ImRaii.PushFont(this.font1.IconFont))
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }
        ImGui.Text("Fixed Width Icon Font");
        using (ImRaii.PushFont(this.font1.IconFixedWidth))
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }
        using (ImRaii.PushFont(this.font1.DefaultFont))
        {
            ImGui.Text(SeIconChar.BoxedLetterD.ToIconString() + SeIconChar.BoxedLetterA.ToIconString() + SeIconChar.BoxedLetterL.ToIconString() + SeIconChar.BoxedLetterA.ToIconString() + SeIconChar.BoxedLetterM.ToIconString() + SeIconChar.BoxedLetterO.ToIconString() + SeIconChar.BoxedLetterC.ToIconString() + SeIconChar.BoxedLetterK.ToIconString());
        }
    }
}
