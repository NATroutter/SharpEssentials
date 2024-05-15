using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    internal class DamageInfo(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {
     
        private Dictionary<ulong,List<DamageEntry>> damages = new Dictionary<ulong, List<DamageEntry>>();

        public override CommandConfig GetConfig() {
            return config.DamageInfo.Command;
        }

        public override bool IsEnabled() {
            return config.DamageInfo.Enabled;
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            player.PrintToCenterAlert("<h1 style='color:blue'>Neekeri</h1>");
            
            //GetSummary(player);
        }

        public override void Load() {

            plugin.RegisterEventHandler<EventPlayerHurt>((@event, info) => {
                var cfg = config.DamageInfo;

                CCSGameRules? GameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules;
                if(GameRules == null || GameRules.WarmupPeriod) return HookResult.Continue;

                CCSPlayerController? victim = @event.Userid;
                if(!victim.IsLegal()) return HookResult.Continue;
                if(!cfg.IncludeBots && victim.IsBot) return HookResult.Continue;

                CCSPlayerController? attacker = @event.Attacker;
                if(!attacker.IsLegal())  return HookResult.Continue;
                if(!cfg.IncludeBots && attacker.IsBot) return HookResult.Continue;

                if(victim.TeamNum == attacker.TeamNum && !cfg.FriendlyFireMode) return HookResult.Continue;

                int DmgHealth = @event.DmgHealth;
                int DmgArmor = @event.DmgArmor;

                PrintInfos(attacker, victim, cfg.PrintWhenDamaged, DmgHealth, DmgArmor);

                addDamageEntry(attacker, new DamageEntry(victim.SteamID, DmgHealth, DmgArmor));

                return HookResult.Continue;
            });
            plugin.RegisterEventHandler<EventRoundEnd>((@event, info) => {
                var cfg = config.DamageInfo;
                foreach(var p in Utilities.GetPlayers()) {
                    GetSummary(p).ForEach(sum => {
                        p.PrintToChat($"[{sum.name} - {sum.done.hp} - {sum.done.armor} - {sum.done.hits} | {sum.taken.hp} - {sum.taken.armor} - {sum.taken.hits}]");
                    });
                }
                return HookResult.Continue;
            });
        }

        public override void OnDisconnect(CCSPlayerController player) {
            damages.Remove(player.SteamID);
        }

        private void addDamageEntry(CCSPlayerController player, DamageEntry entry) {
            if(!damages.ContainsKey(player.SteamID)) damages.Add(player.SteamID, new List<DamageEntry>());
            var entries = damages[player.SteamID];
            entries.Add(entry);
            damages[player.SteamID] = entries;
        }

        private List<DamageSummary> GetSummary(CCSPlayerController player) {
            List<DamageSummary> summary = new List<DamageSummary>();

            Dictionary<ulong, DamageEntryHits> damageDone = new Dictionary<ulong, DamageEntryHits>();
            Dictionary<ulong, DamageEntryHits> damageTaken = new Dictionary<ulong, DamageEntryHits>();

            foreach(var entry in damages) {
                foreach(DamageEntry damage in entry.Value) {
                    if(entry.Key == player.SteamID) {
                        if(!damageDone.ContainsKey(damage.victimID)) {
                            damageDone[damage.victimID] = new DamageEntryHits(damage.victimID, 0, 0, 0);
                        }

                        DamageEntryHits currentDamage = damageDone[damage.victimID];
                        currentDamage.hp += damage.hp;
                        currentDamage.armor += damage.armor;
                        currentDamage.hits += 1;
                    } else if(damage.victimID == player.SteamID) {
                        if(!damageTaken.ContainsKey(entry.Key)) {
                            damageTaken[entry.Key] = new DamageEntryHits(entry.Key, 0, 0, 0);
                        }

                        DamageEntryHits currentDamage = damageTaken[entry.Key];
                        currentDamage.hp += damage.hp;
                        currentDamage.armor += damage.armor;
                        currentDamage.hits += 1;
                    }
                }
            }

            foreach(ulong victimID in damageDone.Keys.Union(damageTaken.Keys)) {
                CCSPlayerController? victim = Utilities.GetPlayerFromSteamId(victimID);
                string name = (victim != null ? victim.PlayerName : "Unknown");
                DamageEntryHits done = damageDone.ContainsKey(victimID) ? damageDone[victimID] : new DamageEntryHits(victimID, 0, 0, 0);
                DamageEntryHits taken = damageTaken.ContainsKey(victimID) ? damageTaken[victimID] : new DamageEntryHits(victimID, 0, 0, 0);
                summary.Add(new DamageSummary(name, done, taken));
            }
            return summary;
        }

        private void PrintInfos(CCSPlayerController attacker, CCSPlayerController victim, PrintModeConf mode, int hp, int armor) {
            string name = victim.PlayerName;
            if(mode.Console) {
                attacker.PrintToCenterHtml("<h1 style='color:blue'>Damaged: "+name+" | -"+hp+" hp -"+armor+" armor</h1>", 1000);
            }
            if(mode.Chat) {
                attacker.PrintToChat("Damaged: " + name + " | -" + hp + " hp -" + armor + " armor");
            }
            if(mode.Console) {
                attacker.PrintToConsole("Damaged: " + name + " | -" + hp + " hp -" + armor + " armor");
            }
        }

    }
}
