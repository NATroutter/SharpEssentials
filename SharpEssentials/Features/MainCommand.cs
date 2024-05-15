using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class MainCommand(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {
        public override CommandConfig GetConfig() {
            return new CommandConfig(
                true,
                ["sharpessentials", "sess"],
                "Shows information about plugin!",
                false,
                "@sess/maincommand",
                false,
                10,
                "feature on cooldown!"
            );
        }

        public override bool IsEnabled() {
            return true;
        }

        public override void OnCommand(CCSPlayerController? player, CommandInfo command) {
            var info = SharpEssentials.pluginInfo;
            command.Reply(" ");
            command.Reply($" {ChatColors.Green}| SharpEssentials |");
            command.Reply($" {ChatColors.Green}» {ChatColors.Grey}Version: {ChatColors.Lime}" + info.version);
            command.Reply($" {ChatColors.Green}» {ChatColors.Grey}Author: {ChatColors.Lime}" + info.author);
            command.Reply($" {ChatColors.Green}» {ChatColors.Grey}Website: {ChatColors.Lime}" + info.website);
            command.Reply($" {ChatColors.Green}» {ChatColors.Grey}Project: {ChatColors.Lime}" + info.project);
            command.Reply(" ");
        }
    }
}
