using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Log;
using OOPatterns.Core.VisualObjects;
using OOPatterns.Windows.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static OOPatterns.Windows.Helpers.ContextMenuHelper;

namespace OOPatterns.Core
{
    /// <summary>
    /// Core of the application
    /// </summary>
    public class Core
    {
        public const string CLASS = "class";
        public const string INTERFACE = "interface";

        public ILoggable Logger { get; }
        public ThemeHelper ThemeHelper { get; protected set; }
        public CanvasHelper CanvasHelper { get; }
        public EditHelper EditHelper { get; }
        public NavBarHelper NavBarHelper { get; }
        public ContextMenuHelper ContextMenuHelper { get; }
        public AnimationHelper AnimationHelper { get; }

        /// <summary>
        /// List of objects on the diagram
        /// </summary>
        public List<UserType> Objects
        {
            get => CanvasHelper.Objects.Select(o => o.Object).ToList();
        }

        private UserType Selected;
        /// <summary>
        /// Window with controls for helpers
        /// </summary>
        private MainWindow Window;

        private static Core core = null;

        private Core(MainWindow window)
        {
            Logger = new Logger();
            if(window != null)
            {
                Window = window;

                ThemeHelper = new ThemeHelper(window);
                CanvasHelper = new CanvasHelper(window.ElementsView);
                NavBarHelper = new NavBarHelper(window.Diagram, window.Edit);
                AnimationHelper = new AnimationHelper(window);
                ContextMenuHelper = new ContextMenuHelper(window);
                EditHelper = new EditHelper(window);

                ContextMenuHelper.OnMenuItemClick += OnMenuItemClick;
                NavBarHelper.OnNavItemClick += OnNavItemClick;
                CanvasHelper.OnSelectedChanged += OnSelectedItemChanged;
                CanvasHelper.OnDoubleClick += OnItemDoubleClick;
                EditHelper.OnDataChanged += OnDataChanged;
            }
        }
        
        private void OnDataChanged(object sender, EventArgs e)
        {
            if(Selected != EditHelper.CurrentObject)
            {
                CanvasHelper.Remove(CanvasHelper.SelectedItem);
                CanvasHelper.SelectedItem = null;
                Selected = EditHelper.CurrentObject;
                if (Selected == null)
                {
                    NavBarHelper.Navigate(Window.Diagram);
                    NavBarHelper.DisablePanel(Window.Edit);
                }
                else
                {
                    CanvasHelper.Add(new VisualObject(Selected, CanvasHelper.Canvas), true);
                }
            }
            CanvasHelper.ReDraw();
        }

        private void OnMenuItemClick(object sender, EventArgs e)
        {
            var menuItemDetail = sender as MenuItemDetail;
            var actions = new Dictionary<string, Action>
            {
                {
                    CLASS, () =>
                    {
                        CanvasHelper.Add(new VisualObject(new Class(), CanvasHelper.Canvas));
                        PreparationEdit();
                    }
                },
                {
                    INTERFACE, () =>
                    {
                        CanvasHelper.Add(new VisualObject(new Interface(), CanvasHelper.Canvas));
                        PreparationEdit();
                    }
                },
                {
                    Properties.Resources.aggregation, () => CanvasHelper.AddRelation(Properties.Resources.aggregation)
                },
                {
                    Properties.Resources.composition, () => CanvasHelper.AddRelation(Properties.Resources.composition)
                },
                {
                    Properties.Resources.dependency, () => CanvasHelper.AddRelation(Properties.Resources.dependency)
                },
                {
                    Properties.Resources.realization, () => CanvasHelper.AddRelation(Properties.Resources.realization)
                },
                {
                    Properties.Resources.delete, () =>
                    {
                        switch (menuItemDetail.Target)
                        {
                            case Canvas canvas:
                                CanvasHelper.Remove(CanvasHelper.From);
                                break;
                            case ListViewItem lvi:
                                if (lvi.DataContext is Variable v)
                                {
                                    (Selected as Class).Variables.Remove(v);
                                    Window.Variables_LV.Items.Remove(v);
                                }
                                else if (lvi.DataContext is Method m)
                                {
                                    Selected.Methods.Remove(m);
                                    Window.Methods_LV.Items.Remove(m);
                                }
                                CanvasHelper.ReDraw();
                                break;
                        }
                    }
                }
            };
            /*Action action;
            if (actions.TryGetValue(menuItemDetail.Header, out action))
            {
                action();
            }*/
            actions[menuItemDetail.Header]();
        }

        /// <summary>
        /// Preparation the window for editing object
        /// </summary>
        private void PreparationEdit()
        {
            CanvasHelper.Clear(true);
            CanvasHelper.SelectedItem.IsCentered = true;
            CanvasHelper.ReDraw();
            CanvasHelper.SelectedItem.X -= Window.RightToolbar.Width / 2d;

            Selected = CanvasHelper.SelectedItem.Object;
            EditHelper.CurrentObject = Selected;
            NavBarHelper.EnablePanel(Window.Edit, true);
            CanvasHelper.IsEventsEnabled = false;
        }

        private void OnSelectedItemChanged(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                NavBarHelper.EnablePanel(Window.Edit);
            }
            else
            {
                NavBarHelper.DisablePanel(Window.Edit);
            }
        }

        private void OnItemDoubleClick(object sender, EventArgs e)
        {
            PreparationEdit();
        }

        private void OnNavItemClick(object sender, EventArgs e)
        {
            var title = ((sender as StackPanel).Children[1] as Label).Content.ToString();

            if (title == Properties.Resources.diagram)
            {
                AnimationHelper.Animate(Window.RightToolbar.Name);
                CanvasHelper.IsEventsEnabled = true;
                CanvasHelper.Restore();
                if (CanvasHelper.SelectedItem != null)
                {
                    CanvasHelper.SelectedItem.IsCentered = false;
                    CanvasHelper.SelectedItem.Select();
                }
            }
            else if (title == Properties.Resources.editing)
            {
                AnimationHelper.Animate(Window.RightToolbar.Name);
                if (e != null)
                {
                    EditHelper.CurrentObject = CanvasHelper.SelectedItem.Object;
                    PreparationEdit();
                }
            }
        }

        public static Core GetInstance(MainWindow window = null)
        {
            return core ?? (core = new Core(window));
        }
    }
}
