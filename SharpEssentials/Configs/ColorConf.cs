using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public class ColorConf {

        public ColorConf() { }
        public ColorConf(int red, int green, int blue) {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public int Red { get; set; } = 0;
        public int Green { get; set; } = 0;
        public int Blue { get; set; } = 0;


        public Color GetColor() {
            return Color.FromArgb(Red, Green, Blue);
        }

    }
}
