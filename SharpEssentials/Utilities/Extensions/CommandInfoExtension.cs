using CounterStrikeSharp.API.Modules.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class CommandInfoExtension {

        public static void Reply(this CommandInfo info, string message) {

            if(info.CallingContext == CommandCallingContext.Chat) {
                info.ReplyToCommand(message.Tags());
            } else {
                info.ReplyToCommand(RemoveTags(message.Tags()));
            }

        }

        private static string RemoveTags(string input) {
            return new Regex(@"\{.*?\}").Replace(input, "");
        }

    }
}
