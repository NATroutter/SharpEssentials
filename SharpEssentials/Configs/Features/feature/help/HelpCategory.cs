using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class HelpCategory {

        public HelpCategory() { }

        public HelpCategory(string name, string permission, List<string> message) {
            this.Name = name;
            this.Permission = permission;
            this.Message = message;
        }

        public string Name { get; set; }
        public string Permission { get; set; }
        public List<string> Message { get; set; } = new List<string>();


    }
}
