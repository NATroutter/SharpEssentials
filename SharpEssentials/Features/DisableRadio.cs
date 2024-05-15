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
    public class DisableRadio(SharpEssentials plugin) : PluginFeature(plugin) {

        private Cooldown? cooldown { get; set; }

        private string[] RadioArray = new string[] {
            "coverme", "takepoint", "holdpos", "regroup",
            "followme", "takingfire", "go", "fallback",
            "sticktog",  "getinpos", "stormfront", "report",
            "roger", "enemyspot", "needbackup", "sectorclear",
            "inposition", "reportingin", "getout", "negative",
            "enemydown", "sorry", "cheer", "compliment",
            "thanks", "go_a", "go_b", "needrop",
            "deathcry"
        };

        public override bool IsEnabled() {
            return config.DisableRadio.Enabled;
        }

        public override void Load() {
            var feature = config.DisableRadio;

            if(feature.UseCooldown) {
                cooldown = new Cooldown(feature.Cooldown, lang.RadioCooldown.Tags());
            }

            //Register event
            for(int i = 0; i < RadioArray.Length; i++) {
                plugin.AddCommandListener(RadioArray[i], (player, info) => OnRadioCommandUse(player, info, config));
            }
            
        }
        
        public override void Unload() {
            for(int i = 0; i < RadioArray.Length; i++) {
                plugin.RemoveCommandListener(RadioArray[i], (player, info) => OnRadioCommandUse(player, info, config), HookMode.Pre);
            }
            cooldown = null;
        }

        //Remove player from cooldown
        public override void OnDisconnect(CCSPlayerController? player) {
            if(cooldown != null && player != null && player.IsLegal()) {
                cooldown.Dispose(player);
            }
        }

        private HookResult OnRadioCommandUse(CCSPlayerController? player, CommandInfo info, Configuration config) {
            if(player == null || !player.IsLegal()) return HookResult.Continue;

            if(config.DisableRadio.UseCooldown) {
                if(cooldown != null && cooldown.Has(player)) return HookResult.Handled;
                return HookResult.Continue;
            }

            return HookResult.Handled;
        }
    }
}
