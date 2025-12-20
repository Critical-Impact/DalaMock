namespace DalaMock.Core.Mocks;

using System;

using Dalamud.Plugin.VersionInfo;

public class MockDalamudVersionInfo : IDalamudVersionInfo
{
    public MockDalamudVersionInfo(Version version)
    {
        this.Version = version;
    }

    public Version Version { get; set; }

    public string? BetaTrack { get; set; }

    public string? GitHash { get; set; }

    public string? GitHashClientStructs { get; set; }

    public string? ScmVersion { get; set; }
}
