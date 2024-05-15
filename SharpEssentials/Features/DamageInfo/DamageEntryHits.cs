using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageEntryHits(ulong victimID, int healthDmg, int armorDmg, int hits) : DamageEntry(victimID, healthDmg, armorDmg) {
        public int hits { get; set; } = hits;
    }
}
