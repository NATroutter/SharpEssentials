using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class StringExtension {

        public static string Tags(this string text) {
            return text.Tags(null);
        }

        public static string Tags(this string text, CCSPlayerController? player) {
            Dictionary<string, string> tags = new Dictionary<string, string>() {
                { "{PREFIX}", SharpEssentials._Config.Language.Prefix },
                { "{NAME}", (player.IsLegal() ? player.PlayerName : "Unknown") },
                { "{STEAM_ID}", (player.IsLegal() ? player.SteamID.ToString() : "Unknown") }
            };
            
            foreach(var tag in tags) {
                if(text.Contains(tag.Key, StringComparison.OrdinalIgnoreCase)) {
                    text = text.Replace(tag.Key, tag.Value, StringComparison.OrdinalIgnoreCase);
                }
            }

            return text.ReplaceColorTags();
        }

        public static string ReplaceIgnoreCase(this string value, string pattern, object replacement) {
            return value.Replace(pattern, replacement.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCommand(this string message) {
            var prefixes = SharpEssentials._Config .ChatFormat.CommandTriggers;
            return prefixes.Any(prefix => message.StartsWith(prefix));
        }
    }

}