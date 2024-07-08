namespace DalaMock.Core.Mocks;

using System;
using Dalamud.Game;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Lumina;
using Lumina.Data;
using Lumina.Excel;

public class MockDataManager : IDataManager, IMockService
{
    private readonly GameData gameData;
    private readonly ClientLanguage mockLanguage;

    public MockDataManager(GameData gameData, MockDalamudConfiguration configuration)
    {
        this.gameData = gameData;
        this.mockLanguage = configuration.ClientLanguage;
    }

    public Type BackingType { get; }

    public ExcelSheet<T>? GetExcelSheet<T>() where T : ExcelRow
    {
        return this.gameData.GetExcelSheet<T>();
    }

    public ExcelSheet<T>? GetExcelSheet<T>(ClientLanguage language) where T : ExcelRow
    {
        return this.gameData.GetExcelSheet<T>(ClientLanguageExtensions.ToLumina(language));
    }

    public FileResource? GetFile(string path)
    {
        return this.gameData.GetFile(path);
    }

    public T? GetFile<T>(string path) where T : FileResource
    {
        return this.gameData.GetFile<T>(path);
    }

    public bool FileExists(string path)
    {
        return this.gameData.FileExists(path);
    }

    public ClientLanguage Language => this.mockLanguage;

    public GameData GameData => this.gameData;

    public ExcelModule Excel => this.gameData.Excel;

    public bool HasModifiedGameDataFiles => false;

    public string ServiceName => "Data Manager";
}