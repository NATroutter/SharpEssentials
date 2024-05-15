using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class BulletTracerPerms {
        
        public bool UsePermissions { get; set; } = false;
        public string Toggle { get; set; } = "@sess/bullet_tracers/toggle";
        public string Color { get; set; } = "@sess/bullet_tracers/color";
        public string ColorCustom { get; set; } = "@sess/bullet_tracers/color/custom";
        public string ColorRainbow { get; set; } = "@sess/bullet_tracers/color/rainbow";
        
    }
}
