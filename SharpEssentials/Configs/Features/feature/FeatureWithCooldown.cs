using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class FeatureWithCooldown : ToggleableFeature {

        public FeatureWithCooldown(bool enabled, bool useCooldown, int cooldown) : base(enabled) {
            UseCooldown = useCooldown;
            Cooldown = cooldown;
        }

        public bool UseCooldown { get; set; }
        public int Cooldown { get; set; }
    }
}
