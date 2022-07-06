using System.Windows;
using System.Windows.Media;

namespace Typishe.Resources.Controls
{
    public class FontData
    {
        public FontFamily FontFamily { get; set; }
        public double FontSize { get; set; }
        public FontWeight FontWeight { get; set; }
        public TextDecorationCollection TextDecorations { get; set; }

        public FontData Clone()
        {
            var clonedFontFamily = new FontFamily(FontFamily.Source);
            var clonedTextDecorations = TextDecorations == null? null : TextDecorations.Clone();

            return new FontData()
            {
                FontFamily = clonedFontFamily,
                FontSize = this.FontSize,
                FontWeight = this.FontWeight,
                TextDecorations = clonedTextDecorations,
            };
        }
    }
}
