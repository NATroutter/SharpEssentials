using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class ToggleableFeature(bool enabled) {
  
        public bool Enabled { get; set; } = enabled;

    }
}
