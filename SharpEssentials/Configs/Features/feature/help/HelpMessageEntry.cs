using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class HelpMessageEntry {

        public string Permission { get; set; }
        public List<string> Lines { get; set; }

        public HelpMessageEntry() { }

        public HelpMessageEntry(string permission, List<string> lines) {
            this.Permission = permission;
            this.Lines = lines;
        }

    }
}
