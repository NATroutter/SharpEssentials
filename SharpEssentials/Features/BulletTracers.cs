using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace SharpEssentials {
    public class BulletTracers(SharpEssentials plugin) : PluginFeatureWithCommand(plugin) {

        public override CommandConfig GetConfig() {
            return config.BulletTracers.Command;
        }

        public override bool IsEnabled() {
            return config.BulletTracers.Enabled;
        }

        private Dictionary<ulong, bool> activeUsers = new Dictionary<ulong, bool>();
        private Dictionary<ulong, bool> activeCustomUsers = new Dictionary<ulong, bool>();
        private Dictionary<ulong, bool> activeRainbowUsers = new Dictionary<ulong, bool>();
        private Dictionary<ulong, Color> userColors = new Dictionary<ulong, Color>();
        private Dictionary<ulong, bool> bulletFired = new Dictionary<ulong, bool>();
        private Dictionary<ulong, int> rainbowIndex = new Dictionary<ulong, int>();

        private List<Color> RainbowColors = new List<Color>() {
            Color.Red, Color.OrangeRed, Color.Yellow, Color.Green, Color.Blue, Color.BlueViolet,Color.Magenta
        };
        
        public override void OnCommand(CCSPlayerController player, CommandInfo command) {
            if(!player.IsLegalAlive()) player.Send(lang.OnlyAlive);

            if(command.ArgCount == 1) {

                if(!activeUsers.ContainsKey(player.SteamID)) {
                    activeUsers.Add(player.SteamID, true);
                }
                if(!activeCustomUsers.ContainsKey(player.SteamID)) {
                    activeCustomUsers.Add(player.SteamID, false);
                }

                if(!config.BulletTracers.AllowToggle && !config.BulletTracers.AllowColorChange) {
                    command.Reply(lang.NotAllowedToUse);
                    return;
                }

                ShowMainMenu(plugin, player);

            } else {
                command.Reply(lang.TooManyArgs);
            }
        }

        public override void Load() {
            plugin.RegisterEventHandler<EventBulletImpact>((@event, info) => {
                var cfg = config.BulletTracers;


                CCSPlayerController? player = @event.Userid;
                if(!player.IsLegalAlive()) return HookResult.Continue;

                if(cfg.UseMultiImpactFix) {
                    if(!bulletFired.ContainsKey(player.SteamID)) bulletFired.Add(player.SteamID, false);
                    if(bulletFired[player.SteamID]) return HookResult.Continue;
                    bulletFired[player.SteamID] = true;

                    plugin.RunDelayed(10, () => {
                        if (player.IsConnected()) {
                            ulong id = player.SteamID;
                            if(!bulletFired.ContainsKey(id)) bulletFired.Add(id, false);
                            bulletFired[id] = false;
                        }
                    });
                }

                var weaponService = player.PlayerPawn.Value?.WeaponServices;
                if(weaponService == null) return HookResult.Continue;

                var activeWeapon = weaponService.ActiveWeapon.Value;
                if(activeWeapon == null) return HookResult.Continue;

                if(!activeUsers.ContainsKey(player.SteamID)) {
                    activeUsers.Add(player.SteamID, true);
                }
                if(!activeCustomUsers.ContainsKey(player.SteamID)) {
                    activeCustomUsers.Add(player.SteamID, false);
                }
                if(!activeRainbowUsers.ContainsKey(player.SteamID)) {
                    activeRainbowUsers.Add(player.SteamID, false);
                }

                if(weaponService.MyWeapons.Count != 0 && activeUsers[player.SteamID]) {
                    try {
                        string weaponName = activeWeapon.DesignerName;

                        if(cfg.TracedWeapons.Contains(weaponName)) {

                            Vector? playerPos = player.GetPosition();
                            if(playerPos == null) return HookResult.Continue;

                            Vector origin = new Vector(playerPos.X, playerPos.Y, playerPos.Z + cfg.BeamOriginVerticalOffset);
                            Vector destination = new Vector(@event.X, @event.Y, @event.Z);

                            Color defaultColor = player.IsCt() ? cfg.Beam.Color_CT.GetColor() : cfg.Beam.Color_T.GetColor();
                            float defaultWidth = player.IsCt() ? cfg.Beam.Width_CT : cfg.Beam.Width_T;
                            float defaultLife = player.IsCt() ? cfg.Beam.Lifetime_CT : cfg.Beam.Lifetime_T;

                            if(activeCustomUsers[player.SteamID]) {
                                if(activeRainbowUsers[player.SteamID]) {

                                    player.DrawLaserBetween(plugin, origin, destination, GetNextRainbowEntry(player), defaultLife, defaultWidth);

                                } else {

                                    if(!userColors.ContainsKey(player.SteamID)) {
                                        userColors.Add(player.SteamID, Color.White);
                                    }
                                    Color customColor = userColors[player.SteamID];
                                    player.DrawLaserBetween(plugin, origin, destination, customColor, defaultLife, defaultWidth);

                                }
                            } else {
                                player.DrawLaserBetween(plugin, origin, destination, defaultColor, defaultLife, defaultWidth);
                            }
                            
                        }
                    } catch(Exception ex) {
                        Logger.error("[BulletTracer] "+ex.ToString());
                    }
                }
                return HookResult.Continue;
            });
        }

        public override void OnDisconnect(CCSPlayerController player) {
            activeUsers.Remove(player.SteamID);
            activeCustomUsers.Remove(player.SteamID);
            activeRainbowUsers.Remove(player.SteamID);
            userColors.Remove(player.SteamID);
            bulletFired.Remove(player.SteamID);
            rainbowIndex.Remove(player.SteamID);
        }

        #region Functions
        public string currentColorString(CCSPlayerController player) {
            if(!userColors.ContainsKey(player.SteamID)) {
                userColors.Add(player.SteamID, Color.White);
            }
            Color color = userColors[player.SteamID];
            return String.Format("({0},{1},{2})", color.R, color.G, color.B);
        }
        public string toggleState(CCSPlayerController player, Dictionary<ulong, bool> map) {
            if(!map.ContainsKey(player.SteamID)) return "OFF";
            return map[player.SteamID] ? "ON" : "OFF";
        }
        public Color GetNextRainbowEntry(CCSPlayerController player) {
            if(!rainbowIndex.ContainsKey(player.SteamID)) rainbowIndex.Add(player.SteamID, 0);
            Color nextColor = RainbowColors[rainbowIndex[player.SteamID]];
            rainbowIndex[player.SteamID] = (rainbowIndex[player.SteamID] + 1) % RainbowColors.Count;
            return nextColor;
        }
        #endregion

        #region Menus
        [Obsolete]
        public void ShowMainMenu(BasePlugin plugin, CCSPlayerController player) {
            var cfg = config.BulletTracers;

            CenterHtmlMenu menu = new CenterHtmlMenu("BulletTracer settings");
            if(cfg.AllowToggle) {
                if(cfg.Permissions.UsePermissions) {
                    if(player.HasPermission(cfg.Permissions.Toggle)) {
                        menu.AddMenuOption("Toggle [" + toggleState(player, activeUsers) + "]", (player, menu) => ToggleTracers(plugin, player));
                    }
                } else {
                    menu.AddMenuOption("Toggle [" + toggleState(player, activeUsers) + "]", (player, menu) => ToggleTracers(plugin, player));
                }
            }
            if(cfg.AllowColorChange) {
                if(cfg.Permissions.UsePermissions) {
                    if(player.HasPermission(cfg.Permissions.ColorCustom)) {
                        menu.AddMenuOption("Custom Color [" + toggleState(player, activeCustomUsers) + "]", (player, menu) => ToggleCustomColor(plugin, player));
                        menu.AddMenuOption("Change Color", (player, menu) => ShowColorMenu(plugin, player));
                    }
                } else {
                    menu.AddMenuOption("Custom Color [" + toggleState(player, activeCustomUsers) + "]", (player, menu) => ToggleCustomColor(plugin, player));
                    menu.AddMenuOption("Change Color", (player, menu) => ShowColorMenu(plugin, player));
                }
            }
            MenuManager.OpenCenterHtmlMenu(plugin, player!, menu);
        }


        [Obsolete]
        private void ShowColorMenu(BasePlugin plugin, CCSPlayerController player) {
            var cfg = config.BulletTracers;
            var enabled = cfg.EnabledColors;

            if(
                !enabled.Custom && !enabled.Rainbow && !enabled.Red && !enabled.Orange &&
                !enabled.Yellow && !enabled.Lime && !enabled.Green && !enabled.Cyan &&
                !enabled.Blue && !enabled.Violet && !enabled.Magenta && !enabled.White
            ) {
                player.Send(lang.NotAllowedToUse);
                return;
            }

            CenterHtmlMenu menu = new CenterHtmlMenu("Choose color");

            if(enabled.Custom) {
                if(cfg.Permissions.UsePermissions) {
                    if(player.HasPermission(cfg.Permissions.ColorCustom)) {
                        menu.AddMenuOption("Custom", (player, menu) => ShowCustomColorMenu(plugin, player));
                    }
                } else {
                    menu.AddMenuOption("Custom", (player, menu) => ShowCustomColorMenu(plugin, player));
                }
            }
            if(enabled.Rainbow) {
                if(cfg.Permissions.UsePermissions) {
                    if(player.HasPermission(cfg.Permissions.ColorRainbow)) {
                        menu.AddMenuOption("Rainbow", (player, menu) => ActiveRainbow(player));
                    }
                } else {
                    menu.AddMenuOption("Rainbow", (player, menu) => ActiveRainbow(player));
                }
            }

            if(enabled.Red) {
                menu.AddMenuOption("Red", (player, menu) => ChangeColor(plugin, player, Color.Red)); 
            }
            if(enabled.Orange) {
                menu.AddMenuOption("Orange", (player, menu) => ChangeColor(plugin, player, Color.OrangeRed));
            }
            if(enabled.Yellow) {
                menu.AddMenuOption("Yellow", (player, menu) => ChangeColor(plugin, player, Color.Yellow));
            }
            if(enabled.Lime) {
                menu.AddMenuOption("Lime", (player, menu) => ChangeColor(plugin, player, Color.LimeGreen));
            }
            if(enabled.Green) {
                menu.AddMenuOption("Green", (player, menu) => ChangeColor(plugin, player, Color.Green));
            }
            if(enabled.Cyan) {
                menu.AddMenuOption("Cyan", (player, menu) => ChangeColor(plugin, player, Color.Cyan));
            }
            if(enabled.Blue) {
                menu.AddMenuOption("Blue", (player, menu) => ChangeColor(plugin, player, Color.Blue));
            }
            if(enabled.Violet) {
                menu.AddMenuOption("Violet", (player, menu) => ChangeColor(plugin, player, Color.BlueViolet));
            }
            if(enabled.Magenta) {
                menu.AddMenuOption("Magenta", (player, menu) => ChangeColor(plugin, player, Color.Magenta));
            }
            if(enabled.White) {
                menu.AddMenuOption("White", (player, menu) => ChangeColor(plugin, player, Color.White));
            }
            MenuManager.OpenCenterHtmlMenu(plugin, player!, menu);
        }

        [Obsolete]
        private void ShowCustomColorMenu(BasePlugin plugin, CCSPlayerController player) {
            SetActiveUser(player, true);
            SetCustomUser(player, true);

            CenterHtmlMenu menu = new CenterHtmlMenu("Choose color " + currentColorString(player));
            menu.AddMenuOption("Red", (player, menu) => ShowCustomColorModifyMenu(plugin, player, ColorType.RED));
            menu.AddMenuOption("Green", (player, menu) => ShowCustomColorModifyMenu(plugin, player, ColorType.GREEN));
            menu.AddMenuOption("Blue", (player, menu) => ShowCustomColorModifyMenu(plugin, player, ColorType.BLUE));
            MenuManager.OpenCenterHtmlMenu(plugin, player!, menu);
        }
        
        [Obsolete]
        private void ShowCustomColorModifyMenu(BasePlugin plugin, CCSPlayerController player, ColorType type) {
            CenterHtmlMenu menu = new CenterHtmlMenu("Custom: ["+ type.GetName()+ "]" + currentColorString(player));
            menu.AddMenuOption("+1", (player, menu) => ChangeCustomColor(plugin, player, type, 1));
            menu.AddMenuOption("+10", (player, menu) => ChangeCustomColor(plugin, player, type, 10));
            menu.AddMenuOption("-10", (player, menu) => ChangeCustomColor(plugin, player, type, -10));
            menu.AddMenuOption("-1", (player, menu) => ChangeCustomColor(plugin, player, type, -1));
            menu.AddMenuOption("« Go back", (player, menu) => ShowCustomColorMenu(plugin, player));
            MenuManager.OpenCenterHtmlMenu(plugin, player!, menu);
        }
        #endregion

        #region Menu Actions
        [Obsolete]
        public void ToggleTracers(BasePlugin plugin, CCSPlayerController player) {
            ToggleActiveUser(player);

            string msg = lang.TracerToggled;
            msg = msg.Replace("{STATE}", activeUsers[player.SteamID].GetLangName());
            player.PrintToChat(msg.Tags());

            ShowMainMenu(plugin, player);
        }

        [Obsolete]
        public void ToggleCustomColor(BasePlugin plugin, CCSPlayerController player) {
            ToggleCustomUser(player);

            string msg = lang.TracerCustomColorToggled;
            msg = msg.Replace("{STATE}", activeCustomUsers[player.SteamID].GetLangName());
            player.PrintToChat(msg.Tags());

            ShowMainMenu(plugin, player);
        }

        [Obsolete]
        public void ActiveRainbow(CCSPlayerController player) {
            SetCustomUser(player, true);
            SetActiveUser(player, true);
            SetRainbow(player, true);

            string msg = lang.TracerColorChanged;
            msg = msg.Replace("{COLOR}", lang.Colors.Rainbow);

            player.PrintToChat(msg.Tags());

            MenuManager.CloseActiveMenu(player);
        }

        [Obsolete]
        public void ChangeColor(BasePlugin plugin, CCSPlayerController player, Color color) {
            SetUserColor(player, color);
            SetCustomUser(player, true);
            SetActiveUser(player, true);
            SetRainbow(player, false);

            string msg = lang.TracerColorChanged;
            string? langColor = color.getLangName();
            msg = msg.Replace("{COLOR}", (langColor != null ? langColor : "Unknown"));

            player.PrintToChat(msg.Tags());

            MenuManager.CloseActiveMenu(player);
        }

        [Obsolete]
        public void ChangeCustomColor(BasePlugin plugin, CCSPlayerController player, ColorType type, int amount) {
            if(!userColors.ContainsKey(player.SteamID)) {
                userColors.Add(player.SteamID, Color.White);
            }

            SetCustomUser(player, true);
            SetRainbow(player, false);

            Color color = userColors[player.SteamID];

            if(type == ColorType.RED) color.setRed((color.R + amount).Clamp(0, 255));
            if(type == ColorType.GREEN) color.setGreen((color.G + amount).Clamp(0, 255));
            if(type == ColorType.BLUE) color.setBlue((color.B + amount).Clamp(0, 255));

            SetUserColor(player, color);
            ShowCustomColorModifyMenu(plugin, player, type);
        }
        #endregion

        #region Actions

        public void SetActiveUser(CCSPlayerController player, bool value) {
            if(!activeUsers.ContainsKey(player.SteamID)) {
                activeUsers.Add(player.SteamID, value);
            }
            activeUsers[player.SteamID] = value;
        }
        public void ToggleActiveUser(CCSPlayerController player) {
            if(!activeUsers.ContainsKey(player.SteamID)) {
                activeUsers.Add(player.SteamID, true);
            } else {
                activeUsers[player.SteamID] = !activeUsers[player.SteamID];
            }
        }

        public void SetCustomUser(CCSPlayerController player, bool value) {
            if(!activeCustomUsers.ContainsKey(player.SteamID)) {
                activeCustomUsers.Add(player.SteamID, value);
            }
            activeCustomUsers[player.SteamID] = value;
        }
        public void ToggleCustomUser(CCSPlayerController player) {
            if(!activeCustomUsers.ContainsKey(player.SteamID)) {
                activeCustomUsers.Add(player.SteamID, true);
            } else {
                activeCustomUsers[player.SteamID] = !activeCustomUsers[player.SteamID];
            }
        }

        public void SetUserColor(CCSPlayerController player, Color color) {
            if(!userColors.ContainsKey(player.SteamID)) {
                userColors.Add(player.SteamID, color);
            } else {
                userColors[player.SteamID] = color;
            }
        }
        public void SetRainbow(CCSPlayerController player, bool value) {
            if(!activeRainbowUsers.ContainsKey(player.SteamID)) {
                activeRainbowUsers.Add(player.SteamID, value);
            } else {
                activeRainbowUsers[player.SteamID] = value;
            }
        }
        #endregion
    }
}
