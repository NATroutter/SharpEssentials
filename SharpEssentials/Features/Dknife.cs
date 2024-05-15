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
    public class Dknife(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {

        public override CommandConfig GetConfig() {
            return config.Dknife;
        }
        
        public override bool IsEnabled() {
            return config.Dknife.Enabled;
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            if (!player.IsLegalAlive()) player.Send(lang.OnlyAlive);

            if(command.ArgCount == 1) {

                foreach(var item in player.FindWeapons()) {
                   if(item.isKnife()) {
                        command.Reply(lang.KnifeAlreadyInBelt);
                        return;
                    }
                }

                CsItem knife = (player.Team == CsTeam.Terrorist ? CsItem.KnifeT : CsItem.Knife);

                player.GiveNamedItem(knife);
                command.Reply(lang.KnifeAddedToBelt);
                return;
            } else {
                command.Reply(lang.TooManyArgs);
            }

        }
    }
}
