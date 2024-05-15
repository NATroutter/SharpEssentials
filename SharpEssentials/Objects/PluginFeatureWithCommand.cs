using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;

namespace SharpEssentials {
    public abstract class PluginFeatureWithCommand : PluginFeature {

        private Cooldown cooldown;
        
        public abstract CommandConfig GetConfig();

        public PluginFeatureWithCommand(SharpEssentials plugin) : base(plugin) {
            cooldown = new Cooldown(GetConfig().CooldownTime, GetConfig().CooldownMessage.Tags());
        }

        public virtual void OnCommand(CCSPlayerController player, CommandInfo command) {}

        public virtual void OnServerConsoleCommand(CommandInfo info) {
            info.Reply("That command can only be used ingame!");
        }

        public virtual void DestroyCooldown() {
            cooldown.Destroy();
        }
        public virtual void DisposeCooldown(CCSPlayerController player) {
            cooldown.Dispose(player);
        }
        public virtual bool hasCooldown(CCSPlayerController? player) {
            return GetConfig().UseCooldown && player.IsLegal() && cooldown.Has(player);
        }
        
    }
}
