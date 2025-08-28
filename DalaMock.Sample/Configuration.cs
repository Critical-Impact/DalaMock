namespace DalaMock.Sample;

using Dalamud.Configuration;

using Newtonsoft.Json;

public class Configuration : IPluginConfiguration
{
    private bool configOption;

    public int Version { get; set; }

    [JsonIgnore]
    public bool IsDirty { get; set; }

    public bool ConfigOption
    {
        get => this.configOption;
        set
        {
            this.configOption = value;
            this.IsDirty = true;
        }
    }
}
