using OOPatterns.Core.Helpers;
using OOPatterns.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OOPatterns
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core.Core core;
        CanvasHelper canvasHelper;
        Point startPosition;
        bool IsMaximized = false;

        private bool isDown = false;

        public MainWindow()
        {
            InitializeComponent();
            core = Core.Core.GetInstance();
            InitCanvas();
        }

        private void InitCanvas()
        {
            canvasHelper = new CanvasHelper(ElementsView);
            Load();
        }

        private void Load()
        {
            core.Objects.ForEach(obj => canvasHelper.Add(obj.VisualObject));
        }

        private void configurationButton_Click(object sender, RoutedEventArgs e)
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
        }

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

        private void Nav_MouseEnter(object sender, MouseEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = Brushes.AliceBlue;
            (panel.Children[1] as Label).Foreground = Brushes.Black;
        }

        private void Nav_MouseLeave(object sender, MouseEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = Brushes.Transparent;
            (panel.Children[1] as Label).Foreground = Brushes.White;
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).ContextMenu.IsOpen = true;
        }
    }
}
