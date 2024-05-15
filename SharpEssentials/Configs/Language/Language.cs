using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class Language {

        public string Prefix { get; set; } = "{GREEN}Server » {GREY}";
        public string TooManyArgs { get; set; } = "{PREFIX} Too many arguments, type {LIME}!help";
        public string NoPermission { get; set; } = "{PREFIX} You don't have permissions to do that!";
        public string JoinMessage { get; set; } = "{PREFIX} {LIME}{NAME} {GREY}has connected!";
        public string QuitMessage { get; set; } = "{PREFIX} {LIME}{NAME} {GREY}has disconnected!";
        public string JoinTeam_T { get; set; } = "{PREFIX} {LIME}{NAME} {GREY}is joining the {LIME}Terrorists";
        public string JoinTeam_CT { get; set; } = "{PREFIX} {LIME}{NAME} {GREY}is joining the {LIME}Counter-Terrorists";
        public string JoinTeam_SPEC { get; set; } = "{PREFIX} {LIME}{NAME} {GREY}is joining the {LIME}Spectators";
        public string RadioCooldown { get; set; } = "{PREFIX} This action is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds";
        public string ChatWheelCooldown { get; set; } = "{PREFIX} This action is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds";
        public string PingCooldown { get; set; } = "{PREFIX} This action is on cooldown for {LIME}{H} {GREY}hours {LIME}{M} {GREY}minutes {LIME}{S} {GREY}seconds";
        public string KnifeAlreadyInBelt { get; set; } = "{PREFIX} You already have knife in your belt!";
        public string KnifeAddedToBelt { get; set; } = "{PREFIX} Default knife added to your belt!";
        public string TeamChatIsDisabled { get; set; } = "{PREFIX} Team chat is disabled!";
        public string MessageWithBadWords { get; set; } = "{PREFIX} Your message contains bad words ({WORD}), Please dont use them.";
        public string ScopingNotAllowed { get; set; } = "{PREFIX} You are not allowed to scope!";
        public string TracerColorChanged { get; set; } = "{PREFIX} Bullet tracer color has been changed to {LIME}{COLOR}";
        public string TracerToggled { get; set; } = "{PREFIX} Bullet tracer has been {LIME}{STATE}";
        public string TracerCustomColorToggled { get; set; } = "{PREFIX} Bullet tracer custom colors has been {LIME}{STATE}";
        public string NotAllowedToUse { get; set; } = "{PREFIX} You are not allowed to use this feature!";
        public string CategoryNotExists { get; set; } = "{PREFIX} That category does not exists";
        public string OnlyAlive { get; set; } = "{PREFIX} This feature can only be used wehen alive!";
        public string NoItemShopCategories { get; set; } = "{PREFIX} There are no categories!";
        public string NotEnoughtMoney { get; set; } = "{PREFIX} You don't have enought money!";
        public string YouPurchasedItem { get; set; } = "{PREFIX} You have pruchased {LIME}{ITEM}{GREY} for ${LIME}{PRICE}";
        public string YouPurchasedItemFree { get; set; } = "{PREFIX} You got {LIME}{ITEM}{GREY}";
        public string DamageInfoChatFormat { get; set; } = "{PREFIX} You got {LIME}{ITEM}{GREY}";



        public ToggleStatesLang ToggleStates {  get; set; } = new ToggleStatesLang();
        public ColorsLang Colors { get; set; } = new ColorsLang();


    }
}
