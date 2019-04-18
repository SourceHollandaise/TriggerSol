using System.Drawing;
using TriggerSol.Extensions.Types;

namespace TriggerSol.Drawing
{
    public class ColorConverter
    {
        public GenericColor Get(Color color) => new GenericColor(color.R, color.G, color.B, color.A);

        public GenericColor Get(int red, int green, int blue, byte alpha) => new GenericColor(red, green, blue, alpha);

        public Color Get(GenericColor color) => Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);

        public string ColorValue(GenericColor color) => Get(color).ToString();
    }
}
