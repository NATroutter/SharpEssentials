using CounterStrikeSharp.API.Core;

namespace SharpEssentials {
    public class DisabledBroadcasts(SharpEssentials plugin) : PluginFeature(plugin) {

        public override bool IsEnabled() {
            var d = config.DisableBroadcast;
            return (d.BombPlantAnouncement || d.WinOrLoseSound || d.WinOrLosePanel || d.MVPSound || d.DefaultDisconnectMessage || d.DefaultTeamJoinMessage || d.KillFeed);
        }

        public override void Load() {
            var disabled = config.DisableBroadcast;

            if(disabled.BombPlantAnouncement) {
                plugin.RegisterEventHandler<EventBombPlanted>((@event, info) => {
                    return HookResult.Handled;
                }, HookMode.Pre);
            }

            if(disabled.WinOrLoseSound) {
                plugin.RegisterEventHandler<EventRoundEnd>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

            if(disabled.WinOrLosePanel) {
                plugin.RegisterEventHandler<EventCsWinPanelRound>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

            if(disabled.MVPSound) {
                plugin.RegisterEventHandler<EventRoundMvp>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

            if(disabled.DefaultDisconnectMessage) {
                plugin.RegisterEventHandler<EventPlayerDisconnect>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

            if(disabled.DefaultTeamJoinMessage) {
                plugin.RegisterEventHandler<EventPlayerTeam>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

            if(disabled.KillFeed) {
                plugin.RegisterEventHandler<EventPlayerDeath>((@event, info) => {
                    info.DontBroadcast = true;
                    return HookResult.Continue;
                }, HookMode.Pre);
            }

        }


    }
}
