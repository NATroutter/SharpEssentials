using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;
using System.Numerics;

namespace SharpEssentials {
    public class ItemShop(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {
        public override CommandConfig GetConfig() {
            return config.ItemShop.Command;
        }

        public override bool IsEnabled() {
            return config.ItemShop.Enabled;
        }

        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            if(!player.IsLegalAlive()) player.Send(lang.OnlyAlive);

            var cfg = config.ItemShop;
            ShowMainMenu(plugin, player);
        }

        [Obsolete]
        public void ShowMainMenu(BasePlugin plugin, CCSPlayerController player) {
            var cfg = config.ItemShop;

            CenterHtmlMenu menu = new CenterHtmlMenu(cfg.MenuTitle);

            if(cfg.Categories.Count() > 0) {
                int cateCount = 0;
                if(cfg.SingleMenuMode) {

                    foreach(var cate in cfg.Categories) {
                        foreach(var item in cate.Items) {
                            cateCount++;
                            menu.AddMenuOption(NameWithPrice(player, item), onSelect: (player, menu) => buyItem(player, item));
                        }
                    }
                    
                } else {
                    foreach(var cate in cfg.Categories) {
                        if(cfg.UsePermissions) {
                            if(player.HasPermission(cate.Permission)) {
                                cateCount++;
                                menu.AddMenuOption(cate.CategoryName, onSelect: (player, menu) => ShowItemMenu(plugin, player, cate));
                            }
                        } else {
                            cateCount++;
                            menu.AddMenuOption(cate.CategoryName, onSelect: (player, menu) => ShowItemMenu(plugin, player, cate));
                        }
                    }
                }

                if(cateCount > 0) {
                    MenuManager.OpenCenterHtmlMenu(plugin, player, menu);
                } else {
                    player.Send(lang.NoItemShopCategories);
                }

            } else {
                player.Send(lang.NoItemShopCategories);
            }
        }

        [Obsolete]
        public void ShowItemMenu(BasePlugin plugin, CCSPlayerController player, ItemShopCategory category) {
            CenterHtmlMenu menu = new CenterHtmlMenu(category.MenuTitle);
            foreach(var item in category.Items) {
                menu.AddMenuOption(NameWithPrice(player, item), onSelect: (player, menu) => buyItem(player, item));
            }
            MenuManager.OpenCenterHtmlMenu(plugin, player!, menu);
        }
        
        public string NameWithPrice(CCSPlayerController player, ItemConf item) {
            return item.Name.ReplaceIgnoreCase("{PRICE}", PriceWithModifier(player, item));
        }

        public int PriceWithModifier(CCSPlayerController player, ItemConf item) {
            var cfg = config.ItemShop;
            if(!cfg.UsePermissions) return item.Price;
            double finalModifier = 1;
            foreach(PriceModifier priceModifier in cfg.PriceModifiers) {
                if(player.HasPermission(priceModifier.Permission)) {
                    if(cfg.PreferLowerModifier) {
                        if(finalModifier > priceModifier.Modifier) finalModifier = priceModifier.Modifier;
                    } else {
                        if(finalModifier < priceModifier.Modifier) finalModifier = priceModifier.Modifier;
                    }
                }
            }
            int price = (int)Math.Round(finalModifier * item.Price);
            return price;
        }

        public void buyItem(CCSPlayerController player, ItemConf item) {
            var price = PriceWithModifier(player, item);
            
            if(price > 0) {
                int bal = player.GetBalance();
                if(bal >= price) {
                    player.setBalance(bal - price);
                    GiveItem(player, item);
                    MenuManager.CloseActiveMenu(player);

                    string msg = lang.YouPurchasedItem;
                    msg = msg.ReplaceIgnoreCase("{ITEM}", item.Name);
                    msg = msg.ReplaceIgnoreCase("{PRICE}", price);
                    player.Send(msg);
                } else {
                    player.Send(lang.NotEnoughtMoney);
                }
            } else {
                GiveItem(player, item);
                string msg = lang.YouPurchasedItemFree;
                msg = msg.ReplaceIgnoreCase("{ITEM}", item.Name);
                player.Send(msg);
                MenuManager.CloseActiveMenu(player);
            }
        }

        public void GiveItem(CCSPlayerController player, ItemConf item) {
            player.GiveNamedItem(item.Item);
            if(config.AlwaysEmptyWeapons.Enabled) {
                Server.NextFrame(() => {
                    var weapon = player.FindWeapon(item.Item);
                    if(weapon == null) return;
                    weapon.SetAmmo(0, 0);
                });
            }
        }

    }
}
