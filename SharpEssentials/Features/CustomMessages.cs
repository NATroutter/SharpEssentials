using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace SharpEssentials {
    public class CustomMessages(SharpEssentials plugin) : PluginFeature(plugin) {

        public override bool IsEnabled() {
            return config.CustomMessages.Enabled;
        }

        public override void Load() {
            var cm = config.CustomMessages;

            plugin.RegisterEventHandler<EventPlayerTeam>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(!player.IsLegal()) return HookResult.Continue;
                if (!cm.IncludeBots && player.IsBot) return HookResult.Continue;

                if(cm.useTeamJoinT && @event.Team == PlayerExtension.TEAM_T) {
                    Server.PrintToChatAll(lang.JoinTeam_T.Tags(@event.Userid));
                }
                if(cm.useTeamJoinCT && @event.Team == PlayerExtension.TEAM_CT) {
                    Server.PrintToChatAll(lang.JoinTeam_CT.Tags(@event.Userid));
                }
                if(cm.useTeamJoinSPEC && @event.Team == PlayerExtension.TEAM_SPEC) {
                    Server.PrintToChatAll(lang.JoinTeam_SPEC.Tags(@event.Userid));
                }

                return HookResult.Continue;
            }, HookMode.Post);
        }

        public override void OnConnected(CCSPlayerController player) {
            var cm = config.CustomMessages;
            if(!cm.IncludeBots && player.IsBot) return;
            if(cm.useJoin) Server.PrintToChatAll(lang.JoinMessage.Tags(player));
        }
        public override void OnDisconnect(CCSPlayerController player) {
            var cm = config.CustomMessages;
            if(!cm.IncludeBots && player.IsBot) return;
            if(cm.useQuit) Server.PrintToChatAll(lang.QuitMessage.Tags(player));
        }
    }
}
