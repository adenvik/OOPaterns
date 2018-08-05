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
    public class ContextMenuHelper
    {
        public ContextMenu StandartElementsViewContextMeny { get; set; } = null;
        public ContextMenu ChildElementOnElementsViewContextMeny { get; set; } = null;

        public ContextMenuHelper(RoutedEventHandler onClick)
        {
            InitializeContextMenus(onClick);
        }
        
        private void InitializeContextMenus(RoutedEventHandler OnClick)
        {
            #region StandartElementsViewContextMeny
            StandartElementsViewContextMeny = new ContextMenu();

            MenuItem addNew = new MenuItem();
            addNew.Header = Properties.Resources.add;

            MenuItem addClass = new MenuItem();
            addClass.Header = Core.Core.CLASS;
            addClass.Icon = new Image
            {
                Source = new BitmapImage(new Uri("/Images/class_ico.png", UriKind.Relative))
            };
            addClass.Click += OnClick;

            MenuItem addInterface = new MenuItem();
            addInterface.Header = Core.Core.INTERFACE;
            addInterface.Icon = new Image
            {
                Source = new BitmapImage(new Uri("/Images/interface_ico.png", UriKind.Relative))
            };
            addInterface.Click += OnClick;

            addNew.Items.Add(addClass);
            addNew.Items.Add(addInterface);

            StandartElementsViewContextMeny.Items.Add(addNew);
            #endregion
        }
    }
}
