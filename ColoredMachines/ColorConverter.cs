using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ColoredMachines
{
    public class ColorConverter
    {
        private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color>
        {
            {"red", Color.Red},
            {"orange", Color.Orange},
            {"yellow", Color.Yellow},
            {"green", Color.Green},
            {"blue", Color.Blue},
            {"indigo", Color.Indigo},
            {"violet", Color.Violet},
            {"white", Color.White},
            {"brown", Color.Brown},
            {"black", Color.Black}
        };

        public static Color getColorFromString(string colorName)
        {
            string lowercaseColorName = colorName.ToLower();
            if (colors.ContainsKey(lowercaseColorName))
            {
                return colors[lowercaseColorName];
            }

            if (lowercaseColorName.StartsWith("#") && lowercaseColorName.Length == 7)
            {
                int r = Convert.ToInt32(lowercaseColorName.Substring(1, 2), 16);
                int g = Convert.ToInt32(lowercaseColorName.Substring(3, 2), 16);
                int b = Convert.ToInt32(lowercaseColorName.Substring(5, 2), 16);
                return new Color(r, g, b);
            }

            return Color.White;
        }
        
    }
}