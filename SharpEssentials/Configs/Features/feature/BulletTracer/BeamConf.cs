using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class BeamConf {

        public ColorConf Color_CT { get; set; } = new ColorConf(0,0,255);
        public ColorConf Color_T { get; set; } = new ColorConf(255, 0, 0);
        public float Width_CT { get; set; } = 2.0f;
        public float Width_T { get; set; } = 2.0f;
        public float Lifetime_CT { get; set; } = 1.0f;
        public float Lifetime_T { get; set; } = 1.0f;

        
    }
}
