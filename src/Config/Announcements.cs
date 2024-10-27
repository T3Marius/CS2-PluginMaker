using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API;

namespace PluginMaker
{
    public class AnnouncementsConfig
    {
        public static AnnConfig Load()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "";
            string configPath = $"{Server.GameDirectory}/csgo/addons/counterstrikesharp/configs/plugins/{assemblyName}/announcements.json";

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Configuration file not found: {configPath}");
            }

            string jsonContent = File.ReadAllText(configPath);
            AnnConfig config = JsonSerializer.Deserialize<AnnConfig>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            }) ?? new AnnConfig();

            return config;
        }
    }

    public class AnnConfig
    {
        [JsonPropertyName("WelcomeMessage")]
        public WelcomeMessageConfig WelcomeMessage { get; set; } = new WelcomeMessageConfig();

        [JsonPropertyName("Announcements")]
        public List<Announcement> Announcements { get; set; } = new List<Announcement>();
    }

    public class Announcement
    {
        public string Type { get; set; } = "chat";
        public string Message { get; set; } = "Default announcement message.";
        public float Interval { get; set; } = 60.0f;
    }

    public class WelcomeMessageConfig
    {
        public string Message { get; set; } = "Welcome to the server, {playername}!";
        public float Delay { get; set; } = 5.0f;
        public bool EnableWelcomeMessage { get; set; } = true;
        public string PrintTo { get; set; } = "chat";
    }
}
