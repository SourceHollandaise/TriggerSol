using System;
using System.Drawing;
using System.Linq;
using TriggerSol.Extensions.Types;

namespace TriggerSol.Drawing
{
    public class ColorConverter
    {
        public GenericColor Get(Color color)
        {
            return new GenericColor(color.R, color.G, color.B, color.A);
        }

        public GenericColor Get(int red, int green, int blue, byte alpha)
        {
            return new GenericColor(red, green, blue, alpha);
        }

        public Color Get(GenericColor color)
        {
            return Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
        }

        public string ColorValue(GenericColor color)
        {
            return Get(color).ToString();
        }
    }
}
