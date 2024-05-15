using CounterStrikeSharp.API.Core;
using SharpEssentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cooldown {

    private Dictionary<ulong, DateTime> cooldowns = new Dictionary<ulong, DateTime>();

    public string RemainingMessage { get; set; }
    public int cooldownSeconds { get; set; } = 120;
    public bool debug { get; set; } = false;

    public Cooldown(int cooldown, string remainingMessage) {
        this.cooldownSeconds = cooldown;
        this.RemainingMessage = remainingMessage;
    }

    public void Destroy() {
        cooldowns.Clear();
    }

    public void Dispose(CCSPlayerController player) {
        cooldowns.Remove(player.SteamID);
        if(debug) { Logger.debug("[Cooldown] Disposed player with id > " + player.SteamID); }
    }

    public bool Has(CCSPlayerController player) {
        if(!cooldowns.ContainsKey(player.SteamID)) {
            cooldowns.Add(player.SteamID, DateTime.UtcNow);
            if(debug) { Logger.debug("[Cooldown] Added cooldown for id > " + player.SteamID); }
        } else {
            DateTime time = cooldowns[player.SteamID];
            TimeSpan diff = DateTime.UtcNow - time;
            if(diff.TotalSeconds >= cooldownSeconds) {
                cooldowns[player.SteamID] = DateTime.UtcNow;
                if(debug) { Logger.debug("[Cooldown] Reseted cooldown for id > " + player.SteamID); }
            } else {
                int seconds = (cooldownSeconds - (int)diff.TotalSeconds);
                TimeSpan left = TimeSpan.FromSeconds(seconds);

                //Console.WriteLine("Debug1: " + RemainingMessage);
                //Console.WriteLine(left.Hours + " - " + left.Minutes + " - " + left.Seconds);
                string formated = RemainingMessage;
                formated = formated.Replace("{H}", left.Hours.ToString(), StringComparison.OrdinalIgnoreCase);
                formated = formated.Replace("{M}", left.Minutes.ToString(), StringComparison.OrdinalIgnoreCase);
                formated = formated.Replace("{S}", left.Seconds.ToString(), StringComparison.OrdinalIgnoreCase);
                player.PrintToChat(formated);
                
                if(debug) {
                    string debugTime = String.Format("{0}:{1}:{2}", left.Hours, left.Minutes, left.Seconds);
                    Logger.debug("[Cooldown] Printing cooldown for id > " + player.SteamID + " - " + debugTime);
                }
                return true;
            }
        }
        return false;
    }



}

