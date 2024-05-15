using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Timers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace SharpEssentials {
    public class Advertisements(SharpEssentials plugin) : PluginFeature(plugin) {

        public override bool IsEnabled() {
            return config.Advertisements.Enabled;
        }

        private Timer? timer;
        private int index = 0;

        public override void Load() {
            var cfg = config.Advertisements;

            timer = plugin.AddTimer(cfg.Interval, () => {

                if(cfg.RandomOrder) {
                    index = new Random().Next(cfg.Messages.Count());
                    Server.PrintToChatAll(cfg.Messages[index].Tags());
                } else {
                    if(index > cfg.Messages.Count() - 1) index = 0;
                    Server.PrintToChatAll(cfg.Messages[index].Tags());
                    index++;
                }

            }, TimerFlags.REPEAT);
            
        }

        public override void Unload() {
            Utils.KillTimer(ref timer);
        }

    }
}
