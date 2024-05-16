using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageSummary(string? name, DamageEntryHits done, DamageEntryHits taken) {
        public string? name { get; set; } = name;
        public DamageEntryHits done { get; set; } = done;
        public DamageEntryHits taken { get; set; } = taken;
    }
}
