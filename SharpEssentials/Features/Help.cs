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
    public class Help(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {
        public override CommandConfig GetConfig() {
            return config.Help.Command;
        }

        public override bool IsEnabled() {
            return config.Help.Enabled;
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            var cfg = config.Help;
            
            if(command.ArgCount == 1) {

                List<string> lines = new List<string>();
                lines.AddRange(cfg.HelpMessage.Start);

                foreach(var lineGroup in cfg.HelpMessage.Lines) {
                    if(cfg.UsePermission) {
                        if(player.HasPermission(lineGroup.Permission)) {
                            lines.AddRange(lineGroup.Lines);
                        }
                    } else {
                        lines.AddRange(lineGroup.Lines);
                    }
                }
                lines.AddRange(cfg.HelpMessage.End);
                lines.ForEach(x => command.Reply(x));


            } else if(command.ArgCount == 2) {
                if(cfg.SinglePageMode) return;
                string page = command.ArgByIndex(1);

                HelpCategory? cate = cfg.Categories.Find(x => x.Name.Equals(page, StringComparison.OrdinalIgnoreCase));
                
                if(cate == null) {
                    command.Reply(lang.CategoryNotExists);
                    return;
                }

                if(cfg.UsePermission && !player.HasPermission(cate.Permission)) {
                    command.Reply(lang.NoPermission);
                    return;
                }

                cate.Message.ForEach(x => command.Reply(x));

            } else {
                command.Reply(lang.TooManyArgs);
            }
        }
    }
}
