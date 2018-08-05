using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Windows.Controls;
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
    /// Логика взаимодействия для VariablesInMethod.xaml
    /// </summary>
    public partial class VariablesInMethodWindow : Window
    {
        Method method;

        public VariablesInMethodWindow(Method method)
        {
            InitializeComponent();
            this.method = method;
            Load();
        }

        private void Load()
        {
            method.Parameters.ForEach(p => Variables_LV.Items.Add(new
            {
                Type = p.Type,
                Name = p.Name,
                DataContext = p
            }));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddVariable_Click(object sender, RoutedEventArgs e)
        {
            var variable = new Variable();
            ParamObjectWindow window = new ParamObjectWindow(variable, true);
            window.ShowDialog();
            if (window.IsAdded)
            {
                method.Parameters.Add(variable);
                Variables_LV.Items.Add(new
                {
                    Type = variable.Type,
                    Name = variable.Name,
                    DataContext = variable
                });
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonControl;
            var variable = button.DataContext as Variable;
            var index = method.Parameters.FindIndex(v => v.Name == variable.Name);
            method.Parameters.Remove(variable);
            Variables_LV.Items.RemoveAt(index);
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception) { }
        }
    }
}
