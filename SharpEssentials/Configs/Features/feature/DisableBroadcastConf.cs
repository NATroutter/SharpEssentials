using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DisableBroadcastConf() {

        public bool BombPlantAnouncement { get; set; } = false;
        public bool WinOrLoseSound { get; set; } = false;
        public bool WinOrLosePanel { get; set; } = false;
        public bool MVPSound { get; set; } = false;
        public bool DefaultDisconnectMessage { get; set; } = true;
        public bool DefaultTeamJoinMessage { get; set; } = true;
        public bool KillFeed { get; set; } = false;
    }
}
