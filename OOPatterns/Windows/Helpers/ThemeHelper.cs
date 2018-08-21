using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with thems
    /// </summary>
    public class ThemeHelper
    {
        /// <summary>
        /// Window for working with resources
        /// </summary>
        private Window Window;

        /// <summary>
        /// Brush of visual class object
        /// </summary>
        public LinearGradientBrush ClassGradient;

        /// <summary>
        /// Brush of visual interface object
        /// </summary>
        public LinearGradientBrush InterfaceGradient;

        /// <summary>
        /// Brush of item stroke, if item selected
        /// </summary>
        public Brush SelectedItemBrush;

        /// <summary>
        /// Brush of item stroke, if item does not selected
        /// </summary>
        public Brush NormalItemBrush;

        public ThemeHelper(Window window)
        {
            Window = window;
            Load();
        }

        /// <summary>
        /// Loads brushes
        /// </summary>
        private void Load()
        {
            ClassGradient = (LinearGradientBrush)Window.TryFindResource("ClassGradient");
            InterfaceGradient = (LinearGradientBrush)Window.TryFindResource("InterfaceGradient");
            SelectedItemBrush = (SolidColorBrush)Window.TryFindResource("SelectedItemBrush");
            NormalItemBrush = (SolidColorBrush)Window.TryFindResource("NormalItemBrush");
        }

        /// <summary>
        /// Load Light theme, and update brushes
        /// </summary>
        public void LoadLightTheme()
        {
            var uri = new Uri("/Windows/Themes/Brushes_light.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            Load();
        }

        /// <summary>
        /// Load Dark theme, and update brushes
        /// </summary>
        public void LoadDarkTheme()
        {
            var uri = new Uri("/Windows/Themes/Brushes.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            Load();
        }
    }
}
