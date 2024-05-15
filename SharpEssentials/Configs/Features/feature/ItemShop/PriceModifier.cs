using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials{
    public class PriceModifier {

        public string Permission { get; set; }
        public double Modifier { get; set; }


        public PriceModifier() { }
        public PriceModifier(string permission, double multiplier) {
            this.Permission = permission;
            this.Modifier = multiplier;
        }

    }
}
