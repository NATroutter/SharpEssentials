using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class AdvertisementsConf(bool enabled) : ToggleableFeature(enabled) {

        public bool RandomOrder { get; set; } = false;
        public int Interval { get; set; } = 120;
        public List<string> Messages { get; set; } = new List<string>() {
            "{GREEN}Broadcast » {GREY}Type {LIME}!help {GREY}for list of commands/information",
            "{GREEN}Broadcast » {GREY}Visit server website at {LIME}https://example.com",
            "{GREEN}Broadcast » {GREY}Buy vip access now at {LIME}https://shop.example.com"
        };

    }
}
