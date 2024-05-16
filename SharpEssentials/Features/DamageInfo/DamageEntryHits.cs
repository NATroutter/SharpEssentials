using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageEntryHits(int victimSlot, string? VictimName, int healthDmg, int armorDmg, int hits) : DamageEntry(victimSlot, VictimName, healthDmg, armorDmg) {
        public int hits { get; set; } = hits;
    }
}
