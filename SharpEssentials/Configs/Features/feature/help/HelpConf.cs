using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpEssentials{
    public class HelpConf(bool enabled) : ToggleableFeature(enabled) {


        public bool UsePermission { get; set; } = false;
        
        public CommandConfig Command { get; set; } = new CommandConfig(
            true,
            [ "help" ],
            "Print servers custom help message!",
            false,
            "@sess/help",
            true,
            10,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );

        public bool SinglePageMode { get; set; } = false;
        public HelpMessageConf HelpMessage { get; set; } = new HelpMessageConf();

        public List<HelpCategory> Categories { get; set; } = new List<HelpCategory>() {
            new HelpCategory("general","@sess/help/general",
                [
                    " ",
                    "{GREEN}General help:",
                    "{GREEN}• {LIME}!sess {GREY}- Shows information about plugin!",
                    "{GREEN}• {LIME}!dknife {GREY}- Gives default knife",
                ]
            ),
            new HelpCategory("bullettracer","@sess/help/bullettracer",
                [
                    " ",
                    "{GREEN}BulletTracer help:",
                    "{GREEN}• {LIME}!bt {GREY}- Shows bullet tracer settings"
                ]
            ),
            new HelpCategory("example","@sess/help/example",
                [
                    " ",
                    "{GREEN}Example help:",
                    "{GREEN}• {LIME}!command {GREY}- Description"
                ]
            ),
        };


    }
}
