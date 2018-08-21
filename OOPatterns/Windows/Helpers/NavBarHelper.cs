using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with navigation bar
    /// </summary>
    public class NavBarHelper
    {
        /// <summary>
        /// Event, fired during the transition
        /// </summary>
        public EventHandler OnNavItemClick;

        /// <summary>
        /// Whether the transition fired programmatically
        /// </summary>
        private bool IsProgrammatically = false;

        /// <summary>
        /// Array of navigation panels
        /// </summary>
        UIElement[] NavPanels;

        /// <summary>
        /// Current panel
        /// </summary>
        UIElement Selected;

        public NavBarHelper(params UIElement[] elements)
        {
            NavPanels = elements;
            if(elements.Length > 0) Selected = NavPanels[0];
            InitEvents();
        }

        /// <summary>
        /// Activate navigation panel, and maybe fired navigation
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="navigate"></param>
        /// <returns></returns>
        public NavBarHelper EnablePanel(UIElement panel, bool navigate = false)
        {
            foreach(UIElement p in NavPanels)
            {
                if (p.Equals(panel))
                {
                    p.IsEnabled = true;
                    ((p as StackPanel).Children[1] as Label).SetResourceReference(Label.ForegroundProperty, "PrimaryForeground");
                    break;
                }
            }
            if (navigate) Navigate(panel);
            return this;
        }

        /// <summary>
        /// Disable navigation panel
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <returns></returns>
        public NavBarHelper DisablePanel(UIElement panel)
        {
            foreach (UIElement p in NavPanels)
            {
                if (p.Equals(panel))
                {
                    p.IsEnabled = false;
                    ((p as StackPanel).Children[1] as Label).SetResourceReference(Label.ForegroundProperty, "DisabledForeground");
                    break;
                }
            }
            return this;
        }

        /// <summary>
        /// Fired click event on panel
        /// </summary>
        /// <param name="panel"></param>
        public void Navigate(UIElement panel)
        {
            foreach (UIElement p in NavPanels)
            {
                if (p.Equals(panel))
                {
                    MouseButtonEventArgs arg = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                    arg.RoutedEvent = StackPanel.MouseLeftButtonDownEvent;
                    IsProgrammatically = true;
                    p.RaiseEvent(arg);
                    break;
                }
            }
        }

        /// <summary>
        /// Added events on panels
        /// </summary>
        private void InitEvents()
        {
            for(int i = 0; i < NavPanels.Length; i++)
            {
                NavPanels[i].MouseEnter += (sender, e) =>
                {
                    ((sender as StackPanel).Parent as Border).SetResourceReference(Border.BackgroundProperty, "PrimaryLight");
                };

                NavPanels[i].MouseLeave += (sender, e) =>
                {
                    ((sender as StackPanel).Parent as Border).SetResourceReference(StackPanel.BackgroundProperty, "SystemButton");
                };

                NavPanels[i].MouseLeftButtonDown += (sender, e) =>
                {
                    if (Selected == (sender as UIElement)) return;
                    if (Selected != null)
                    {
                        var p = Selected as StackPanel;
                        p.SetResourceReference(StackPanel.BackgroundProperty, "SystemButton");
                        (p.Children[1] as Label).SetResourceReference(Label.ForegroundProperty, "PrimaryForeground");
                    }

                    var panel = sender as StackPanel;

                    panel.SetResourceReference(StackPanel.BackgroundProperty, "PrimaryDark");
                    var label = panel.Children[1] as Label;
                    label.SetResourceReference(Label.ForegroundProperty, "PrimaryForeground");

                    Selected = sender as UIElement;

                    if (IsProgrammatically)
                    {
                        OnNavItemClick?.Invoke(Selected, null);
                        IsProgrammatically = false;
                    }
                    else
                    {
                        OnNavItemClick?.Invoke(Selected, EventArgs.Empty);
                    }
                };
            }
        }
    }
}
