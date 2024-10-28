# CS2-PluginMaker
Make sure you read this before installing the plugin so you won't make mistakes.
# MainConfig (not really that important)
```ini
[Tag]
Tag = "{red}[Plugin Maker]{default}"

[Settings]
EnableAnnouncements = false
EnableCommands = false
EnableMenu = true
```
# Commands Config
```ini
[[Commands]]
Name = "discord"
Description = "Shows server discord"
Message = "{red}[Plugin Maker]{default} Discord server: {lime}discord.gg/example"
ClientServerCommand = ""
ClientCommand = ""
ServerCommand = ""
Permissions = []

[[Commands]] // add this line for a new command.
Name = "ban"
Description = "bans menu"
Message = ""
ClientServerCommand = "css_admin"
ClientCommand = ""
ServerCommand = ""
Permissions = ["@css/root"]

```
# Announcements Config
```json
{
  "WelcomeMessage": {
    "Message": "Welcome to the server, {playername}!",
    "Delay": 1,
    "EnableWelcomeMessage": true,
    "PrintTo": "chat" // center, chat, html, alert
  },
  "Announcements": [
        {
            "Type": "chat", // type chat, center, alert, html
            "Message": "{red}Don't forget to join our discord server!",
            "Interval": 10 // interval in seconds
        },
        {
            "Type": "center",
            "Message": "Join our steam group!",
            "Interval": 15
        }
],
  "ConfigVersion": 1
}
```
# Menus Config
```json
{
  "Menus": {
    "menu1": {
      "Name": "Html Menu",
      "Type": "html", // chat, wasd, kitsune
      "Command": "htmlmenu", // !htmlmenu
      "FreezeMenu": true, // this is avalible only for kitsune for now.
      "Permissions": [],
      "Options": [
        {
          "Name": "Kick Marius_",
          "ServerCommand": "kick Marius_",
          "ClientCommandFromServer": "",
          "ClientCommand": "",
          "Message": "{red}[Admin]{default} You've kicked {lime}Marius_"
        },
        {
          "Name": "Add Bot To CT",
          "ServerCommand": "bot_add ct",
          "ClientCommandFromServer": "",
          "ClientCommand": "",
          "Message": "{red}[Admin]{default} You've added a bot to {blue}CT "
        }
      ]
    },
    "menu2": {
      "Name": "Admin Menu",
      "Type": "kitsune", // chat, wasd, kitsune
      "Command": "kitsunemenu",
      "FreezeMenu": true, 
      "Permissions": ["@css/root"], // custom permissions
      "Options": [
        {
          "Name": "Admin Menu",
          "ServerCommand": "",
          "ClientCommandFromServer": "css_admin",
          "ClientCommand": "",
          "Message": "{red}[Admin]{default} You've opened admin menu!"
        },
        {
          "Name": "Play a sound",
          "ServerCommand": "",
          "ClientCommandFromServer": "",
          "ClientCommand": "play birds/bird.vsnd_c",
          "Message": ""
        }
      ]
    }
  }
}

```
