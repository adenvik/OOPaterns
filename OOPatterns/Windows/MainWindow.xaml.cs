using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects;
using OOPatterns.Windows;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOPatterns
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core.InternalObject.Core core;
        CanvasWorker canvasWorker;

        private bool isDown = false;

        public MainWindow()
        {
            InitializeComponent();

            core = Core.InternalObject.Core.GetInstance();
            InitCanvas();
        }

        private void InitCanvas()
        {
            canvasWorker = new CanvasWorker(ElementsView);
            UpdateElements();
        }

        private void UpdateElements()
        {
            canvasWorker.Clear();
            core.GetVisualObjects().ForEach(obj => canvasWorker.AddElement(obj.CenteredObject(false)));
        }

        private void configurationButton_Click(object sender, RoutedEventArgs e)
        {
            UserTypeConfigurationWindow w = new UserTypeConfigurationWindow();
            w.ShowDialog();
            UpdateElements();
        }

        /*
         * TODO: Заменить
         */
        private void ElementsView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
            if (!(e.OriginalSource is Canvas))
            {
                (e.OriginalSource as FrameworkElement).ReleaseMouseCapture();
            }

        }
        
        Point startPosition;
        private void ElementsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Canvas))
            {
                isDown = true;
                startPosition = e.GetPosition(ElementsView);
                var elem = e.OriginalSource as FrameworkElement;
                elem.CaptureMouse();
                canvasWorker.SelectElement(elem.Name);
            }
        }

        private void ElementsView_MouseMove(object sender, MouseEventArgs e)
        {
            Point endPosition = e.GetPosition(ElementsView);
            State.Content = $"{endPosition.X} {endPosition.Y}";
            if (!(e.OriginalSource is Canvas))
            {
                if (isDown)
                {
                    canvasWorker.MoveElement(startPosition, endPosition);
                    startPosition = endPosition;
                }
            }
        }
    }
}
