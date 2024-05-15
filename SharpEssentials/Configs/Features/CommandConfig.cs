namespace SharpEssentials {
    public class CommandConfig {
        public bool Enabled { get; set; }
        public List<string> Commands { get; set; }
        public string Description { get; set; }
        
        public bool usePermission { get; set; }
        public string Permission { get; set; }

        public bool UseCooldown { get; set; }
        public int CooldownTime { get; set; }
        public string CooldownMessage { get; set; }
        


        public CommandConfig(bool enabled, List<string> commands, string description, bool usePermission, string Permission, bool useCooldown, int cooldownTime, string cooldownMessage) {
            this.Enabled = enabled;
            this.Commands = commands;
            this.Description = description;
            
            this.usePermission = usePermission;
            this.Permission = Permission;

            this.UseCooldown = useCooldown;
            this.CooldownTime = cooldownTime;
            this.CooldownMessage = cooldownMessage;
        }

    }
}
