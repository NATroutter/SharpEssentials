using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class WelcomeMessageConf(bool enabled) : ToggleableFeature(enabled) {

        public CommandConfig Command { get; set; } = new CommandConfig(
            true,
            ["welcome_test"],
            "Sends the welcome message",
            true,
            "@sess/welcome_test",
            false,
            10,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );

        public int MillsecondsBeforeSending { get; set; } = 500;
        public bool AfterTeamJoin { get; set; } = true;
        public List<string> Message { get; set; } = new List<string>() {
            " ",
            "{GREEN}| Welcome to server |",
            "{GREEN}• {GREY}Website: {LIME}https://example.com",
            "{GREEN}• {GREY}Store: {LIME}https://store.example.com",
            "{GREEN}• {GREY}Statistics: {LIME}https://stats.example.com",
            "{GREEN}• {GREY}Discord: {LIME}https://discord.example.com",
            " ",
            "{GREEN}• {GREY}If you need help with anything type {LIME}!help",
            " "
        };

    }
}
