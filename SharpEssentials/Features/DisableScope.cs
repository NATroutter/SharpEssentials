using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace SharpEssentials {
    public class DisableScope(SharpEssentials plugin) : PluginFeature(plugin) {

        private Dictionary<ulong, bool> zooming = new Dictionary<ulong, bool>();

        public override bool IsEnabled() {
            var disabled = config.DisableScope;
            return (disabled.awp || disabled.ssg08 || disabled.g3sg1 || disabled.scar20 || disabled.aug || disabled.sg556);
        }

        //awp ssg08 aug sg556 g3sg1 scar20
        public override void Load() {
            plugin.RegisterListener<Listeners.OnTick>(() => {
                foreach(var player in Utils.GetAlivePlayers()) {
                    OnTick(player, config);
                }
            });
        }

        public override void OnDisconnect(CCSPlayerController player) {
            zooming.Remove(player.SteamID);
        }


        private void OnTick(CCSPlayerController player, Configuration config) {
            var disabled = config.DisableScope;

            var weaponService = player.PlayerPawn.Value?.WeaponServices;
            if(weaponService == null) return;

            var activeWeapon = weaponService.ActiveWeapon.Value;
            if(activeWeapon == null) return;

            if(!zooming.ContainsKey(player.SteamID)) {
                zooming.Add(player.SteamID, false);
            }

            try {
                if(weaponService.MyWeapons.Count != 0) {
                    var weaponName = activeWeapon.DesignerName;

                    bool disableScope = weaponName switch {
                        "weapon_awp" => disabled.awp,
                        "weapon_ssg08" => disabled.ssg08,
                        "weapon_aug" => disabled.aug,
                        "weapon_sg556" => disabled.sg556,
                        "weapon_g3sg1" => disabled.g3sg1,
                        "weapon_scar20" => disabled.scar20,
                        _ => false
                    };

                    if(disableScope) {
                        activeWeapon.NextSecondaryAttackTick = Server.TickCount + 600;
                        var buttons = player.Buttons;
                        if(!zooming[player.SteamID] && (buttons & PlayerButtons.Attack2) != 0) {
                            zooming[player.SteamID] = true;

                            Server.NextFrame(() => {
                                player.PrintToChat(lang.ScopingNotAllowed.Tags());
                            });

                        } else if(zooming[player.SteamID] && (buttons & PlayerButtons.Attack2) == 0) {
                            zooming[player.SteamID] = false;
                        }

                    }
                }
            } catch(Exception ex) {
                Logger.error("[NoScope] " + ex.ToString());
            }
            
        }
    }
}
