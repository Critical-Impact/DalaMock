namespace DalaMock.Host.Hosting;

using DalaMock.Host.Mediator;

public record PluginStartedMessage() : MessageBase;

public record PluginStoppingMessage() : MessageBase();
