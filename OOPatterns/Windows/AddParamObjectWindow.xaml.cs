using OOPatterns.Core.Helpers;
using System.Linq;
using System.Windows;

namespace OOPatterns.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddVariableWindow.xaml
    /// </summary>
    public partial class AddParamObjectWindow : Window
    {
        public bool isClosed = false;

        public AddParamObjectWindow()
        {
            InitializeComponent();
        }

        public AddParamObjectWindow(bool withAccess) : this()
        {
            if (!withAccess)
            {
                Access_Control.Height = 0;
                Others_Control.Margin = new Thickness(0, 8, 0, 0);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string type = Type_CB.Text;
            string name = Name_TB.Text;

            if (!SystemName.Check(name))
            {
                //MessageBox();
                return;
            }

            isClosed = true;

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Access.GetAccess().ToList().ForEach(item => Access_CB.Items.Add(item));
            Core.Helpers.Type.GetTypes().ToList().ForEach(item => Type_CB.Items.Add(item));

            Access_CB.SelectedIndex = 0;
            Type_CB.SelectedIndex = 0;
        }
    }
}
