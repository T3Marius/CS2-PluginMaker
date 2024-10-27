using Tomlyn;
using Tomlyn.Model;
using System.Reflection;
using CounterStrikeSharp.API;
using static PluginMaker.PluginMaker;
using Microsoft.Extensions.Logging;

namespace PluginMaker
{
    public static class CommandsConfigLoader
    {
        public static List<Commands> CommandsList { get; set; } = new List<Commands>();
        public static bool EnabledCommands { get; set; }

        public static void Load()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "";
            string cfgPath = $"{Server.GameDirectory}/csgo/addons/counterstrikesharp/configs/plugins/{assemblyName}";

            LoadConfig($"{cfgPath}/commands.toml");

        }
        public static void LoadConfig(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found: {configPath}");
            }

            string configText = File.ReadAllText(configPath);
            TomlTable model = Toml.ToModel(configText);

            var commandsTableArray = (TomlTableArray)model["Commands"];

            foreach (TomlTable commandTable in commandsTableArray)
            {
                Commands command = new Commands
                {
                    Name = commandTable["Name"].ToString()!,
                    Description = commandTable["Description"].ToString()!,
                    Message = commandTable["Message"].ToString()!,
                    ClientServerCommand = commandTable["ClientServerCommand"].ToString()!,
                    ClientCommand = commandTable["ClientCommand"].ToString()!,
                    ServerCommand = commandTable["ServerCommand"].ToString()!,
                    Permissions = GetTomlArray(commandTable, "Permissions")
                };

                CommandsList.Add(command);
            }
        }

        public class CommandsConfig
        {
            public List<Commands> Commands { get; set; } = new();
        }

        private static string[] GetTomlArray(TomlTable table, string key)
        {
            if (table.TryGetValue(key, out var value) && value is TomlArray array)
            {
                return array.OfType<string>().ToArray();
            }
            return Array.Empty<string>();
        }

        public class Commands
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string ClientServerCommand { get; set; } = string.Empty;
            public string ClientCommand { get; set; } = string.Empty;
            public string ServerCommand { get; set; } = string.Empty;
            public string[] Permissions { get; set; } = Array.Empty<string>();
        }
    }
}
