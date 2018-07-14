using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OOPatterns.Core.Helpers
{
    public static class VisualProperties
    {
        public static double FontSize = 12;
        public static string FontFamily = "Arial";

        public static float PROPERTIES_TEXT_SIZE = 10f;
        public static string VARIABLES = Properties.Resources.variables.ToLower();
        public static string METHODS = Properties.Resources.methods.ToLower();

        public static Brush Selected = Brushes.GreenYellow;
        public static Brush Normal = Brushes.Black;

        public static Brush Interface = Brushes.SandyBrown;
        public static Brush Class = Brushes.SkyBlue;

        public static float ImageSize = 12f;
        public static float Delta = 5f;

        public static Size GetTextSize(string text)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(FontFamily), FontStyles.Normal,
                FontWeights.Normal, new FontStretch()),
                FontSize,
                Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
    }
}
