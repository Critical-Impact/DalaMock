using Dalamud.IoC;
using Dalamud.Plugin;

public class DalamudServiceWrapper<T>
{
    [PluginService]
    public T Service { get; private set; } = default!;

    public DalamudServiceWrapper(IDalamudPluginInterface pi)
    {
        pi.Inject(this);
    }
}
