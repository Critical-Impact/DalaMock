using DalaMock.Sample.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DalaMock.Sample.Tests;

/// <summary>
/// NUnit test fixture that verifies the behaviour of <see cref="ConfigurationService"/>.
/// Inherits the full DI host from <see cref="BaseTest"/> so that a real
/// <see cref="ConfigurationService"/> instance (registered by the plugin) is
/// available for each test.
/// </summary>
[TestFixture]
public class ConfigurationTest : BaseTest
{
    private ConfigurationService configurationService = null!;

    /// <summary>
    /// Resolves <see cref="ConfigurationService"/> from the DI container before each test
    /// so that every test method starts with a freshly fetched service reference.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        configurationService = Host.Services.GetRequiredService<ConfigurationService>();
    }

    /// <summary>
    /// Verifies that calling <see cref="ConfigurationService.Save"/> persists the
    /// configuration and clears the <c>IsDirty</c> flag.
    /// </summary>
    /// <remarks>
    /// The test marks the configuration dirty, saves it, then asserts that
    /// <c>IsDirty</c> is reset to <c>false</c> — confirming that the save
    /// operation correctly acknowledges all pending changes.
    /// </remarks>
    [Test]
    public void TestConfiguration()
    {
        var config = configurationService.GetConfiguration();
        config.IsDirty = true;
        configurationService.Save();
        Assert.AreEqual(false, config.IsDirty);
    }
}