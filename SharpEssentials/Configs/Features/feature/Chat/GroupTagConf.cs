using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class GroupTagConf {

        public int priority { get; set; }
        public string group { get; set; }
        public string tag { get; set; }

        public GroupTagConf() { }

        public GroupTagConf(int priority, string group, string tag) {
            this.priority = priority;
            this.group = group;
            this.tag = tag;
        }

    }
}
