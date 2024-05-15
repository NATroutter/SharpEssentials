using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Admin;
using System.Drawing;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using SharpEssentials;
using System;

public static class PlayerExtension {
    // CONST DEFS
    public const int TEAM_SPEC = 1;
    public const int TEAM_T = 2;
    public const int TEAM_CT = 3;

    public static readonly Color DEFAULT_COLOUR = Color.FromArgb(255, 255, 255, 255);

    public static Dictionary<Guid, CBeam> beams = new Dictionary<Guid, CBeam>();

    public static void Send(this CCSPlayerController player, string msg) {
        player.PrintToChat(msg.Tags());
    }

    public static bool HasPermission(this CCSPlayerController player, params string[] nodes) {
        if(AdminManager.PlayerHasPermissions(player, SharpEssentials.SharpEssentials._Config.GlobalBypassPermission)) return true;
        return AdminManager.PlayerHasPermissions(player, nodes);
    }

    public static void cleanBeams() {
        foreach(var entry in beams) {
            if(entry.Value != null && entry.Value.IsValid) entry.Value.Remove();
            beams.Remove(entry.Key);
        }
    }

    public static void DrawLaserBetween(this CCSPlayerController player, BasePlugin plugin, Vector start, Vector end, Color color, float life, float width) {
        if(!player.IsLegalAlive()) return;

        CBeam? beam = Utilities.CreateEntityByName<CBeam>("beam");
        if(beam == null) {
            Logger.error("[PlayerExtension] Failed to create beam!!");
            return;
        }
        beam.Render = color;
        beam.Width = width;
        
        beam.Teleport(start, player.PlayerPawn.Value?.AbsRotation, player.PlayerPawn.Value?.AbsVelocity);
        beam.EndPos.X = end.X;
        beam.EndPos.Y = end.Y;
        beam.EndPos.Z = end.Z;
        beam.DispatchSpawn();

        Guid beamID = Guid.NewGuid();
        beams.Add(beamID, beam);

        plugin.AddTimer(life, () => {
            if(beam != null && beam.IsValid) {
                beam.Remove();
                
                if(!beams.ContainsKey(beamID)) return;
                
                CBeam sBean = beams[beamID];
                if(sBean != null && sBean.IsValid) sBean.Remove();
                beams.Remove(beamID);

            };
        });
    }

    public static bool setBalance(this CCSPlayerController player, int amount) {
        if(!player.IsLegal()) return false;

        var service = player.InGameMoneyServices;
        if(service == null) return false;

        service.Account = amount;

        Utilities.SetStateChanged(player, "CCSPlayerController", "m_pInGameMoneyServices");
        
        return true;
    }

    public static int GetBalance(this CCSPlayerController player) {
        if(!player.IsLegal()) return 0;
        
        var service = player.InGameMoneyServices;
        if(service == null) return 0;

        return service.Account;
    }

    public static Vector? GetPosition(this CCSPlayerController? player) {
        if(player != null && player.IsLegalAlive()) {
            return player.Pawn.Value?.AbsOrigin;
        }
        return null;
    }

    public static void PrintToTeamChat(this CCSPlayerController? player, string message) {
        if(!player.IsLegal()) return;

        Utils.GetPlayers().ForEach(online => {
            if(online.Team == player.Team) {
                online.PrintToChat(message);
            }
        });
    }

    public static void GiveArmour(this CCSPlayerController? player) {
        if(player.IsLegalAlive()) {
            player.GiveNamedItem("item_assaultsuit");
        }
    }

    public static void removeAllWeaponAmmo(this CCSPlayerController? player) {
        Server.NextFrame(() => {
            var weapons = player.FindWeapons();
            if(weapons != null) {
                foreach(var weapon in weapons) {
                    weapon.SetAmmo(0, 0);
                }
            }
        });
    }

    public static void Slay(this CCSPlayerController? player) {
        if(player.IsLegalAlive()) {
            player.PlayerPawn.Value?.CommitSuicide(true, true);
        }
    }

    // Cheers Kill for suggesting method extenstions
    public static bool IsLegal([NotNullWhen(true)] this CCSPlayerController? player) {
        return player != null && player.IsValid && player.PlayerPawn.IsValid && player.PlayerPawn.Value?.IsValid == true;
    }

    public static bool IsConnected([NotNullWhen(true)] this CCSPlayerController? player) {
        return player.IsLegal() && player.Connected == PlayerConnectedState.PlayerConnected;
    }

    public static bool IsT(this CCSPlayerController? player) {
        return IsLegal(player) && player.TeamNum == TEAM_T;
    }

    public static bool IsCt(this CCSPlayerController? player) {
        return IsLegal(player) && player.TeamNum == TEAM_CT;
    }
    public static bool IsSpec(this CCSPlayerController? player) {
        return IsLegal(player) && player.TeamNum == TEAM_SPEC;
    }

    public static bool IsLegalAlive([NotNullWhen(true)] this CCSPlayerController? player) {
        return player.IsConnected() && player.PawnIsAlive && player.PlayerPawn.Value?.LifeState == (byte)LifeState_t.LIFE_ALIVE;
    }

    public static bool IsLegalAliveT([NotNullWhen(true)] this CCSPlayerController? player) {
        return player.IsLegalAlive() && player.IsT();
    }


    public static bool IsLegalAliveCT([NotNullWhen(true)] this CCSPlayerController? player) {
        return player.IsLegalAlive() && player.IsCt();
    }

    public static int SlotFromName(String name) {
        foreach(CCSPlayerController player in Utils.GetPlayers()) {
            if(player.PlayerName == name) {
                return player.Slot;
            }
        }

        return -1;
    }

    public static CCSPlayerPawn? Pawn(this CCSPlayerController? player) {
        if(!player.IsLegalAlive()) {
            return null;
        }

        CCSPlayerPawn? pawn = player.PlayerPawn.Value;

        return pawn;
    }

    public static void SetHealth(this CCSPlayerController? player, int hp) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null) {
            pawn.Health = hp;
            Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
        }
    }

    public static int GetHealth(this CCSPlayerController? player) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn == null) {
            return 0;
        }

        return pawn.Health;
    }

    public static void Freeze(this CCSPlayerController? player) {
        player.SetMoveType(MoveType_t.MOVETYPE_NONE);
    }

    public static void UnFreeze(this CCSPlayerController? player) {
        player.SetMoveType(MoveType_t.MOVETYPE_WALK);
    }

    public static void SetMoveType(this CCSPlayerController? player, MoveType_t type) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null) {
            pawn.MoveType = type;
        }
    }

    public static void SetGravity(this CCSPlayerController? player, float value) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null) {
            pawn.GravityScale = value;
        }
    }

    public static void SetVelocity(this CCSPlayerController? player, float value) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null) {
            pawn.VelocityModifier = value;
        }
    }


    public static void SetArmour(this CCSPlayerController? player, int hp) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null) {
            pawn.ArmorValue = hp;
        }
    }

    public static void StripWeapons(this CCSPlayerController? player, bool removeKnife = false) {
        // only care if player is valid
        if(!player.IsLegalAlive()) {
            return;
        }

        player.RemoveWeapons();

        // dont remove knife its buggy
        if(!removeKnife) {
            player.GiveWeapon("knife");
        }
    }

    public static void SetColour(this CCSPlayerController? player, Color colour) {
        CCSPlayerPawn? pawn = player.Pawn();

        if(pawn != null && player.IsLegalAlive()) {
            pawn.RenderMode = RenderMode_t.kRenderTransColor;
            pawn.Render = colour;
            Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
        }
    }

    public static bool IsGenericAdmin(this CCSPlayerController? player) {
        if(!player.IsLegal()) {
            return false;
        }

        return AdminManager.PlayerHasPermissions(player, new String[] { "@css/generic" });
    }

    public static void PlaySound(this CCSPlayerController? player, String sound) {
        if(!player.IsLegal()) {
            return;
        }

        player.ExecuteClientCommand($"play {sound}");
    }


    public static void ListenAll(this CCSPlayerController? player) {
        if(!player.IsLegal()) {
            return;
        }

        player.VoiceFlags |= VoiceFlags.ListenAll;
        player.VoiceFlags &= ~VoiceFlags.ListenTeam;
    }

    public static void ListenTeam(this CCSPlayerController? player) {
        if(!player.IsLegal()) {
            return;
        }

        player.VoiceFlags &= ~VoiceFlags.ListenAll;
        player.VoiceFlags |= VoiceFlags.ListenTeam;
    }

    public static void Mute(this CCSPlayerController? player) {
        if(!player.IsLegal()) {
            return;
        }

        // admins cannot be muted by the plugin
        if(!player.IsGenericAdmin()) {
            player.VoiceFlags |= VoiceFlags.Muted;
        }
    }

    // TODO: this needs to be hooked into the ban system that becomes used
    public static void UnMute(this CCSPlayerController? player) {
        if(!player.IsLegal()) {
            return;
        }

        player.VoiceFlags &= ~VoiceFlags.Muted;
    }


    public static void RestoreHP(this CCSPlayerController? player, int damage, int health) {
        if(!player.IsLegal()) {
            return;
        }

        // TODO: why does this sometimes mess up?
        if(health < 100) {
            player.SetHealth(Math.Min(health + damage, 100));
        } else {
            player.SetHealth(health + damage);
        }
    }

    public static Vector? EyeVector(this CCSPlayerController? player) {
        var pawn = player.Pawn();

        if(pawn == null) {
            return null;
        }

        QAngle eyeAngle = pawn.EyeAngles;

        // convert angles to rad 
        double pitch = (Math.PI / 180) * eyeAngle.X;
        double yaw = (Math.PI / 180) * eyeAngle.Y;

        // get direction vector from angles
        Vector eyeVector = new Vector((float)(Math.Cos(yaw) * Math.Cos(pitch)), (float)(Math.Sin(yaw) * Math.Cos(pitch)), (float)(-Math.Sin(pitch)));

        return eyeVector;
    }

    public static void RespawnCallback(int slot) {
        var player = Utilities.GetPlayerFromSlot(slot);

        if(player.IsLegal()) {
            player.Respawn();
        }
    }

    public static void Nuke() {
        foreach(CCSPlayerController target in Utils.GetPlayers()) {
            target.Slay();
        }
    }
}