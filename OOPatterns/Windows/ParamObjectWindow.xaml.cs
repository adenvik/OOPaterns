using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOPatterns.Windows
{
    /// <summary>
    /// Логика взаимодействия для ParamObjectWindow.xaml
    /// </summary>
    public partial class ParamObjectWindow : Window
    {
        IParamObject paramObject;
        public bool IsAdded = false;

        public ParamObjectWindow(IParamObject obj, bool inMethod = false)
        {
            InitializeComponent();
            paramObject = obj;
            Load();

            switch (obj)
            {
                case Variable v:
                    this.Width = 350;
                    this.Height = 65;
                    Title.Content = Properties.Resources.add_variable;
                    TypeCB.Items.Remove(Core.Helpers.Type.VOID);
                    if (TypeCB.SelectedIndex == -1) TypeCB.SelectedIndex = 0;
                    break;
                case Method m:
                    this.Width = 500;
                    this.Height = 65;
                    Title.Content = Properties.Resources.add_method;
                    break;
            }
            if (inMethod)
            {
                AccessCB.Visibility = Visibility.Collapsed;
                this.Width -= 70;
            }

        }

        private void Load()
        {
            foreach(string access in Access.GetAccess())
            {
                AccessCB.Items.Add(access);
            }

            foreach(string type in Core.Helpers.Type.GetTypes())
            {
                TypeCB.Items.Add(type);
            }
            TypeCB.SelectedIndex = 0;

            if (paramObject.Access != null) AccessCB.SelectedItem = paramObject.Access;
            else AccessCB.SelectedIndex = 0;

            if (paramObject.Type != null) TypeCB.SelectedItem = paramObject.Type;
            else TypeCB.SelectedIndex = 0;

            NameTB.Text = paramObject.Name;

            UpdateVariablesFields();
        }

        private void UpdateVariablesFields()
        {
            if (paramObject is Method m)
            {
                if (m.Parameters.Count > 0)
                {
                    AddVariable.Visibility = Visibility.Collapsed;
                    VariablesTB.Visibility = Visibility.Visible;
                    VariablesTB.Width = 140;
                    VariablesTB.Text = "";
                    string result = "";
                    m.Parameters.ForEach(p => result += $"{p.Type} {p.Name}, ");
                    result = result.Substring(0, result.Length - 2);
                    VariablesTB.Text = result;
                }
                else
                {
                    VariablesTB.Visibility = Visibility.Collapsed;
                    VariablesTB.Width = 0;
                    AddVariable.Visibility = Visibility.Visible;
                }
            }
        }

        private void Variables_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                VariablesInMethodWindow window = new VariablesInMethodWindow(paramObject as Method);
                window.ShowDialog();
                UpdateVariablesFields();
            }
        }

        private void AddVariable_Click(object sender, RoutedEventArgs e)
        {
            VariablesInMethodWindow window = new VariablesInMethodWindow(paramObject as Method);
            window.ShowDialog();
            UpdateVariablesFields();
        }

        private bool Validate()
        {
            var name = NameTB.Text;
            if (SystemName.Check(name))
            {
                MessageBox.Show(Properties.Resources.systemNameAlert);
                return (IsAdded = false);
            }
            if (name == "")
            {
                IsAdded = MessageBox.Show(Properties.Resources.nameIsEmpty, Properties.Resources.closeWithoutSaving, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No;
                return !IsAdded;
            }
            return (IsAdded = true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            paramObject.Access = AccessCB.Text;
            paramObject.Type = TypeCB.Text;
            paramObject.Name = NameTB.Text;
            e.Cancel = !Validate();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid) DragMove();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Close();
            }
        }
    }
}
