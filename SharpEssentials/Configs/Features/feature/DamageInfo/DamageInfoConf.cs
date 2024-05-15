using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageInfoConf(bool enabled) : ToggleableFeature(enabled) {

        public CommandConfig Command { get; set; } = new CommandConfig(
            true,
            ["damageinfo"],
            "Toggle damage info printing on/off",
            false,
            "@sess/damageinfo",
            false,
            10,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );

        public PrintModeConf PrintWhenDamaged { get; set; } = new PrintModeConf();
        public PrintModeConf RoundEndSummary { get; set; } = new PrintModeConf();

        public bool FriendlyFireMode { get; set; } = false;
        public bool IncludeBots { get; set; } = false;

    }
}
