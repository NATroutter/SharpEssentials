using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageEntry(int victimSlot, string? victimName, int healthDmg, int armorDmg) {
        public int victimSlot { get; set; } = victimSlot;
        public string? victimName { get; set; } = victimName;
        public int hp { get; set; } = healthDmg;
        public int armor { get; set; } = armorDmg;
    }
}
