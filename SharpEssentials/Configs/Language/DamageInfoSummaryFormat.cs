using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageInfoSummaryFormat {

        public List<string> Header { get; set; } = [
            " ",
            "{GREEN}▬▬▬▬▬▬▬▬ Summary ▬▬▬▬▬▬▬▬"
            ];
        public List<string> Footer { get; set; } = [
            "{GREEN}▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬",
            " ",
            ];
        public List<string> Entry { get; set; } = [
            "{GREEN}#{NUM} {GREY}- {LIME}{NAME} {GREY}| From: {LIME}{HP}{GREY}/{LIME}{ARMOR} {GREY}In {LIME}{HITS} {GREY}| To: {LIME}{ENEMY_HP}{GREY}/{LIME}{ENEMY_ARMOR} {GREY}In {LIME}{ENEMY_HITS}"
        ];

    }
}
