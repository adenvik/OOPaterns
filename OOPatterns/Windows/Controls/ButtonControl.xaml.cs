using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOPatterns.Windows.Controls
{
    /// <summary>
    /// Логика взаимодействия для ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register(
                    "Text",
                    typeof(string),
                    typeof(ButtonControl));

        public string Text
        {
            get => GetValue(TextProperty).ToString(); 
            set { SetValue(TextProperty, value); }
        }

        public static DependencyProperty NormalBackgroundProperty = DependencyProperty.Register(
                    "NormalBackground",
                    typeof(SolidColorBrush),
                    typeof(ButtonControl),
                    new PropertyMetadata { DefaultValue = null });

        public SolidColorBrush NormalBackground
        {
            get => (SolidColorBrush) GetValue(NormalBackgroundProperty);
            set { SetValue(NormalBackgroundProperty, value); }
        }

        public static DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
                    "MouseOverBackground",
                    typeof(SolidColorBrush),
                    typeof(ButtonControl),
                    new PropertyMetadata(null));

        public SolidColorBrush MouseOverBackground
        {
            get => (SolidColorBrush)GetValue(MouseOverBackgroundProperty);
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonControl));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public ButtonControl()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = MouseOverBackground ?? (SolidColorBrush)TryFindResource("OverButton");
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = NormalBackground ?? (SolidColorBrush)TryFindResource("PrimaryButton");
            panel.BeginAnimation(OpacityProperty, null);
            panel.Opacity = 1;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.8,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            (sender as StackPanel).BeginAnimation(OpacityProperty, fadeOut);
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0.8,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            (sender as StackPanel).BeginAnimation(OpacityProperty, fadeIn);
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(args);
        }
    }
}
