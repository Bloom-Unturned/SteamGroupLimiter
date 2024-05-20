using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.API.Eventing;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Players.Connections.Events;
using OpenMod.Unturned.Plugins;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html
namespace SteamGroupLimiter.Events
{
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent> 
    {
        public UnturnedPlayerConnectedEventListener()
        {
        }

        public Task HandleEventAsync(object sender, UnturnedPlayerConnectedEvent @event)
        {
            UnturnedPlayer player = @event.Player;

            if (Provider.modeConfigData.Gameplay.Max_Group_Members != 0 && GetMemberCount(player.Player.quests.groupID) > Provider.modeConfigData.Gameplay.Max_Group_Members)
            {
                player.Player.quests.leaveGroup(true);
                player.PrintMessageAsync("Tu grupo excede el limite de 8 miembros!");
            }

            return Task.CompletedTask;
        }

        public byte GetMemberCount(CSteamID ID)
        {
            if (ID == CSteamID.Nil) return 0;
            return (byte)Provider.clients.Where(x => x.player.quests.isMemberOfGroup(ID)).Count();
        }
    }

}
