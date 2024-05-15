using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class ColorExtension {

        public static void setRed(ref this Color color, int value) {
            color = Color.FromArgb(value, color.G, color.B);
        }
        public static void setGreen(ref this Color color, int value) {
            color = Color.FromArgb(color.R, value, color.B);
        }
        public static void setBlue(ref this Color color, int value) {
            color = Color.FromArgb(color.R, color.G, value);
        }
        public static void setColor(ref this Color color, int r, int g, int b) {
            color = Color.FromArgb(r, g, b);
        }

        public static string? getLangName(this Color color) {
            var names = SharpEssentials._Config.Language.Colors;

            if(color == Color.Red) {
                return names.Red;

            } else if(color == Color.Red) {
                return names.Red;

            } else if(color == Color.OrangeRed) {
                return names.Orange;
                
            } else if(color == Color.Yellow) {
                return names.Yellow;

            } else if(color == Color.LimeGreen) {
                return names.Lime;

            } else if(color == Color.Green) {
                return names.Green;

            } else if(color == Color.Cyan) {
                return names.Cyan;

            } else if(color == Color.Blue) {
                return names.Blue;

            } else if(color == Color.BlueViolet) {
                return names.Violet;

            } else if(color == Color.Magenta) {
                return names.Magenta;

            } else if(color == Color.White) {
                return names.White;

            }
            return null;
        }
    }

}