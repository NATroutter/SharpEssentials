using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {

    public class PluginManager {

        public BasePlugin plugin;
        public Configuration config;

        private List<PluginFeature> features = new List<PluginFeature>();

        public PluginManager(SharpEssentials plugin) {
            this.plugin = plugin;
            this.config = plugin.Config;

            //Handle user disconnection!!
            plugin.RegisterEventHandler<EventPlayerDisconnect>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(!player.IsLegal()) return HookResult.Continue;

                //Call the disconnect hook!
                foreach(PluginFeature handler in features) {
                    handler.OnDisconnect(player);

                    if(handler is PluginFeatureWithCommand cmd) {
                        cmd.DisposeCooldown(player);
                    }
                }

                return HookResult.Continue;
            }, HookMode.Pre);


            //Handle user fully connected!
            plugin.RegisterEventHandler<EventPlayerConnectFull>((@event, info) => {
                CCSPlayerController? player = @event.Userid;
                if(!player.IsLegal()) return HookResult.Continue;

                //Call the connected hook!
                foreach(PluginFeature handler in features) {
                    handler.OnConnected(player);
                }
                return HookResult.Continue;
            }, HookMode.Pre);

        }

        public void printStats() {
            Console.WriteLine("Enabled Features:");
            features.ForEach(handler => {
                if(handler is not MainCommand) {
                    Console.WriteLine("- " + handler.GetType().Name);
                    if(handler is PluginFeatureWithCommand cmd) {
                        cmd.GetConfig().Commands.ForEach(c => {
                            Console.WriteLine(" - css_" + c);
                        });
                    }
                }
            });
            Console.WriteLine(" ");
        }

        public void Register(PluginFeature handler) {

            if(handler.IsEnabled()) {

                if(handler is PluginFeatureWithCommand cmd && cmd.IsEnabled()) {
                    //Handdle command registeration!
                    cmd.GetConfig().Commands.ForEach(x => {
                        plugin.AddCommand("css_" + x, cmd.GetConfig().Description, (player, info) => callCommand(player,info,cmd));
                    });
                }

                //Handle loading!!!
                handler.Load();
                features.Add(handler);
            }
        }

        public void callCommand(CCSPlayerController? player, CommandInfo info, PluginFeatureWithCommand cmd) {
            if(!player.IsLegal()) {
                cmd.OnServerConsoleCommand(info);
                return;
            }

            if(cmd.GetConfig().usePermission) {
                if(player.HasPermission(cmd.GetConfig().Permission)) {

                    if(cmd.GetConfig().UseCooldown && cmd.hasCooldown(player)) return;

                    cmd.OnCommand(player, info);
                } else {
                    info.Reply(config.Language.NoPermission);
                }
            } else {

                if(cmd.GetConfig().UseCooldown && cmd.hasCooldown(player)) return;

                cmd.OnCommand(player, info);
            }
        }

        //Unregister all commands
        public void unload() {
            foreach (PluginFeature handler in features) {
                if(handler is PluginFeatureWithCommand cmd) {
                    if(cmd.IsEnabled()) {
                        cmd.GetConfig().Commands.ForEach(x => plugin.RemoveCommand("css_" + x, (player, info) => callCommand(player, info, cmd)));
                    }
                    cmd.DestroyCooldown();
                }
                handler.Unload();
            }
        }
    }
}
