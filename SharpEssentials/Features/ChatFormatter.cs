using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;


namespace SharpEssentials {
    public class ChatFormatter(SharpEssentials plugin) : PluginFeature(plugin) {

        public override bool IsEnabled() {
            return config.ChatFormat.Enabled;
        }

        private List<GroupTagConf> tags = new List<GroupTagConf>();

        public override void Load() {
            plugin.AddCommandListener("say", (player, info) => OnPlayerChatAll(player, info, config));
            plugin.AddCommandListener("say_team", (player, info) => OnPlayerChatTeam(player, info, config));

            tags = config.ChatFormat.GroupTags.OrderByDescending(x => x.priority).ToList();

        }

        public override void Unload() {
            plugin.RemoveCommandListener("say", (player, info) => OnPlayerChatAll(player, info, config), HookMode.Pre);
            plugin.RemoveCommandListener("say_team", (player, info) => OnPlayerChatTeam(player, info, config), HookMode.Pre);
        }
        
        private HookResult OnPlayerChatAll(CCSPlayerController? player, CommandInfo info, Configuration config) {
            string message = info.GetArg(1);
            
            if(!player.IsLegal() || player.IsBot || string.IsNullOrEmpty(message)) return HookResult.Handled;
            if(message.IsCommand()) return HookResult.Continue;

            var cfg = config.ChatFormat;

            return HandleGlobalChat(player, message, config);
        }

        private HookResult OnPlayerChatTeam(CCSPlayerController? player, CommandInfo info, Configuration config) {
            string message = info.GetArg(1);
            
            if(!player.IsLegal() || player.IsBot || string.IsNullOrEmpty(message)) return HookResult.Handled;
            if(message.IsCommand()) return HookResult.Continue;

            var cfg = config.ChatFormat;

            if (cfg.DisableTeamChat) {
                player.PrintToChat(lang.TeamChatIsDisabled.Tags());
                return HookResult.Handled;
            }
            if (cfg.TeamToGlobalChat) return HandleGlobalChat(player, message, config);

            return HandleTeamChat(player, message, config);
        }


        private bool FilterBadWords(CCSPlayerController player, ref string message, Configuration config) {
            var cfg = config.ChatFormat;
            foreach(string word in cfg.BadWords) {
                if(cfg.useReplaceMode) {
                    string repl = new String('*', word.Length);
                    message = cfg.IgnoreCase ? Regex.Replace(message, word, repl, RegexOptions.IgnoreCase) : Regex.Replace(message, word, repl);
                } else {
                    var reg = cfg.IgnoreCase ? Regex.Match(message, word, RegexOptions.IgnoreCase) : Regex.Match(message, word);
                    if(reg.Success) {
                        string msg = lang.MessageWithBadWords.Replace("{WORD}", reg.Value);
                        player.PrintToChat(msg.Tags());
                        return false;
                    }
                }
            }
            return true;
        }

        private HookResult HandleGlobalChat(CCSPlayerController player, string message, Configuration config) {
            var cfg = config.ChatFormat;
            GroupTagConf? tag = getPrimaryGroup(player);

            string group = (tag != null) ? tag.tag : "";
            string death = !player.IsLegalAlive() && !player.IsSpec() ? cfg.Tags.Death : "";
            string team = player.IsSpec() ? cfg.Tags.TeamSpec : (player.IsCt() ? cfg.Tags.TeamCT : cfg.Tags.TeamT);


            if(cfg.UseBadWordFilter) {
                if (!FilterBadWords(player, ref message, config)) return HookResult.Handled;
            }

            string format = cfg.Format_All;
            format = format.Replace("{DEATH}", death, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{GROUP}", group, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{TEAM}", team, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{MESSAGE}", message, StringComparison.OrdinalIgnoreCase);

            Server.PrintToChatAll(format.Tags(player));
            Console.WriteLine("[Chat][All] " + player.PlayerName + "(" + player.SteamID + "): " + message);

            return HookResult.Handled;
        }

        private HookResult HandleTeamChat(CCSPlayerController player, string message, Configuration config) {
            var cfg = config.ChatFormat;
            GroupTagConf? tag = getPrimaryGroup(player);

            string group = (tag != null) ? tag.tag : "";
            string death = !player.IsLegalAlive() && !player.IsSpec() ? cfg.Tags.Death : "";
            string team = player.IsSpec() ? cfg.Tags.TeamSpec : (player.IsCt() ? cfg.Tags.TeamCT : cfg.Tags.TeamT);

            if(cfg.UseBadWordFilter) {
                if(!FilterBadWords(player, ref message, config)) return HookResult.Handled;
            }

            string format = player.IsSpec() ? cfg.Format_Team_Spec : (player.IsCt() ? cfg.Format_Team_CT : cfg.Format_Team_T);
            
            format = format.Replace("{DEATH}", death, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{GROUP}", group, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{TEAM}", team, StringComparison.OrdinalIgnoreCase);
            format = format.Replace("{MESSAGE}", message, StringComparison.OrdinalIgnoreCase);

            player.PrintToTeamChat(format.Tags(player));

            string cteam = player.IsSpec() ? "[SPEC]" : (player.IsCt() ? "[CT]": "[T]");
            Console.WriteLine("[TEAM]["+ cteam + "] " + player.PlayerName + "(" + player.SteamID + "): " + message);
            
            return HookResult.Handled;
        }

        private GroupTagConf? getPrimaryGroup(CCSPlayerController player) {
            foreach(var item in tags) {
                if(AdminManager.PlayerInGroup(player, item.group)) {
                    return item;
                }
            }
            return null;
        }

    }
}
