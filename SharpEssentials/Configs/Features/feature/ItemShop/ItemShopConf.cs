using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class ItemShopConf(bool enabled) : ToggleableFeature(enabled) {

        public CommandConfig Command { get; set; } = new CommandConfig(
            true,
            ["shop"],
            "Opens gun menu",
            false,
            "@sess/itemshop",
            true,
            10,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );

        public string MenuTitle { get; set; } = "Choose a category";
        public bool SingleMenuMode { get; set; } = false;
        public bool UsePermissions { get; set; } = false;

        public bool PreferLowerModifier { get; set; } = true;
        public List<PriceModifier> PriceModifiers { get; set; } = new List<PriceModifier>() {
            new PriceModifier("@sess/itemshop/vip", 0.9),
            new PriceModifier("@sess/itemshop/vip2", 0.5)
        };
        
        public List<ItemShopCategory> Categories { get; set; } = new List<ItemShopCategory>() {
            new ItemShopCategory(
                "Pistols",
                "Choose a pistol!",
                "@sess/itemshop/pistols",
                [
                    new ItemConf("Glock-18 [${PRICE}]", "weapon_glock", 100),
                    new ItemConf("P2000 [${PRICE}]", "weapon_hkp2000", 100),
                    new ItemConf("USP-S [${PRICE}]", "weapon_usp_silencer", 100),
                    new ItemConf("Dual Berettas [${PRICE}]", "weapon_elite", 100),
                    new ItemConf("P250 [${PRICE}]", "weapon_p250", 100),
                    new ItemConf("Tec-9 [${PRICE}]", "weapon_tec9", 100),
                    new ItemConf("Five-SeveN [${PRICE}]", "weapon_fiveseven", 100),
                    new ItemConf("CZ75-Auto [${PRICE}]", "weapon_cz75a", 100),
                    new ItemConf("Desert Eagle [${PRICE}]", "weapon_deagle", 100),
                    new ItemConf("R8-Revolver [${PRICE}]", "weapon_revolver", 100)
                ]
            ),
            new ItemShopCategory(
                "Rifles",
                "Choose a rifle!",
                "@sess/itemshop/rifles",
                [
                    new ItemConf("Galil AR [${PRICE}]", "weapon_galilar", 100),
                    new ItemConf("FAMAS [${PRICE}]", "weapon_famas", 100),
                    new ItemConf("AK-47 [${PRICE}]", "weapon_ak47", 100),
                    new ItemConf("M4A4 [${PRICE}]", "weapon_m4a1", 100),
                    new ItemConf("M4A1-S [${PRICE}]", "weapon_m4a1_silencer", 100),
                    new ItemConf("AUG [${PRICE}]", "weapon_aug", 100),
                    new ItemConf("SG 553 [${PRICE}]", "weapon_sg556", 100)
                ]
            ),
            new ItemShopCategory(
                "Smgs",
                "Choose a smg!",
                "@sess/itemshop/smgs",
                [
                    new ItemConf("MP5-SD [${PRICE}]", "weapon_mp5sd", 100),
                    new ItemConf("P90 [${PRICE}]", "weapon_p90", 100),
                    new ItemConf("MP7 [${PRICE}]", "weapon_mp7", 100),
                    new ItemConf("MAC-10 [${PRICE}]", "weapon_mac10", 100),
                    new ItemConf("MP9 [${PRICE}]", "weapon_mp9", 100),
                    new ItemConf("PP-Bizon [${PRICE}]", "weapon_bizon", 100),
                    new ItemConf("UMP-45 [${PRICE}]", "weapon_ump45", 100)
                ]
            ),
            new ItemShopCategory(
                "Snipers",
                "Choose a sniper!",
                "@sess/itemshop/snipers",
                [
                    new ItemConf("SSG 08 [${PRICE}]", "weapon_ssg08", 100),
                    new ItemConf("AWP [${PRICE}]", "weapon_awp", 100),
                    new ItemConf("G3SG1 [${PRICE}]", "weapon_g3sg1", 100),
                    new ItemConf("SCAR-20 [${PRICE}]", "weapon_scar20", 100)
                ]
            ),
            new ItemShopCategory(
                "Heavy",
                "Choose a heavy!",
                "@sess/itemshop/heavy",
                [
                    new ItemConf("Nova [${PRICE}]", "weapon_nova", 100),
                    new ItemConf("MAG-7 [${PRICE}]", "weapon_mag7", 100),
                    new ItemConf("Sawed-Off [${PRICE}]", "weapon_sawedoff", 100),
                    new ItemConf("XM1014 [${PRICE}]", "weapon_xm1014", 100),
                    new ItemConf("M249 [${PRICE}]", "weapon_m249", 100),
                    new ItemConf("Negev [${PRICE}]", "weapon_negev", 100)
                ]
            ),
            new ItemShopCategory(
                "Special",
                "Choose a special item!",
                "@sess/itemshop/special",
                [
                    new ItemConf("Healthshot [${PRICE}]", "weapon_healthshot", 1000)
                ]
            )
        };

    }
}
