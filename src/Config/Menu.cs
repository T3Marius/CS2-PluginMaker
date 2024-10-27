using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API;

namespace PluginMaker
{
    public class Menu_Config
    {
        public static MenuConfig Load()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "";
            string configPath = $"{Server.GameDirectory}/csgo/addons/counterstrikesharp/configs/plugins/{assemblyName}/menus.json";

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found: {configPath}");
            }

            string jsonContent = File.ReadAllText(configPath);
            MenuConfig config = JsonSerializer.Deserialize<MenuConfig>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            }) ?? new MenuConfig();

            return config;
        }
    }

    public class MenuConfig
    {
        [JsonPropertyName("Menus")]
        public Dictionary<string, Menus> Menus { get; set; } = new();
    }

    public class Menus
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Command { get; set; } = "";
        public bool FreezeMenu { get; set; } = true;
        public List<string> Permissions { get; set; } = new();
        public List<Options> Options { get; set; } = new();
    }

    public class Options
    {
        public string Name { get; set; } = "";
        public string ServerCommand { get; set; } = "";
        public string ClientCommandFromServer { get; set; } = "";
        public string ClientCommand { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
