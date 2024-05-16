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
     
        private Dictionary<int,List<DamageEntry>> damages = new Dictionary<int, List<DamageEntry>>();


        public override CommandConfig GetConfig() {
            return config.DamageInfo.Command;
        }

        public override bool IsEnabled() {
            return config.DamageInfo.Enabled;
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
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
                int HitBox = @event.Hitgroup;

                if(!attacker.IsBot) {
                    PrintInfos(attacker, victim, cfg.PrintWhenDamaged, DmgHealth, DmgArmor, HitBox);
                }

                addDamageEntry(attacker, new DamageEntry(victim.Slot, victim.PlayerName, DmgHealth, DmgArmor));
                
                return HookResult.Continue;
            });
            plugin.RegisterEventHandler<EventRoundEnd>((@event, info) => {
                PrintAllSummaries();
                damages.Clear();
                return HookResult.Continue;
            });
        }

        private void PrintAllSummaries() {
            var cfg = config.DamageInfo;
            foreach(var p in Utilities.GetPlayers()) {
                if (p.IsBot) continue;
                var summaries = GetSummary(p);
                if(summaries.Count() > 0) {
                    lang.DamageInfo.Summary.Header.ForEach(p.Send);
                    for(int i = 0; i < summaries.Count; i++) {
                        var sum = summaries[i];
                        formatSummaryLine(i + 1, sum).ForEach(p.Send);
                    }
                    lang.DamageInfo.Summary.Footer.ForEach(p.Send);
                }
            }
        }
        private List<string> formatSummaryLine(int num, DamageSummary sum) {
            var formatLines = lang.DamageInfo.Summary.Entry;
            List<string> summary = new List<string>();

            foreach(var format in formatLines) {
                string name = sum.name != null ? sum.name : GetPlayerName(sum.done.victimSlot);
                string line = format;
                line = line.ReplaceIgnoreCase("{NUM}", num);
                line = line.ReplaceIgnoreCase("{NAME}", name);
                line = line.ReplaceIgnoreCase("{HP}", sum.taken.hp);
                line = line.ReplaceIgnoreCase("{ARMOR}", sum.taken.armor);
                line = line.ReplaceIgnoreCase("{HITS}", sum.taken.hits);
                line = line.ReplaceIgnoreCase("{ENEMY_HP}", sum.done.hp);
                line = line.ReplaceIgnoreCase("{ENEMY_ARMOR}", sum.done.armor);
                line = line.ReplaceIgnoreCase("{ENEMY_HITS}", sum.done.hits);
                summary.Add(line);
            }
            return summary;
        }

        private void addDamageEntry(CCSPlayerController player, DamageEntry entry) {
            if(!damages.ContainsKey(player.Slot)) damages.Add(player.Slot, new List<DamageEntry>());
            var entries = damages[player.Slot];
            entries.Add(entry);
            damages[player.Slot] = entries;
        }

        private List<DamageSummary> GetSummary(CCSPlayerController player) {
            List<DamageSummary> summary = new List<DamageSummary>();

            Dictionary<int, DamageEntryHits> damageDone = new Dictionary<int, DamageEntryHits>();
            Dictionary<int, DamageEntryHits> damageTaken = new Dictionary<int, DamageEntryHits>();

            foreach(var entry in damages) {
                foreach(DamageEntry damage in entry.Value) {
                    if(entry.Key == player.Slot) {
                        if(!damageDone.ContainsKey(damage.victimSlot)) {
                            damageDone[damage.victimSlot] = new DamageEntryHits(damage.victimSlot, damage.victimName, 0, 0, 0);
                        }

                        DamageEntryHits currentDamage = damageDone[damage.victimSlot];
                        currentDamage.hp += damage.hp;
                        currentDamage.armor += damage.armor;
                        currentDamage.hits += 1;
                    } else if(damage.victimSlot == player.Slot) {
                        if(!damageTaken.ContainsKey(entry.Key)) {
                            damageTaken[entry.Key] = new DamageEntryHits(entry.Key, damage.victimName, 0, 0, 0);
                        }

                        DamageEntryHits currentDamage = damageTaken[entry.Key];
                        currentDamage.hp += damage.hp;
                        currentDamage.armor += damage.armor;
                        currentDamage.hits += 1;
                    }
                }
            }

            foreach(int slot in damageDone.Keys.Union(damageTaken.Keys)) {
                DamageEntryHits done = damageDone.ContainsKey(slot) ? damageDone[slot] : new DamageEntryHits(slot, GetPlayerName(slot), 0, 0, 0);
                DamageEntryHits taken = damageTaken.ContainsKey(slot) ? damageTaken[slot] : new DamageEntryHits(slot, GetPlayerName(slot), 0, 0, 0);
                summary.Add(new DamageSummary(done.victimName, done, taken));
            }
            return summary;
        }
        private string GetPlayerName(int slot) {
            CCSPlayerController? victim = Utilities.GetPlayerFromSlot(slot);
            return (victim != null ? victim.PlayerName : "Unknown");
        }

        private string GetHitBoxLang(int hitbox) {
            var boxes = lang.DamageInfo.HitBoxes;
            return hitbox switch {
                0 => boxes.Body,
                1 => boxes.Head,
                2 => boxes.Chest,
                3 => boxes.Stomach,
                4 => boxes.LeftArm,
                5 => boxes.RightArm,
                6 => boxes.LeftLeg,
                7 => boxes.RightLeg,
                10 => boxes.Gear,
                _ => boxes.Unknown
            };;
        }

        private void PrintInfos(CCSPlayerController attacker, CCSPlayerController victim, PrintModeConf mode, int hp, int armor, int box) {
            string name = victim.PlayerName;

            if(mode.Chat) {
                var format = lang.DamageInfo.ChatFormat;
                format = format.ReplaceIgnoreCase("{NAME}", name);
                format = format.ReplaceIgnoreCase("{HEALTH}", hp);
                format = format.ReplaceIgnoreCase("{ARMOR}", armor);
                format = format.ReplaceIgnoreCase("{HITBOX}", GetHitBoxLang(box));
                attacker.Send(format);
            }
            if(mode.Console) {
                var format = lang.DamageInfo.ConsoleFormat;
                format = format.ReplaceIgnoreCase("{NAME}", name);
                format = format.ReplaceIgnoreCase("{HEALTH}", hp);
                format = format.ReplaceIgnoreCase("{ARMOR}", armor);
                format = format.ReplaceIgnoreCase("{HITBOX}", GetHitBoxLang(box));
                attacker.PrintToConsole(format);
            }
        }

    }
}
