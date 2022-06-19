using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Typishe.Service
{
    public static class Extensions
    {
        private static BrushConverter _converter = new BrushConverter();

        public static SolidColorBrush ToSolidSolorBrush(this string brush) =>
            _converter.ConvertFromString(brush) as SolidColorBrush;

        public static Color ToColor(this string color) =>
            (Color)ColorConverter.ConvertFromString(color);

        public static bool IsInRange(this int number, int a, int b) =>
            ((number >= a) && (number <= b));

        public static double Parse(this string line)
        {
            if (line.TryParse(out double result))
            {
                return result;
            }

            return 0d;
        }

        public static bool TryParse(this string line, out double result)
        {
            var lastCharacter = line.Last();
            if (lastCharacter == '.' || lastCharacter == ',')
            {
                result = 0;
                return false;
            }

            var success = double.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
            result = value;

            return success;
        }

        public static T Copy<T>(this T grid) where T : UIElement
        {
            var stringReader = new StringReader(XamlWriter.Save(grid));
            var xmlReader = XmlReader.Create(stringReader);

            var newGrid = XamlReader.Load(xmlReader) as T;
            return newGrid;
        }

        public static Color GetRelativeColor(this GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                {
                    before = gs;
                }
                if (gs.Offset > offset && gs.Offset < after.Offset)
                {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }

        public static bool IsEqualOr(this int number, params int[] numbers)
        {
            var value = false;

            foreach (var num in numbers)
            {
                if (num == number)
                {
                    value = true;
                    break;
                }
            }

            return value;
        }

        public static bool IsEqualOr(this string line, params string[] lines)
        {
            var value = false;

            foreach (var lineOne in lines)
            {
                if (lineOne == line)
                {
                    value = true;
                    break;
                }
            }

            return value;
        }

        public static HsvColor ToHsv(this Color rgbColor) 
        {
            int max = Math.Max(rgbColor.R, Math.Max(rgbColor.G, rgbColor.B));
            int min = Math.Min(rgbColor.R, Math.Min(rgbColor.G, rgbColor.B));

            double hue, saturation, value;
            hue = GetHue(rgbColor);

            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;

            return new HsvColor()
            {
                Hue = GetHue(rgbColor),
                Saturation = saturation,
                Brightness = value
            };
        }
            

        private static double GetHue(Color rgbColor)
        {
            if (rgbColor.R == rgbColor.G && rgbColor.G == rgbColor.B)
            {
                return 0d;
            }

            var r = rgbColor.R / 255d;
            var g = rgbColor.G / 255d;
            var b = rgbColor.B / 255d;

            double max, min;
            double delta;
            double hue = 0.0d;

            min = max = r; 

            if (g > max)
            {
                max = g;
            }

            if (b > max) 
            { 
                max = b; 
            }

            if (g < min)
            {
                min = g;
            }

            if (b < min)
            {
                min = b;
            }

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }

            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }

            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }

            hue *= 60;

            if (hue < 0.0d)
            {
                hue += 360.0d;
            }

            return hue;
        }
   
    }
}
