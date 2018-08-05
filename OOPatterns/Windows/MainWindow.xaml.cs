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
        Windows.Helpers.CanvasHelper canvasHelper;
        AnimationHelper animationHelper;
        NavBarHelper navBarHelper;
        ContextMenuHelper contextMenuHelper;
        Element current = null;

        bool IsMaximized = false;

        public MainWindow()
        {
            InitializeComponent();
            core = Core.Core.GetInstance();

            navBarHelper = new NavBarHelper(Diagram, Edit);
            canvasHelper = new Windows.Helpers.CanvasHelper(ElementsView);
            canvasHelper.OnSelectedChanges += (s, e) =>
            {
                if ((bool)s)
                {
                    navBarHelper.EnablePanel(Edit);
                }
                else
                {
                    navBarHelper.DisablePanel(Edit);
                }
            };
            animationHelper = new AnimationHelper();
            contextMenuHelper = new ContextMenuHelper(ElementsViewContextMenuItem_Click);

            PreparingAnimation();
            ElementsView.ContextMenu = contextMenuHelper.StandartElementsViewContextMeny;

            navBarHelper.OnNavItemClick += (sender, e) =>
            {
                var title = ((sender as StackPanel).Children[1] as Label).Content.ToString();
                if (title == Properties.Resources.diagram)
                {
                    animationHelper.Animate(RightToolbar.Name);
                    ElementsView.ContextMenu = contextMenuHelper.StandartElementsViewContextMeny;
                    canvasHelper.IsEventsEnabled = true;
                }
                else if (title == Properties.Resources.editing)
                {
                    animationHelper.Animate(RightToolbar.Name);
                }
            };
            
            Type_CB.Items.Add(Core.Core.CLASS);
            Type_CB.Items.Add(Core.Core.INTERFACE);
        }

        private void Load()
        {
            core.Objects.ForEach(obj => canvasHelper.Add(obj.VisualObject));
        }

        /*private void configurationButton_Click(object sender, RoutedEventArgs e)
        {
            UserTypeConfigurationWindow w = new UserTypeConfigurationWindow();
            w.ShowDialog();
            //Load last elements
            if (w.IsAdded)
            {
                var obj = core.Objects[core.Objects.Count - 1];
                obj.VisualObject = new Core.VisualObjects.VisualObject(obj.UserObject, ElementsView);
                canvasHelper.Add(obj.VisualObject);
            }
        }
        
        private void ElementsView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
            if (!(e.OriginalSource is Canvas))
            {
                (e.OriginalSource as FrameworkElement).ReleaseMouseCapture();
            }

        }
        
        private void ElementsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Canvas))
            {
                isDown = true;
                startPosition = e.GetPosition(ElementsView);
                var elem = e.OriginalSource as FrameworkElement;
                elem.CaptureMouse();
                canvasHelper.Select(elem.Name);
                if (e.ClickCount == 2)
                {
                    canvasHelper.SelectedItem.DestroyOnCanvas();
                    ElementsView.Children.Clear();
                    var selectedObj = core.Objects.Find(obj => obj.VisualObject.Name == canvasHelper.SelectedItem.Name);
                    canvasHelper.Remove(selectedObj.VisualObject);

                    UserTypeConfigurationWindow w = new UserTypeConfigurationWindow(selectedObj);
                    w.ShowDialog();

                    isDown = false;
                    elem.ReleaseMouseCapture();
                    //UpdateElements
                    if (w.IsAdded)
                    {
                        selectedObj.VisualObject = new Core.VisualObjects.VisualObject(selectedObj.UserObject, ElementsView);
                        canvasHelper.Add(selectedObj.VisualObject);
                        selectedObj.VisualObject.Select();
                        selectedObj.VisualObject.MoveTo(startPosition.X, startPosition.Y);
                    }
                }
            }
            else
            {
                canvasHelper.Deselect();
            }
        }

        private void ElementsView_MouseMove(object sender, MouseEventArgs e)
        {
            Point endPosition = e.GetPosition(ElementsView);
            State.Content = $"x:{endPosition.X} y:{endPosition.Y}";
            if (!(e.OriginalSource is Canvas))
            {
                if (isDown)
                {
                    canvasHelper.SelectedItem.Move(startPosition, endPosition);
                    startPosition = endPosition;
                }
            }
        }*/

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TopToolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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

        private void Panel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            animationHelper.Animate(((sender as FrameworkElement).Parent as FrameworkElement).Name);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void PreparingAnimation()
        {
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                ThicknessProperty = AnimationHelper.AnimationDetails.ThicknessAnimationProperty.Right,
                Key = RightToolbar.Name,
                StartValue = 0,
                EndValue = 200,
                AnimateProperty = MarginProperty,
                Element = ElementsView
            });
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                ThicknessProperty = AnimationHelper.AnimationDetails.ThicknessAnimationProperty.Right,
                Key = RightToolbar.Name,
                StartValue = -201,
                EndValue = 0,
                AnimateProperty = MarginProperty,
                Element = RightToolbar
            });

            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Parents.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = HeightProperty,
                Element = Parents
            });
            Parents_Arrow.RenderTransform = new RotateTransform(0);
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Parents.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Parents_Arrow.RenderTransform
            });
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Variables.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = HeightProperty,
                Element = Variables
            });
            Variables_Arrow.RenderTransform = new RotateTransform(0);
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Variables.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Variables_Arrow.RenderTransform
            });
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Methods.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = HeightProperty,
                Element = Methods
            });
            Methods_Arrow.RenderTransform = new RotateTransform(0);
            animationHelper.Add(new AnimationHelper.AnimationDetails
            {
                Key = Methods.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Methods_Arrow.RenderTransform
            });
        }

        private void AddVariable_Click(object sender, RoutedEventArgs e)
        {
            var variable = new Variable();
            ParamObjectWindow window = new ParamObjectWindow(variable);
            window.ShowDialog();
            if (window.IsAdded)
            {
                (current.UserObject as Class).Variables.Add(variable);
                Variables_LV.Items.Add(variable);
                canvasHelper.ReDraw(current.VisualObject);
            }
        }

        private void AddMethod_Click(object sender, RoutedEventArgs e)
        {
            var method = new Method();
            ParamObjectWindow window = new ParamObjectWindow(method);
            window.ShowDialog();
            if (window.IsAdded)
            {
                current.UserObject.Methods.Add(method);
                Methods_LV.Items.Add(method);
                canvasHelper.ReDraw(current.VisualObject);
            }
        }

        private void ElementsViewContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var header = (sender as MenuItem).Header.ToString();
            
            switch (header)
            {
                case Core.Core.CLASS:
                    Type_CB.SelectedIndex = 0;
                    current = new Element
                    {
                        VisualObject = new VisualObject(new Class(), ElementsView)
                    };
                    break;
                case Core.Core.INTERFACE:
                    Type_CB.SelectedIndex = 1;
                    current = new Element
                    {
                        VisualObject = new VisualObject(new Interface(), ElementsView)
                    };
                    break;
            }
            navBarHelper.EnablePanel(Edit).Navigate(Edit);
            canvasHelper.Add(current.VisualObject, true);
            current.VisualObject.X -= RightToolbar.Width / 2d;
            NameTB.Focus();
            NameTB.Text = current.UserObject.Name;
            ElementsView.ContextMenu = null;
            canvasHelper.IsEventsEnabled = false;
        }
    }
}
