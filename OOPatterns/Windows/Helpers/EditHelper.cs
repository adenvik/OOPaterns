using OOPatterns.Core;
using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with window, when editing object
    /// </summary>
    public class EditHelper
    {
        #region Fields
        private TextBox NameTB;
        public TextBox NameTextBox
        {
            get => NameTB;
            set
            {
                NameTB = value;
                NameTB.TextChanged += NameTB_TextChanged;
            }
        }

        private ComboBox TypeCB;
        public ComboBox TypeComboBox
        {
            get => TypeCB;
            set
            {
                TypeCB = value;
                TypeCB.SelectionChanged += TypeSelectionChanged;
            }
        }
        
        private ComboBox ParentsCB;
        public ComboBox ParentComboBox
        {
            get => ParentsCB;
            set
            {
                ParentsCB = value;
            }
        }

        private ButtonControl AddParentBtn;
        public ButtonControl AddParentButton
        {
            get => AddParentBtn;
            set
            {
                AddParentBtn = value;
                AddParentBtn.Click += AddParent_Click;
            }
        }

        private ListView ParentsLV;
        public ListView ParentsListView
        {
            get => ParentsLV;
            set
            {
                ParentsLV = value;
                ParentsLV.KeyDown += ListView_KeyDown;
            }
        }

        private ButtonControl AddVariableBtn;
        public ButtonControl AddVariableButtton
        {
            get => AddVariableBtn;
            set
            {
                AddVariableBtn = value;
                AddVariableBtn.Click += AddVariable_Click;
            }
        }

        private ListView VariablesLV;
        public ListView VariablesListView
        {
            get => VariablesLV;
            set
            {
                VariablesLV = value;
                VariablesLV.MouseDoubleClick += Variables_MouseDoubleClick;
                VariablesLV.KeyDown += ListView_KeyDown;
            }
        }

        private ButtonControl AddMethodBtn;
        public ButtonControl AddMethodButton
        {
            get => AddMethodBtn;
            set
            {
                AddMethodBtn = value;
                AddMethodBtn.Click += AddMethod_Click;
            }
        }

        private ListView MethodsLV;
        public ListView MethodsListView
        {
            get => MethodsLV;
            set
            {
                MethodsLV = value;
                MethodsLV.MouseDoubleClick += Methods_MouseDoubleClick;
                MethodsLV.KeyDown += ListView_KeyDown;
            }
        }

        private ButtonControl DeleteBtn;
        public ButtonControl DeleteButton
        {
            get => DeleteBtn;
            set
            {
                DeleteBtn = value;
                DeleteBtn.Click += Delete_Click;
            }
        }
        #endregion

        private UserType currentObject;
        /// <summary>
        /// Current object for editing
        /// </summary>
        public UserType CurrentObject
        {
            get => currentObject;
            set
            {
                currentObject = value;
                if (currentObject != null) UpdateFields();
            }
        }

        private bool onUpdate = false;

        /// <summary>
        /// Main window with controls
        /// </summary>
        private MainWindow Window;

        /// <summary>
        /// Event fired when object data changed
        /// </summary>
        public EventHandler OnDataChanged;

        public EditHelper(MainWindow window)
        {
            Window = window;

            Window.ScrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            NameTextBox = Window.NameTB;
            TypeComboBox = Window.Type_CB;
            ParentComboBox = Window.Parent_CB;
            AddParentButton = Window.AddParent;
            ParentsListView = Window.Parents_LV;
            AddVariableButtton = Window.AddVariable;
            VariablesListView = Window.Variables_LV;
            AddMethodButton = Window.AddMethod;
            MethodsListView = Window.Methods_LV;
            DeleteButton = Window.DeleteButton;

            TypeComboBox.Items.Add(Core.Core.CLASS);
            TypeComboBox.Items.Add(Core.Core.INTERFACE);
        }
        
        /// <summary>
        /// Update control source
        /// </summary>
        private void UpdateFields()
        {
            onUpdate = true;
            NameTextBox.Text = CurrentObject.Name;
            TypeComboBox.SelectedIndex = CurrentObject is Class ? 0 : 1;
            UpdateToolbar(CurrentObject.GetType().Name.ToLower());

            ParentsListView.Items.Clear();
            VariablesListView.Items.Clear();
            MethodsListView.Items.Clear();

            CurrentObject.Parents.ForEach(p => ParentsListView.Items.Add(new
            {
                Image = p.ICO_PATH,
                Name = p.Name,
                DataContext = p
            }));

            if (CurrentObject is Class c)
            {
                c.Variables.ForEach(v => VariablesListView.Items.Add(v));
            }
            CurrentObject.Methods.ForEach(m => MethodsListView.Items.Add(m));
            onUpdate = false;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void NameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (onUpdate) return;
            currentObject.Name = (sender as TextBox).Text;
            OnDataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AddVariable_Click(object sender, RoutedEventArgs e)
        {
            var variable = new Variable();
            ParamObjectWindow window = new ParamObjectWindow(variable);
            window.ShowDialog();
            if (window.IsAdded)
            {
                (currentObject as Class).Variables.Add(variable);
                VariablesListView.Items.Add(variable);
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Variables_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Variable;
            if (item != null)
            {
                ParamObjectWindow window = new ParamObjectWindow(item);
                window.ShowDialog();
                if (!window.IsAdded)
                {
                    (currentObject as Class).Variables.Remove(item);
                    VariablesListView.Items.Remove(item);
                }
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AddMethod_Click(object sender, RoutedEventArgs e)
        {
            var method = new Method();
            ParamObjectWindow window = new ParamObjectWindow(method);
            window.ShowDialog();
            if (window.IsAdded)
            {
                currentObject.Methods.Add(method);
                MethodsListView.Items.Add(method);
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Methods_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Method;
            if (item != null)
            {
                ParamObjectWindow window = new ParamObjectWindow(item);
                window.ShowDialog();
                if (!window.IsAdded)
                {
                    currentObject.Methods.Remove(item);
                    MethodsListView.Items.Remove(item);
                }
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void TypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (onUpdate) return;
            bool change = true;
            if (CurrentObject is Class && (CurrentObject as Class).Variables.Count > 0)
            {
                change = ShowYesNoDialog("Change", "To interface");
            }
            if (change)
            {
                switch (TypeComboBox.SelectedItem.ToString())
                {
                    case Core.Core.CLASS:
                        CurrentObject = CurrentObject.ToClass();
                        UpdateToolbar(Core.Core.CLASS);
                        break;
                    case Core.Core.INTERFACE:
                        CurrentObject = CurrentObject.ToInterface();
                        UpdateToolbar(Core.Core.INTERFACE);
                        break;
                }
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TypeComboBox.SelectionChanged -= TypeSelectionChanged;
                TypeComboBox.SelectedIndex = CurrentObject is Class ? 0 : 1;
                TypeComboBox.SelectionChanged += TypeSelectionChanged;
            }
        }

        private void AddParent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            CurrentObject = null;
            OnDataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                var senderName = (sender as ListView).Name;
                if (senderName == ParentsListView.Name)
                {
                    dynamic selectedItem = ParentsListView.SelectedItem;
                    if (selectedItem == null) return;
                    ParentsListView.Items.Remove(selectedItem);

                    CurrentObject.Parents.Remove(selectedItem.DataContext);
                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                }
                else if(senderName == VariablesListView.Name)
                {
                    var selectedItem = VariablesListView.SelectedItem as Variable;
                    if (selectedItem == null) return;
                    VariablesListView.Items.Remove(selectedItem);

                    (CurrentObject as Class).Variables.Remove(selectedItem);
                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                }
                else if(senderName == MethodsListView.Name)
                {
                    var selectedItem = MethodsListView.SelectedItem as Method;
                    if (selectedItem == null) return;
                    MethodsListView.Items.Remove(selectedItem);

                    CurrentObject.Methods.Remove(selectedItem);
                    OnDataChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Show dialog with YesNo buttons
        /// </summary>
        /// <param name="title">Title of dialog</param>
        /// <param name="message">Message of dialog</param>
        /// <returns>User press Yes</returns>
        private bool ShowYesNoDialog(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        private void UpdateToolbar(string type)
        {
            GridView gridView = new GridView();
            switch (type)
            {
                case Core.Core.CLASS:
                    (Window.Variables.Parent as FrameworkElement).Visibility = Visibility.Visible;

                    gridView.Columns.Add(CreateColumn("Access", Properties.Resources.access, 50));
                    gridView.Columns.Add(CreateColumn("Type", Properties.Resources.type, 50));
                    gridView.Columns.Add(CreateColumn("Name", Properties.Resources.name, 100));
                    break;
                case Core.Core.INTERFACE:
                    (Window.Variables.Parent as FrameworkElement).Visibility = Visibility.Collapsed;

                    gridView.Columns.Add(CreateColumn("Type", Properties.Resources.type, 70));
                    gridView.Columns.Add(CreateColumn("Name", Properties.Resources.name, 130));
                    break;
            }
            //MethodsListView.View = gridView;
        }

        private GridViewColumn CreateColumn(string bindPath, string header, double width)
        {
            return new GridViewColumn
            {
                DisplayMemberBinding = new Binding(bindPath),
                Header = header,
                Width = width
            };
        }
    }
}
