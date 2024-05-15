using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class BulletTracersConf(bool enabled) : ToggleableFeature(enabled) {

        public CommandConfig Command { get; set; } = new CommandConfig(
            true,
            new List<string> { "bt", "tracer" },
            "Opens bullet tracer settings",
            false,
            "@sess/bullet_tracers",
            true,
            5,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );
        
        public bool AllowToggle { get; set; } = true;
        public bool AllowColorChange { get; set; } = true;
        public List<string> TracedWeapons { get; set; } = new List<string>() {
            "weapon_awp", "weapon_ssg08", "weapon_aug",
            "weapon_sg556","weapon_g3sg1","weapon_scar20"
        };

        public BeamConf Beam { get; set; } = new BeamConf();
        public BulletTracerEnabledColors EnabledColors { get; set; } = new BulletTracerEnabledColors();

        public int BeamOriginVerticalOffset {get; set;} = 70;
        public bool UseMultiImpactFix { get; set; } = true;
        
        public BulletTracerPerms Permissions { get; set; } = new BulletTracerPerms();
        
    }
}
