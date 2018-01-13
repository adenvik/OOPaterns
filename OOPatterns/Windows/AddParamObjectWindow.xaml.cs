using OOPatterns.Core.InternalObject;
using OOPatterns.Core.Utils.Name;
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
using System.Windows.Shapes;

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

            if (SystemName.Check(name))
            {
                //MessageBox();
                return;
            }

            isClosed = true;

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Core.InternalObject.Core core = Core.InternalObject.Core.GetInstance();

            core.Access.GetAccess().ForEach(item => Access_CB.Items.Add(item));
            core.Type.GetTypes().ForEach(item => Type_CB.Items.Add(item.TypeName));

            Access_CB.SelectedIndex = 0;
            Type_CB.SelectedIndex = 0;
        }
    }
}
