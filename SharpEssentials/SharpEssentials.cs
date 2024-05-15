using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Linq;

namespace SharpEssentials {
    public class SharpEssentials : BasePlugin, IPluginConfig<Configuration> {


        public override string ModuleName => "SharpEssentials";
        public override string ModuleVersion => "1.2.0";
        public override string ModuleAuthor => "NATroutter";
        public override string ModuleDescription => "Essential features for cs2 server";

        public PluginManager pm = default!;
        public Configuration Config { get; set; } = default!;
        public static Configuration _Config { get; set; } = default!;
        public static PluginInfo pluginInfo = default!;


        public void OnConfigParsed(Configuration config) {
            Config = config;
            _Config = config;
            
            var bt = config.BulletTracers;
            bt.Beam.Color_T.Red = bt.Beam.Color_T.Red.Clamp(0, 255);
            bt.Beam.Color_T.Green = bt.Beam.Color_T.Green.Clamp(0, 255);
            bt.Beam.Color_T.Blue = bt.Beam.Color_T.Blue.Clamp(0, 255);

            bt.Beam.Color_CT.Red = bt.Beam.Color_CT.Red.Clamp(0, 255);
            bt.Beam.Color_CT.Green = bt.Beam.Color_CT.Green.Clamp(0, 255);
            bt.Beam.Color_CT.Blue = bt.Beam.Color_CT.Blue.Clamp(0, 255);
        }
        
        public override void Load(bool hotReload) {
            pm = new PluginManager(this);
            pluginInfo = new PluginInfo(ModuleName, ModuleVersion, ModuleAuthor, ModuleDescription);

            pm.Register(new Dknife(this));
            pm.Register(new BulletTracers(this));
            pm.Register(new Help(this));
            pm.Register(new MainCommand(this));
            pm.Register(new ItemShop(this));
            pm.Register(new DisablePing(this));
            pm.Register(new DisableRadio(this));
            pm.Register(new DisableChatWheel(this));
            pm.Register(new DisabledBroadcasts(this));
            pm.Register(new AlwaysEmptyWeapons(this));
            pm.Register(new Advertisements(this));
            pm.Register(new CustomMessages(this));
            pm.Register(new ChatFormatter(this));
            pm.Register(new DisableScope(this));
            pm.Register(new WelcomeMessage(this));
            pm.Register(new DamageInfo(this));

            Console.WriteLine(" ");
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――");
            Console.WriteLine(" ___ _                  ___                _   _      _    ");
            Console.WriteLine("/ __| |_  __ _ _ _ _ __| __|______ ___ _ _| |_(_)__ _| |___");
            Console.WriteLine("\\__ \\ ' \\/ _` | '_| '_ \\ _|(_-<_-</ -_) ' \\  _| / _` | (_-<");
            Console.WriteLine("|___/_||_\\__,_|_| | .__/___/__/__/\\___|_||_\\__|_\\__,_|_/__/");
            Console.WriteLine("                  |_|                                      ");
            Console.WriteLine("Version: " + pluginInfo.version);
            Console.WriteLine("Author: " + pluginInfo.author);
            Console.WriteLine("Website: " + pluginInfo.website);
            Console.WriteLine("Project: " + pluginInfo.project);
            pm.printStats();
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――――");
            Console.WriteLine(" ");

        }

        public override void Unload(bool hotReload) {
            pm.unload();
            PlayerExtension.cleanBeams();
        }

    }
}
