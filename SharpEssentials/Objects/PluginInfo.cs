using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharpEssentials {
    public class PluginInfo {

        public string name { get; }
        public string version { get; }
        public string author { get; }
        public string description { get; }
        public string project { get; }
        public string website { get; }

        public PluginInfo(string name, string version, string author, string description) {
            this.name = name;
            this.version = version;
            this.author = author;
            this.description = description;
            project = "https://github.com/NATroutter/" + name;
            website = "https://NATroutter.fi";
        }

    }
}
