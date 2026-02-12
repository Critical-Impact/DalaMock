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

    public MainWindow(IFont font)
        : base("SamplePlugin")
    {
        this.font1 = font;
    }

    public override void Draw()
    {
        ImGui.TextUnformatted("Hello, world!");

        ImGui.Text("A sample window");
        using (ImRaii.PushFont(this.font1.IconFont))
        {
            ImGui.Text(FontAwesomeIcon.Times.ToIconString());
        }
        using (ImRaii.PushFont(this.font1.DefaultFont))
        {
            ImGui.Text(SeIconChar.BoxedLetterD.ToIconString() + SeIconChar.BoxedLetterA.ToIconString() + SeIconChar.BoxedLetterL.ToIconString() + SeIconChar.BoxedLetterA.ToIconString() + SeIconChar.BoxedLetterM.ToIconString() + SeIconChar.BoxedLetterO.ToIconString() + SeIconChar.BoxedLetterC.ToIconString() + SeIconChar.BoxedLetterK.ToIconString());
        }
    }
}
