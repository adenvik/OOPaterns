using OOPatterns.Core;
using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects;
using OOPatterns.Windows;
using OOPatterns.Windows.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OOPatterns
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core.Core core;
        bool IsMaximized = false;

        public MainWindow()
        {
            InitializeComponent();
            core = Core.Core.GetInstance(this);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TopToolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid) DragMove();
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = IsMaximized ? WindowState.Normal : WindowState.Maximized;
            IsMaximized = !IsMaximized;
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).ContextMenu.IsOpen = true;
        }
    }
}
