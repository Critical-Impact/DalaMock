using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Dalamud;
using Dalamud.Game.Command;
using Dalamud.Plugin.Services;

namespace DalaMock.Dalamud;

public class MockCommandManager : ICommandManager
{
    private readonly Regex commandRegexEn = new Regex("^The command (?<command>.+) does not exist\\.$", RegexOptions.Compiled);
    private readonly Regex commandRegexJp = new Regex("^そのコマンドはありません。： (?<command>.+)$", RegexOptions.Compiled);
    private readonly Regex commandRegexDe = new Regex("^„(?<command>.+)“ existiert nicht als Textkommando\\.$", RegexOptions.Compiled);
    private readonly Regex commandRegexFr = new Regex("^La commande texte “(?<command>.+)” n'existe pas\\.$", RegexOptions.Compiled);
    private readonly Regex commandRegexCn = new Regex("^^(“|「)(?<command>.+)(”|」)(出现问题：该命令不存在|出現問題：該命令不存在)。$", RegexOptions.Compiled);
    private readonly ConcurrentDictionary<string, CommandInfo> commandMap = new ConcurrentDictionary<string, CommandInfo>();

    private readonly IPluginLog _logger;
    private readonly ClientLanguage _clientLanguage;
    private readonly Regex currentLangCommandRegex;

    public MockCommandManager(IPluginLog logger, ClientLanguage clientLanguage)
    {
        _logger = logger;
        _clientLanguage = clientLanguage;
        Regex regex;
        switch (_clientLanguage)
        {
            case ClientLanguage.Japanese:
                regex = this.commandRegexJp;
                break;
            case ClientLanguage.English:
                regex = this.commandRegexEn;
                break;
            case ClientLanguage.German:
                regex = this.commandRegexDe;
                break;
            case ClientLanguage.French:
                regex = this.commandRegexFr;
                break;
            default:
                throw new Exception("Not a supported language.");
                break;
        }
        this.currentLangCommandRegex = regex;
    }
    public void DispatchCommand(string command, string argument, CommandInfo info)
    {
        try
        {
            info.Handler(command, argument);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while dispatching command {CommandName} (Argument: {Argument})", (object) command, (object) argument);
        }
    }

    public bool AddHandler(string command, CommandInfo info)
    {
        if (info == null)
            throw new ArgumentNullException(nameof (info), "Command handler is null.");
        if (this.commandMap.TryAdd(command, info))
            return true;
        _logger.Error("Command {CommandName} is already registered.", (object) command);
        return false;
    }

    public bool RemoveHandler(string command) => this.commandMap.Remove<string, CommandInfo>(command, out CommandInfo _);


    public ReadOnlyDictionary<string, CommandInfo> Commands => new ReadOnlyDictionary<string, CommandInfo>((IDictionary<string, CommandInfo>) this.commandMap);

    public bool ProcessCommand(string content)
    {
        int length = content.IndexOf(' ');
        string str1;
        string str2;
        if (length == -1 || length + 1 >= content.Length)
        {
            str1 = length + 1 < content.Length ? content : content.Substring(0, length);
            str2 = string.Empty;
        }
        else
        {
            str1 = content.Substring(0, length);
            int num = length + 1;
            string str3 = content;
            int startIndex = num;
            str2 = str3.Substring(startIndex, str3.Length - startIndex);
        }
        CommandInfo info;
        if (!this.commandMap.TryGetValue(str1, out info))
            return false;
        this.DispatchCommand(str1, str2, info);
        return true;
    }
}