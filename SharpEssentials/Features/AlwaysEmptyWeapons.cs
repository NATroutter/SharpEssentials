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
    public class AlwaysEmptyWeapons(SharpEssentials plugin) : PluginFeature(plugin) {

        public override bool IsEnabled() {
            return config.AlwaysEmptyWeapons.Enabled;
        }

        public override void Load() {
            plugin.RegisterEventHandler<EventItemPurchase>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(player.IsLegal()) {
                    var weapon = player.FindWeapon(@event.Weapon);
                    if(weapon != null) {
                        weapon.SetAmmo(0, 0);
                    }
                }
                return HookResult.Continue;
            }, HookMode.Pre);

            plugin.RegisterEventHandler<EventPlayerSpawn>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(player.IsLegal()) {
                    player.removeAllWeaponAmmo();
                }
                return HookResult.Continue;
            }, HookMode.Post);

            plugin.RegisterEventHandler<EventItemPickup>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(player.IsLegal()) {
                    player.removeAllWeaponAmmo(); //TODO make this better (get weapon from event and check only that!)
                }
                return HookResult.Continue;
            }, HookMode.Post);

            plugin.RegisterEventHandler<EventPlayerTeam>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(player.IsLegal()) {
                    player.removeAllWeaponAmmo();
                }
                return HookResult.Continue;
            }, HookMode.Pre);
        }
        
    }
}
