using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DisableChatWheel(SharpEssentials plugin) : PluginFeature(plugin) {

        private Cooldown? cooldown { get; set; }

        public override bool IsEnabled() {
            return config.DisableChatWheel.Enabled;
        }

        public override void Load() {
            var feature = config.DisableChatWheel;

            if(feature.UseCooldown) {
                cooldown = new Cooldown(feature.Cooldown, lang.ChatWheelCooldown.Tags());
            }

            plugin.AddCommandListener("playerchatwheel", (player, info) => OnChatWheelUse(player, info, config));
            
        }

        public override void Unload() {
            plugin.RemoveCommandListener("playerchatwheel", (player,info)=>OnChatWheelUse(player,info,config), HookMode.Pre);
            cooldown = null;
        }

        //Remove player from cooldown
        public override void OnDisconnect(CCSPlayerController player) {
            if(cooldown != null) {
                cooldown.Dispose(player);
            }
        }

        

        //Event handler!
        private HookResult OnChatWheelUse(CCSPlayerController? player, CommandInfo info, Configuration config) {
            if(player == null || !player.IsLegal()) return HookResult.Continue;

            if(config.DisableChatWheel.UseCooldown) {
                if(cooldown != null && cooldown.Has(player)) return HookResult.Handled;
                return HookResult.Continue;
            }

            return HookResult.Handled;
        }
    }
}
