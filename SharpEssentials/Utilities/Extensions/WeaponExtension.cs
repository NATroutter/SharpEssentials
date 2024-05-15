using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;

public static class WeaponExtension {
    public static bool IsLegal([NotNullWhen(true)] this CBasePlayerWeapon? weapon) {
        return weapon != null && weapon.IsValid;
    }


    public static void SetColour(this CBasePlayerWeapon? weapon, Color colour) {
        if(weapon.IsLegal()) {
            weapon.RenderMode = RenderMode_t.kRenderTransColor;
            weapon.Render = colour;
            Utilities.SetStateChanged(weapon, "CBaseModelEntity", "m_clrRender");
        }
    }

    public static CBasePlayerWeapon? FindWeapon(this CCSPlayerController? player, String name)
    {
        // only care if player is alive
        if(!player.IsLegalAlive())
        {
            return null;
        }

        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn == null)
        {
            return null;
        }

        var weapons = pawn.WeaponServices?.MyWeapons;

        if(weapons == null)
        {
            return null;
        }

        foreach (var weaponOpt in weapons)
        {
            CBasePlayerWeapon? weapon = weaponOpt.Value;

            if(weapon == null)
            {
                continue;
            }
            //Console.WriteLine("W: " + weapon.DesignerName + " - " + weapon.Globalname + " - " + name);

            if(weapon.DesignerName.Contains(name)) {
                return weapon;

            } else if(weapon.DesignerName == "weapon_hkp2000" && name == "weapon_usp_silencer") {
                return weapon;

            } else if(weapon.DesignerName == "weapon_m4a1" && name == "weapon_m4a1_silencer") {
                return weapon;

            } else if(weapon.DesignerName == "weapon_deagle" && name == "weapon_revolver") {
                return weapon;

            }
        }
        return null;
    }
    public static List<CBasePlayerWeapon> FindWeapons(this CCSPlayerController? player) {
        List<CBasePlayerWeapon> weaponList = new List<CBasePlayerWeapon>();

        // only care if player is alive
        if(!player.IsLegalAlive()) {
            return null;
        }

        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn == null) {
            return null;
        }

        var weapons = pawn.WeaponServices?.MyWeapons;

        if(weapons == null) {
            return null;
        }

        foreach(var weaponOpt in weapons) {
            CBasePlayerWeapon? weapon = weaponOpt.Value;

            if(weapon == null) {
                continue;
            }
            weaponList.Add(weapon);
        }
        return weaponList;
    }



    public static bool isKnife(this CBasePlayerWeapon? weapon) {
        if(weapon != null && weapon.IsLegal()) {
            return (weapon.DesignerName.Contains("knife") || weapon.DesignerName.Contains("bayonet"));
        }
        return false;
    }

    public static void SetAmmo(this CBasePlayerWeapon? weapon, int clip, int reserve)
    {
        if(!weapon.IsLegal())
        {
            return;
        }

        // overide reserve max so it doesn't get clipped when
        // setting "infinite ammo"
        // thanks 1Mack
        CCSWeaponBaseVData? weaponData = weapon.As<CCSWeaponBase>().VData;
    
        
        if(weaponData != null)
        {
            // TODO: this overide it for every gun the player has...
            // when not a map gun, this is not a big deal
            // for the reserve ammo it is for the clip though
        /*
            if(clip > weaponData.MaxClip1)
            {
                weaponData.MaxClip1 = clip;
            }
        */
            if(reserve > weaponData.PrimaryReserveAmmoMax)
            {
                weaponData.PrimaryReserveAmmoMax = reserve;
            }
        }
        if(clip != -1)
        {
            weapon.Clip1 = clip;
            Utilities.SetStateChanged(weapon,"CBasePlayerWeapon","m_iClip1");
        }

        if(reserve != -1)
        {
            weapon.ReserveAmmo[0] = reserve;
            Utilities.SetStateChanged(weapon,"CBasePlayerWeapon","m_pReserveAmmo");
        }
    }


    public static Dictionary<String,String> GUN_LIST = new Dictionary<String,String>()
    {
        {"AK47","ak47"},
        {"M4","m4a1_silencer"},
        {"M3","nova"},
        {"P90","p90"},
        {"M249","m249"},
        {"MP5","mp5sd"},
        {"FAL","galilar"},
        {"SG556","sg556"},
        {"BIZON","bizon"},
        {"AUG","aug"},
        {"FAMAS","famas"},
        {"XM1014","xm1014"},
        {"SCOUT","ssg08"},
        {"AWP", "awp"},
    };
    
    public static String GunGiveName(String name)
    {
        return "weapon_" + GUN_LIST[name];
    }

    public static void GiveWeapon(this CCSPlayerController? player,String name)
    {
        if(player.IsLegalAlive())
        {
            player.GiveNamedItem("weapon_" + name);
        }
    }
}