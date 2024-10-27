using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using static PluginMaker.PluginMaker;
using static PluginMaker.Main_Config;
using CounterStrikeSharp.API;

namespace PluginMaker;

public static class Commands
{
    public static void Load()
    {
        var commmandsList = CommandsConfigLoader.CommandsList;

        foreach (var command in commmandsList)
        {
            Instance.AddCommand($"css_{command.Name}", command.Description, (player, cmdInfo) =>
            {
                if (!HasPermissions(player, command))
                {
                    cmdInfo.ReplyToCommand(MainConfig.Tag + Instance.Localizer["command.no.permission"]);
                    return;
                }
                ExecuteCommand(player, command, cmdInfo);
                ExecuteClientCommand(player, command, cmdInfo);
                ExecuteClientCommandFromServer(player, command, cmdInfo);
                ExecuteServerCommand(player, command, cmdInfo);
            });
        }
    }
    private static void ExecuteCommand(CCSPlayerController? player, CommandsConfigLoader.Commands command, CommandInfo info)
    {
        string message = StringExtensions.ReplaceColorTags(command.Message);

        if (player != null)
        {
            player.PrintToChat(message);
        }
    }
    private static void ExecuteClientCommand(CCSPlayerController? player, CommandsConfigLoader.Commands command, CommandInfo info)
    {
        if (player != null)
        {
            player.ExecuteClientCommand(command.ClientCommand);
        }
    }
    private static void ExecuteClientCommandFromServer(CCSPlayerController? player, CommandsConfigLoader.Commands command, CommandInfo info)
    {
        if (player != null)
        {
            player.ExecuteClientCommandFromServer(command.ServerCommand);
        }
    }
    private static void ExecuteServerCommand(CCSPlayerController? player, CommandsConfigLoader.Commands command, CommandInfo info)
    {
        if (player != null)
        {
            Server.ExecuteCommand(command.ServerCommand);
        }
    }
    private static bool HasPermissions(CCSPlayerController? player, CommandsConfigLoader.Commands commands)
    {
        if (commands.Permissions.Length == 0)
        {
            return true;
        }
        return commands.Permissions.Any(permission => AdminManager.PlayerHasPermissions(player, permission));
    }
}