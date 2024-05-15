using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageEntry(ulong victimID, int healthDmg, int armorDmg) {
        public ulong victimID { get; set; } = victimID;
        public int hp { get; set; } = healthDmg;
        public int armor { get; set; } = armorDmg;
    }
}
