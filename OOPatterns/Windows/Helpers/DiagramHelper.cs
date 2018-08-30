using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with DiagramToolbar
    /// </summary>
    public class DiagramHelper
    {
        /// <summary>
        /// Main window with controls
        /// </summary>
        private MainWindow Window;

        /// <summary>
        /// Event fired when item click. sender is DiagramItem
        /// </summary>
        public EventHandler OnDiagramToolbarItemClick;

        public enum DiagramItem
        {
            CLASS,
            INTERFACE,
            AGGREAGTION,
            COMPOSITION,
            REALIZATION,
            DEPENDENCY
        }

        public DiagramHelper(MainWindow window)
        {
            Window = window;
            Window.DiagramToolbar.MouseLeftButtonDown += DiagramToolbarMouseLeftButtonDown;
        }

        /// <summary>
        /// Reset stroke of relations path
        /// </summary>
        public void ClearSelected()
        {
            Window.AggregationPath.Stroke = Brushes.White;
            Window.CompositionPath.Stroke = Brushes.White;
            Window.RealizationPath.Stroke = Brushes.White;
            Window.DependecyPath.Stroke = Brushes.White;
        }

        private void DiagramToolbarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                var position = e.GetPosition(sender as FrameworkElement);
                if (position.Y >= 10 && position.Y <= 65)
                {
                    //class
                    ClearSelected();
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.CLASS, EventArgs.Empty);
                }
                else if (position.Y >= 80 && position.Y <= 115)
                {
                    //interface
                    ClearSelected();
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.INTERFACE, EventArgs.Empty);
                }
                else if (position.Y >= 130 && position.Y <= 155)
                {
                    //aggregation
                    ClearSelected();
                    Window.AggregationPath.Stroke = Core.Core.GetInstance().ThemeHelper.SelectedItemBrush;
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.AGGREAGTION, EventArgs.Empty);
                }
                else if (position.Y >= 170 && position.Y <= 195)
                {
                    //composition
                    ClearSelected();
                    Window.CompositionPath.Stroke = Core.Core.GetInstance().ThemeHelper.SelectedItemBrush;
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.COMPOSITION, EventArgs.Empty);
                }
                else if (position.Y >= 205 && position.Y <= 235)
                {
                    //realization
                    ClearSelected();
                    Window.RealizationPath.Stroke = Core.Core.GetInstance().ThemeHelper.SelectedItemBrush;
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.REALIZATION, EventArgs.Empty);
                }
                else if (position.Y >= 245 && position.Y <= 270)
                {
                    //dependency
                    ClearSelected();
                    Window.DependecyPath.Stroke = Core.Core.GetInstance().ThemeHelper.SelectedItemBrush;
                    OnDiagramToolbarItemClick?.Invoke(DiagramItem.DEPENDENCY, EventArgs.Empty);
                }
            }
        }
    }
}
