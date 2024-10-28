using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace PluginMaker
{
    public class PluginMaker : BasePlugin
    {
        public override string ModuleAuthor => "T3Marius";
        public override string ModuleName => "[ Plugin Maker ]  ";
        public override string ModuleVersion => "1.1";
        public override string ModuleDescription => "Easily create menus, submenus, welcome messages, announcements, and commands from separate configs.";
        public static PluginMaker Instance { get; set; } = new();
        public override void Load(bool hotReload)
        {
            Instance = this;
            Events.Load();
            AnnouncementsConfig.Load();
            Main_Config.Load();
            CommandsConfigLoader.Load();
            Commands.Load();
            Menu_Config.Load();
            MenuClass.Load();

            if (!Main_Config.MainConfig.Settings.EnableAnnouncements)
            {
                Events.UnLoad();
                Logger.LogInformation("ANNOUNCEMENTS ARE DISABLED FROM CONFIG.");
            }
        }
    }
}
