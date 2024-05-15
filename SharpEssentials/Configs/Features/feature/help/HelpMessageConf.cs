using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class HelpMessageConf {

        public List<string> Start { get; set; } = new List<string>() {
            " ",
            "{GREEN}Help Message:",
        };
        public List<string> End { get; set; } = new List<string>() {
            " "
        };

        public List<HelpMessageEntry> Lines { get; set; } = new List<HelpMessageEntry>() {
            new HelpMessageEntry("@sess/help/general",
                [
                    "{GREEN}• {LIME}!help general {GREY}- Show general help commands!"
                ]
            ),
            new HelpMessageEntry("@sess/help/bullettracer",
                [
                    "{GREEN}• {LIME}!help bullettracer {GREY}- Show bullet tracer help!"
                ]
            ),
            new HelpMessageEntry("@sess/help/example",
                [
                    "{GREEN}• {LIME}!help example {GREY}- Show surf related commands!"
                ]
            )
        };

    }
}
