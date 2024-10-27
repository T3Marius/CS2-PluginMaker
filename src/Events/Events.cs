using CounterStrikeSharp.API.Core;
using static PluginMaker.PluginMaker;
using static CounterStrikeSharp.API.Core.Listeners;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core.Translations;

namespace PluginMaker
{
    public static class Events
    {
        private static readonly List<CounterStrikeSharp.API.Modules.Timers.Timer> _timers = new();
        private static AnnConfig? _config;

        public static void Load()
        {
            _config = AnnouncementsConfig.Load();
            Instance.RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);
            Instance.RegisterListener<OnTick>(OnTick);
            StartTimers();
        }

        public static void UnLoad()
        {
            Instance.RemoveListener<OnTick>(OnTick);
            StopTimers();
        }
        
        public static void OnTick()
        {

        }

        private static void ShowAnnouncement(Announcement announcement)
        {
            string message = StringExtensions.ReplaceColorTags(announcement.Message);

            foreach (var player in Utilities.GetPlayers())
            {
                switch (announcement.Type.ToLower())
                {
                    case "chat":
                        player.PrintToChat(message);
                        break;
                    case "center":
                        player.PrintToCenter(message);
                        break;
                    case "alert":
                    case "centeralert":
                        player.PrintToCenterAlert(message);
                        break;
                    case "centerhtml":
                    case "html":
                        player.PrintToCenterHtml(message);
                        break;
                }
            }
        }

        private static void StartTimers()
        {
            if (_config?.Announcements == null || !_config.Announcements.Any())
            {
                return;
            }

            foreach (var announcement in _config.Announcements)
            {
                var timer = Instance.AddTimer(announcement.Interval, () =>
                {
                    ShowAnnouncement(announcement);
                }, TimerFlags.REPEAT);

                if (timer != null)
                {
                    _timers.Add(timer);
                }
            }
        }

        private static void StopTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Kill();
            }
            _timers.Clear();
        }

        public static HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
        {
            if (_config == null)
                return HookResult.Continue;

            CCSPlayerController? player = @event.Userid;
            if (player != null)
            {
                if (!_config.WelcomeMessage.EnableWelcomeMessage)
                {
                    return HookResult.Continue;
                }

                string welcomeMessage = StringExtensions.ReplaceColorTags(
                    _config.WelcomeMessage.Message.Replace("{playername}", player.PlayerName)
                );

                switch (_config.WelcomeMessage.PrintTo.ToLower())
                {
                    case "chat":
                        Instance.AddTimer(_config.WelcomeMessage.Delay, () =>
                        {
                            player.PrintToChat(welcomeMessage);
                        });
                        break;
                    case "center":
                        Instance.AddTimer(_config.WelcomeMessage.Delay, () =>
                        {
                            player.PrintToCenter(welcomeMessage);
                        });
                        break;
                    case "alert":
                        Instance.AddTimer(_config.WelcomeMessage.Delay, () =>
                        {
                            player.PrintToCenterAlert(welcomeMessage);
                        });
                        break;
                    case "centerhtml":
                        Instance.AddTimer(_config.WelcomeMessage.Delay, () =>
                        {
                            player.PrintToCenterHtml(welcomeMessage);
                        });
                        break;
                }
            }
            return HookResult.Continue;
        }
    }
}
