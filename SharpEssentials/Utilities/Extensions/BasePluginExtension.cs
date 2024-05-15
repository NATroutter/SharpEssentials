
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;

namespace SharpEssentials {
    public static class BasePluginExtension {


        public static void RunDelayed(this BasePlugin plugin, int interval, Action callback) {
            plugin.AddTimer((interval / 1000), () => callback());
        }
        
    }
}
