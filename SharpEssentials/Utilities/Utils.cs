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
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using System.Runtime.InteropServices;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;


public static class Utils {

    public static bool IsWindows() {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    public static void PlaySoundAll(String sound) {
        foreach(CCSPlayerController? player in GetPlayers()) {
            player.PlaySound(sound);
        }
    }

    public static void MuteT() {
        foreach(CCSPlayerController player in GetPlayers()) {
            if(player.IsT()) {
                player.Mute();
            }
        }
    }

    public static void KillTimer(ref Timer? timer) {
        if(timer != null) {
            timer.Kill();
            timer = null;
        }
    }

    public static void UnMuteAll() {
        foreach(CCSPlayerController player in GetPlayers()) {
            player.UnMute();
        }
    }

    public static long CurTimestamp() {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public static void SwapAllT() {
        foreach(var player in GetAlivePlayers()) {
            player.SwitchTeam(CsTeam.Terrorist);
        }
    }

    public static void SwapAllCT() {
        foreach(var player in GetAlivePlayers()) {
            player.SwitchTeam(CsTeam.CounterTerrorist);
        }
    }

    public static void RespawnPlayers() {
        // 1up all dead players
        foreach(CCSPlayerController player in GetPlayers()) {
            if(!player.IsLegalAlive()) {
                player.Respawn();
            }
        }
    }

    public static List<CCSPlayerController> GetAlivePlayers() {
        return Utilities.GetPlayers().FindAll(player => player.IsLegalAlive());
    }

    public static List<CCSPlayerController> GetPlayers() {
        return Utilities.GetPlayers().FindAll(player => player.IsLegal() && player.IsConnected());
    }

    public static List<CCSPlayerController> GetAliveCt() {
        return GetPlayers().FindAll(player => player.IsLegalAlive() && player.IsCt());
    }

    public static int CtCount() {
        return GetPlayers().FindAll(player => player.IsLegal() && player.IsCt()).Count;
    }

    public static int TCount() {
        return GetPlayers().FindAll(player => player.IsLegal() && player.IsT()).Count;
    }

    public static int AliveCtCount() {
        return GetAliveCt().Count;
    }

    public static List<CCSPlayerController> GetAliveT() {
        List<CCSPlayerController> players = GetPlayers();
        return players.FindAll(player => player.IsLegalAlive() && player.IsT()); ;
    }

    public static int AliveTCount() {
        return GetAliveT().Count;
    }

    public static bool IsActiveTeam(int team) {
        return (team == PlayerExtension.TEAM_T || team == PlayerExtension.TEAM_CT);
    }
}