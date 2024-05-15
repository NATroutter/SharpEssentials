using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class WelcomeMessage(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {

        public override CommandConfig GetConfig() {
            return config.WelcomeMessage.Command;
        }

        public override bool IsEnabled() {
            return config.WelcomeMessage.Enabled;
        }

        public override void Load() {
            if(config.WelcomeMessage.AfterTeamJoin) {
                plugin.RegisterEventHandler<EventPlayerTeam>((@event, info) => {
                    if(@event.Oldteam == 0 && @event.Userid.IsLegal()) sendWelcome(@event.Userid);
                    return HookResult.Continue;
                }, HookMode.Pre);
            }
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            if(!player.IsLegalAlive()) player.Send(lang.OnlyAlive);

            if(command.ArgCount == 1) {
                sendWelcome(player);
            } else {
                command.Reply(lang.TooManyArgs);
            }
        }

        public override void OnConnected(CCSPlayerController player) {
            if(!config.WelcomeMessage.AfterTeamJoin) {
                plugin.RunDelayed(config.WelcomeMessage.MillsecondsBeforeSending, () => {
                    if(player.IsConnected()) sendWelcome(player);
                });
            }
        }
        
        private void sendWelcome(CCSPlayerController player) {
            foreach(string line in config.WelcomeMessage.Message) {
                player.Send(line);
            }
        }

    }
}
