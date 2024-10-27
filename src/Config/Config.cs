using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using System.Reflection;
using Tomlyn;
using Tomlyn.Model;

namespace PluginMaker;

public static class Main_Config
{
    public static Cfg MainConfig { get; set; } = new Cfg();

    public static void Load()
    {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "";
        string cfgPath = $"{Server.GameDirectory}/csgo/addons/counterstrikesharp/configs/plugins/{assemblyName}";

        LoadConfig($"{cfgPath}/mainconfig.toml");
    }
    private static void LoadConfig(string configPath)
    {
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file not found: {configPath}");
        }

        string configText = File.ReadAllText(configPath);
        TomlTable model = Toml.ToModel(configText);

        TomlTable tagTable = (TomlTable)model["Tag"];
        string config_tag = StringExtensions.ReplaceColorTags(tagTable["Tag"].ToString()!);

        TomlTable settingsTable = (TomlTable)model["Settings"];
        Config_Settings config_settings = new()
        {
            EnableAnnouncements = bool.Parse(settingsTable["EnableAnnouncements"].ToString()!),
            EnableCommands = bool.Parse(settingsTable["EnableCommands"].ToString()!),
            EnableMenu = bool.Parse(settingsTable["EnableMenu"].ToString()!),
        };

        MainConfig = new Cfg
        {
            Tag = config_tag,
            Settings = config_settings,
        };
    }

    private static string[] GetTomlArray(TomlTable table, string key)
    {
        if (table.TryGetValue(key, out var value) && value is TomlArray array)
        {
            return array.OfType<string>().ToArray();
        }
        return Array.Empty<string>();
    }



    public class Cfg
    {
        public string Tag { get; set; } = string.Empty;
        public Config_Settings Settings { get; set; } = new();
    }
    public class Config_Settings
    {
        public bool EnableAnnouncements { get; set; }
        public bool EnableCommands { get; set; }
        public bool EnableMenu { get; set; }
    }
}