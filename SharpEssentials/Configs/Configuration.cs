using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class Configuration : BasePluginConfig {

        public string GlobalBypassPermission { get; set; } = "@css/root";

        public FeatureWithCooldown DisablePing { get; set; } = new FeatureWithCooldown(true, true, 10);
        public FeatureWithCooldown DisableRadio { get; set; } = new FeatureWithCooldown(true, true, 10);
        public FeatureWithCooldown DisableChatWheel { get; set; } = new FeatureWithCooldown(true, true, 10);

        public ToggleableFeature AlwaysEmptyWeapons { get; set; } = new ToggleableFeature(false);

        public DisableBroadcastConf DisableBroadcast { get; set; } = new DisableBroadcastConf();

        public CustomMessagesConf CustomMessages { get; set; } = new CustomMessagesConf(true);

        public AdvertisementsConf Advertisements { get; set; } = new AdvertisementsConf(true);

        public ChatFormatConf ChatFormat { get; set; } = new ChatFormatConf(true);

        public DisableScopeConf DisableScope { get; set; } = new DisableScopeConf();

        public BulletTracersConf BulletTracers { get; set; } = new BulletTracersConf(true);

        public HelpConf Help { get; set; } = new HelpConf(true);

        public CommandConfig Dknife { get; set; } = new CommandConfig(
            true,
            [ "dknife", "london" ],
            "gives default knife",
            false,
            "@sess/dknife",
            true,
            10,
            "{PREFIX} This command is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds"
        );

        public ItemShopConf ItemShop { get; set; } = new ItemShopConf(true);

        public WelcomeMessageConf WelcomeMessage { get; set; } = new WelcomeMessageConf(true);

        public DamageInfoConf DamageInfo { get; set; } = new DamageInfoConf(true);

        public Language Language { get; set; } = new Language();

    }
}
