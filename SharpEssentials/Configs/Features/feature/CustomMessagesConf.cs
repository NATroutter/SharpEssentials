using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class CustomMessagesConf(bool enabled) : ToggleableFeature(enabled) {

        public bool useJoin { get; set; } = true;
        public bool useQuit { get; set; } = true;
        public bool useTeamJoinT { get; set; } = true;
        public bool useTeamJoinCT { get; set; } = true;
        public bool useTeamJoinSPEC { get; set; } = true;
    }
}
