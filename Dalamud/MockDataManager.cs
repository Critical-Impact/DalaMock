using Dalamud;
using Dalamud.Plugin.Services;
using Lumina;
using Lumina.Data;
using Lumina.Excel;

namespace DalaMock.Dalamud;

public class MockDataManager : IDataManager
{
    private readonly GameData _gameData;
    private readonly ClientLanguage _mockLanguage;

    public MockDataManager(GameData gameData, ClientLanguage mockLanguage)
    {
        _gameData = gameData;
        _mockLanguage = mockLanguage;
    }
    public ExcelSheet<T>? GetExcelSheet<T>() where T : ExcelRow
    {
        return _gameData.GetExcelSheet<T>();
    }

    public ExcelSheet<T>? GetExcelSheet<T>(ClientLanguage language) where T : ExcelRow
    {
        return _gameData.GetExcelSheet<T>(language.ToLumina());
    }

    public FileResource? GetFile(string path)
    {
        return _gameData.GetFile(path);
    }

    public T? GetFile<T>(string path) where T : FileResource
    {
        return _gameData.GetFile<T>(path);
    }

    public bool FileExists(string path)
    {
        return _gameData.FileExists(path);
    }

    public ClientLanguage Language => _mockLanguage;
    public GameData GameData => _gameData;
    public ExcelModule Excel => _gameData.Excel;
    public bool HasModifiedGameDataFiles => false;
}