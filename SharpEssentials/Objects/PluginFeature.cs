using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace SharpEssentials {
    public abstract class PluginFeature {

        public BasePlugin plugin { get; set; }
        public Configuration config { get; set; }
        public Language lang { get; set; }

        public PluginFeature(SharpEssentials plugin) {
            this.plugin = plugin;
            this.config = plugin.Config;
            this.lang = plugin.Config.Language;
        }

        public abstract bool IsEnabled();

        public virtual void Load() { }

        public virtual void Unload() { }

        public virtual void OnDisconnect(CCSPlayerController player) { }

        public virtual void OnConnected(CCSPlayerController player) { }

    }
}
