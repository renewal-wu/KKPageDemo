using System;
using Windows.UI;

namespace KKPageDemo
{
    public class ColorHelper
    {
        public static Color GetRandomColor()
        {
            Random random = new Random();
            byte r = (byte)random.Next(0, 255);
            byte g = (byte)random.Next(0, 255);
            byte b = (byte)random.Next(0, 255);

            return Color.FromArgb(255, r, g, b);
        }
    }
}
