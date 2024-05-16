using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class DamageInfoLang {

        public string ChatFormat { get; set; } = "{GREEN}Damaged » {LIME}{NAME} {GREY}| To: {LIME}{HITBOX}{GREY} | {LIME}-{HEALTH} {GREY}HP | {LIME}-{ARMOR} {GREY}Armor";
        public string ConsoleFormat { get; set; } = "Damaged: {NAME} | -{HEALTH} Hp | -{ARMOR} Armor | HitBox: {HITBOX}";

        public DamageInfoSummaryFormat Summary { get; set;} = new DamageInfoSummaryFormat();
        public HitBoxes HitBoxes { get; set; } = new HitBoxes();
    }

}
