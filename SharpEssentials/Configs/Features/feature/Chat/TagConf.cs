using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class TagConf {

        public string Death { get; set; } = "*DEATH*";
        public string TeamCT { get; set; } = "[CT]";
        public string TeamT { get; set; } = "[T]";
        public string TeamSpec { get; set; } = "[SPEC]";

    }
}
