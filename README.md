# DalaMock
Very shoddy mocking class library for Dalamud.  
At present the library can only handle a single plugin at a time and only mocks dalamud's services(ChatGui, Framework, etc).   
In the long term there are plans to provider wrapper classes for ClientStructs. 

How to use:  

- Create a separate console project
- Create a class that implements IMockPlugin
- Add the interface IServiceContainer to your service container
- In your Program.cs file add code that looks like this

```csharp
static void Main(string[] args)
{
    var yourServiceContainer = new Service();
    _program = new MockProgram(yourServiceContainer);
    _mockPlugin = new MockPlugin();
    _program.SetPlugin(_mockPlugin);
    _mockSettingsWindow = new MockSettingsWindow(_program);
    

    if (AppSettings.Default.AutoStart)
    {
        _program.StartPlugin();
    }

    while (_program.PumpEvents(PreUpdate, PostUpdate))
    {
        
    }

    if (MockPlugin != null)
    {
        MockPlugin.Dispose();
        ConfigurationManager.Save();
    }

    _program.Dispose();
}

private static void PostUpdate()
{
    MockPlugin?.Draw();
    _mockSettingsWindow?.Draw();
}

private static void PreUpdate()
{
    MockPlugin?.MockFrameworkService?.FireUpdate();
}
```

- Assuming everything goes well, you should start with a settings window where you can configure your game path, settings path and internal plugin name. Once the plugin is loaded, your service container will have mocked versions of dalamud's services injected.