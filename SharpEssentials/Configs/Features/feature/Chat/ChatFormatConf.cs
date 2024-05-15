using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class ChatFormatConf(bool enabled) : ToggleableFeature(enabled) {

        public bool DisableTeamChat { get; set; } = false;
        public bool TeamToGlobalChat { get; set; } = false;
        public string Format_All { get; set; } = "{GREY}{DEATH} {GROUP} {NAME}: {WHITE}{MESSAGE}";
        public string Format_Team_CT { get; set; } = "{GREY}{DEATH} {TEAM} {GROUP} {NAME}: {WHITE}{MESSAGE}";
        public string Format_Team_T { get; set; } = "{GREY}{DEATH} {TEAM} {GROUP} {NAME}: {WHITE}{MESSAGE}";
        public string Format_Team_Spec { get; set; } = "{GREY}{DEATH} {TEAM} {GROUP} {NAME}: {WHITE}{MESSAGE}";
        public List<string> CommandTriggers { get; set; } = new List<string>() {
            "!", "/"
        };
        public TagConf Tags { get; set; } = new TagConf();

        public List<GroupTagConf> GroupTags { get; set; } = new List<GroupTagConf> {
            new GroupTagConf(100, "#css/admin", "{RED}[ADMIN]{GREY}"),
            new GroupTagConf(90, "#css/mod", "{LIGHTBLUE}[MOD]{GREY}")
        };
        public bool UseBadWordFilter { get; set; } = true;
        public bool useReplaceMode { get; set; } = true;
        public bool IgnoreCase { get; set; } = true;
        public List<string> BadWords { get; set; } = new List<string>() {
            "fuck",
            "faggot",
            "dick",
            "cock",
            "penis",
            "pussy",
            "nigger",
            "nigga"
        };

    }
}
