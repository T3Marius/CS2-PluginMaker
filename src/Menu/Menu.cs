using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using PluginMaker;
using static PluginMaker.PluginMaker;
using static PluginMaker.Main_Config;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API;
using Microsoft.Extensions.Logging;
using CounterStrikeSharp.API.Core.Translations;
using Menu;
using Menu.Enums;
using WASDSharedAPI;
using CounterStrikeSharp.API.Core.Capabilities;

public static class MenuClass
{
    public static void Load()
    {
        var config = Menu_Config.Load();

        foreach (var menu in config.Menus.Values)
        {
            string cmd = menu.Command;
            string description = $"Executes {menu.Name} menu command";

            Instance.AddCommand($"css_{cmd}", description, (player, commandInfo) =>
            {
                OpenMenu(player, commandInfo, menu);
            });
        }
    }
    public static IWasdMenuManager? wasdMenuManager;
    public static IWasdMenuManager? GetMenuManager()
    {
        if (wasdMenuManager == null)
            wasdMenuManager = new PluginCapability<IWasdMenuManager>("wasdmenu:manager").Get();

        return wasdMenuManager;
    }
    private static void OpenMenu(CCSPlayerController? player, CommandInfo info, Menus menu)
    {
        if (player == null)
            return;

        if (menu.Permissions.Count > 0 && !menu.Permissions.Any(permissons => AdminManager.PlayerHasPermissions(player, permissons)))
        {
            info.ReplyToCommand(MainConfig.Tag + Instance.Localizer["command.no.permission"]);
            return;
        }

        switch (menu.Type.ToLower())
        {
            case "html":
            case "htmlmenu":
            case "center":
            case "centerhtml":
                OpenHtml(player, menu);
                break;
            case "chat":
            case "chatmenu":
            case "text":
            case "textmenu":
                OpenChat(player, menu);
                break;
            case "kitsune":
            case "kitsunemenu":
                OpenKitsune(player, menu);
                break;
            case "wasd":
            case "wasdmenu":
                OpenWasdMenu(player, menu);
                break;
            default:
                Instance.Logger.LogWarning("Unknow menu type at csgo/addons/counterstrikesharp/configs/Plugins/PluginMaker/menus.json");
                break;
        }
    }

    private static void OpenHtml(CCSPlayerController player, Menus menuConfig)
    {
        CenterHtmlMenu menu = new(menuConfig.Name, Instance);

        foreach (var option in menuConfig.Options)
        {
            menu.AddMenuOption(option.Name, (player, menuOption) =>
            {
                ExecuteOptions(player, option);
            });
        }
        MenuManager.OpenCenterHtmlMenu(Instance, player, menu);
    }

    private static void OpenChat(CCSPlayerController player, Menus menuConfig)
    {
        ChatMenu menu = new(menuConfig.Name);

        foreach (var option in menuConfig.Options)
        {
            menu.AddMenuOption(option.Name, (player, menuOption) =>
            {
                ExecuteOptions(player, option);
            });
        }
        MenuManager.OpenChatMenu(player, menu);
    }
    private static void OpenKitsune(CCSPlayerController player, Menus menuConfig)
    {
        var kitsuneMenu = new KitsuneMenu(Instance);
        var menuItems = menuConfig.Options.Select(option => new MenuItem(MenuItemType.Button, new List<MenuValue> { new MenuValue(option.Name) })).ToList();

        kitsuneMenu.ShowScrollableMenu(player, menuConfig.Name, menuItems, (buttons, currentMenu, selectedItem) =>
        {
            if (buttons == MenuButtons.Exit)
                return;

            if (selectedItem != null)
            {
                var option = menuConfig.Options.FirstOrDefault(opt => opt.Name == selectedItem.Values![0].Value);
                if (option != null)
                {
                    ExecuteOptions(player, option);
                }
            }
        }, false, menuConfig.FreezeMenu);
    }
    public static void OpenWasdMenu(CCSPlayerController player, Menus menuConfig)
    {
        var manager = GetMenuManager();
        if (manager == null)
            return;

        IWasdMenu menu = manager.CreateMenu(menuConfig.Name);
        AddWasdOptions(menu, menuConfig.Options.ToDictionary(opt => opt.Name));
        manager.OpenMainMenu(player, menu);
    }

    public static void AddWasdOptions(IWasdMenu menu, Dictionary<string, Options> options)
    {
        var manager = GetMenuManager();
        if (manager == null)
            return;

        foreach (var option in options.Values)
        {
            menu.Add(option.Name, (player, Instance) =>
            {
                ExecuteOptions(player, option);
            });
        }
    }
    public static void ExecuteOptions(CCSPlayerController player, Options option)
    {

        if (!string.IsNullOrEmpty(option.Message))
        {
            player.PrintToChat(option.Message.ReplaceColorTags());
        }

        if (!string.IsNullOrEmpty(option.ServerCommand))
        {
            Server.ExecuteCommand(option.ServerCommand);
        }

        if (!string.IsNullOrEmpty(option.ClientCommand))
        {
            player.ExecuteClientCommand(option.ClientCommand);
        }

        if (!string.IsNullOrEmpty(option.ClientCommandFromServer))
        {
            player.ExecuteClientCommandFromServer(option.ClientCommandFromServer);
        }
    }
}
