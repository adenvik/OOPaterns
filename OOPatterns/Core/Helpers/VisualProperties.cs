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
    /// <summary>
    /// Static class for centralize properties necessary for visualization
    /// </summary>
    public static class VisualProperties
    {
        /// <summary>
        /// Standart font size for text
        /// </summary>
        public static double FontSize = 12;

        /// <summary>
        /// Standart font family for text
        /// </summary>
        public static string FontFamily = "Arial";

        /// <summary>
        /// Font size for hint text
        /// </summary>
        public static double PROPERTIES_TEXT_SIZE = 10;

        /// <summary>
        /// 
        /// </summary>
        public static string VARIABLES = Properties.Resources.variables.ToLower();

        /// <summary>
        /// 
        /// </summary>
        public static string METHODS = Properties.Resources.methods.ToLower();

        /// <summary>
        /// Size of image, drawing on visual object, defining type
        /// </summary>
        public static float ImageSize = 12f;

        /// <summary>
        /// Margin parameter for drawing
        /// </summary>
        public static float Delta = 5f;

        /// <summary>
        /// Returns size of text with the specified font size
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="fontSize">Font size</param>
        /// <returns></returns>
        public static Size GetTextSize(string text, double fontSize)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(FontFamily), FontStyles.Normal,
                FontWeights.Normal, new FontStretch()),
                fontSize,
                Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
    }
}
