using System.Drawing;

namespace CountMyCode.Utils;

public static class ColourUtils
{
    private static readonly Random rand = new Random();

    public static Color GetRandomHSBColor()
    {
        float hue = (float)(rand.NextDouble() * 360);
        return FromHSB(hue, 1f, 1f); 
    }

    public static Color FromHSB(float hue, float saturation, float brightness)
    {
        if (saturation == 0)
        {
            int v = (int)(brightness * 255);
            return Color.FromArgb(v, v, v);
        }

        float h = hue / 60f;
        int sector = (int)Math.Floor(h);
        float fraction = h - sector;

        float p = brightness * (1 - saturation);
        float q = brightness * (1 - saturation * fraction);
        float t = brightness * (1 - saturation * (1 - fraction));

        float r = 0, g = 0, b = 0;

        switch (sector % 6)
        {
            case 0: r = brightness; g = t; b = p; break;
            case 1: r = q; g = brightness; b = p; break;
            case 2: r = p; g = brightness; b = t; break;
            case 3: r = p; g = q; b = brightness; break;
            case 4: r = t; g = p; b = brightness; break;
            case 5: r = brightness; g = p; b = q; break;
        }

        return Color.FromArgb(
            (int)(r * 255),
            (int)(g * 255),
            (int)(b * 255));
    }

    public static string ToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
