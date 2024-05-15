using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DisablePing(SharpEssentials plugin) : PluginFeature(plugin) {

        private Cooldown? cooldown { get; set; }

        public override bool IsEnabled() {
            return config.DisablePing.Enabled;
        }

        public override void Load() {
            var feature = config.DisablePing;

            if(feature.UseCooldown) {
                cooldown = new Cooldown(feature.Cooldown, lang.PingCooldown.Tags());
            }

            plugin.AddCommandListener("player_ping", (player, info) => OnPing(player, info, config));
            
        }

        public override void Unload() {
            plugin.RemoveCommandListener("player_ping", (player, info) => OnPing(player, info, config), HookMode.Pre);
            cooldown = null;
        }

        //Remove player from cooldown
        public override void OnDisconnect(CCSPlayerController player) {
            if(cooldown != null) {
                cooldown.Dispose(player);
            }
        }

        private HookResult OnPing(CCSPlayerController? player, CommandInfo info, Configuration config) {
            if(player == null || !player.IsLegal()) return HookResult.Continue;

            if(config.DisablePing.UseCooldown) {
                if(cooldown != null && cooldown.Has(player)) return HookResult.Handled;
                return HookResult.Continue;
            }
            return HookResult.Handled;
        }
    }
}
