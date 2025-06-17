# Hosted Plugin Template

This is an opinionated template for the [.NET Template Engine](https://github.com/dotnet/templating) to make Dalamud plugins.
By default, it provides:

- A fully HostBuilder driven plugin provided by DalaMock
- Autofac injection
- A mediator service for message passing in your UI
- Command, window, installer and configuration services
- A plugin project
- A mock plugin project(which can boot your plugin without the game running)
