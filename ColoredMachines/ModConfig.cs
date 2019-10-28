using System.Collections.Generic;

namespace ColoredMachines
{
    public class ModConfig
    {
        public float AlphaValue { get; set; }

        public Dictionary<string, Dictionary<string, string>> ColorOptions { get; set; }

        private string[] MachineTypes = { "Keg", "Preserves Jar", "Cask", "Oil Maker", "Cheese Press", "Mayonnaise Machine" };
        
        public ModConfig()
        {
            // Set Defaults
            AlphaValue = 0.5f;
            ColorOptions = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> inner = new Dictionary<string, string>()
            {
                { "ready", "red" },
                { "processing", "green" },
                { "empty", "white" }
            };
            foreach (string type in MachineTypes)
            {
                ColorOptions.Add(type, new Dictionary<string, string>(inner));
            }
        }
    }
}