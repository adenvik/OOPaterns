using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with context menu
    /// </summary>
    public class ContextMenuHelper
    {
        /// <summary>
        /// Canvas context menu, if click in the empty position
        /// </summary>
        public ContextMenu StandartElementsViewContextMeny { get; protected set; } = null;

        /// <summary>
        /// Canvas context menu, if click in the object position
        /// </summary>
        public ContextMenu ChildElementOnElementsViewContextMeny { get; protected set; } = null;

        /// <summary>
        /// ListView context menu, for working with variables or methods
        /// </summary>
        public ContextMenu ListViewItemContextMenu { get; protected set; } = null;

        /// <summary>
        /// Event fired when context menu item clicked
        /// </summary>
        public EventHandler OnMenuItemClick;

        /// <summary>
        /// Class with information about current menu item
        /// </summary>
        public class MenuItemDetail
        {
            /// <summary>
            /// Header of the menu item
            /// </summary>
            public string Header { set; get; }

            /// <summary>
            /// Control, which owns context menu
            /// </summary>
            public FrameworkElement Target { set; get; }
        }

        /// <summary>
        /// Main window with controls
        /// </summary>
        private MainWindow Window;

        public ContextMenuHelper(MainWindow window)
        {
            Window = window;
            InitializeContextMenus();
            
            Style itemStyle = Window.Variables_LV.ItemContainerStyle;
            itemStyle.TargetType = typeof(ListViewItem);
            itemStyle.Setters.Add(new Setter(ListViewItem.ContextMenuProperty, ListViewItemContextMenu));

            Window.Parents_LV.ItemContainerStyle = itemStyle;
            Window.Variables_LV.ItemContainerStyle = itemStyle;
            Window.Methods_LV.ItemContainerStyle = itemStyle;
        }

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            var header = (sender as MenuItem).Header.ToString();
            var targetElement = GetTargetElement(sender as MenuItem);
            OnMenuItemClick?.Invoke(new MenuItemDetail { Header = header, Target = targetElement }, EventArgs.Empty);
        }

        /// <summary>
        /// Returns control, which owns context menu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FrameworkElement GetTargetElement(MenuItem item)
        {
            while(!(item.Parent is ContextMenu))
            {
                item = (item.Parent as MenuItem);
            }
            return (item.Parent as ContextMenu).PlacementTarget as FrameworkElement;
        }
        
        /// <summary>
        /// Initialize context menus
        /// </summary>
        private void InitializeContextMenus()
        {
            #region StandartElementsViewContextMenys
            StandartElementsViewContextMeny = new ContextMenu();

            MenuItem addNew = new MenuItem();
            addNew.Header = Properties.Resources.add;

            MenuItem addClass = new MenuItem();
            addClass.Header = Core.Core.CLASS;
            addClass.Icon = new Image
            {
                Source = new BitmapImage(new Uri("/Images/class_ico.png", UriKind.Relative))
            };
            addClass.Click += OnItemClick;

            MenuItem addInterface = new MenuItem();
            addInterface.Header = Core.Core.INTERFACE;
            addInterface.Icon = new Image
            {
                Source = new BitmapImage(new Uri("/Images/interface_ico.png", UriKind.Relative))
            };
            addInterface.Click += OnItemClick;

            addNew.Items.Add(addClass);
            addNew.Items.Add(addInterface);

            StandartElementsViewContextMeny.Items.Add(addNew);
            #endregion

            #region ChildElementOnElementsViewContextMeny
            ChildElementOnElementsViewContextMeny = new ContextMenu();
            MenuItem add = new MenuItem();
            add.Header = Properties.Resources.add;
            MenuItem aggregation = new MenuItem();
            aggregation.Header = Properties.Resources.aggregation;
            aggregation.Click += OnItemClick;

            MenuItem composition = new MenuItem();
            composition.Header = Properties.Resources.composition;
            composition.Click += OnItemClick;

            MenuItem dependency = new MenuItem();
            dependency.Header = Properties.Resources.dependency;
            dependency.Click += OnItemClick;

            MenuItem realization = new MenuItem();
            realization.Header = Properties.Resources.realization;
            realization.Click += OnItemClick;

            add.Items.Add(aggregation);
            add.Items.Add(composition);
            add.Items.Add(dependency);
            add.Items.Add(realization);

            MenuItem delete = new MenuItem();
            delete.Header = Properties.Resources.delete;
            delete.Click += OnItemClick;

            ChildElementOnElementsViewContextMeny.Items.Add(add);
            ChildElementOnElementsViewContextMeny.Items.Add(delete);
            #endregion

            #region ListViewItemContextMenu
            ListViewItemContextMenu = new ContextMenu();
            MenuItem deleteParam = new MenuItem();
            deleteParam.Header = Properties.Resources.delete;
            deleteParam.Click += OnItemClick;
            ListViewItemContextMenu.Items.Add(deleteParam);
            #endregion
        }
    }
}
