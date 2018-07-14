using OOPatterns.Core;
using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OOPatterns.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserTypeConfigurationWindow.xaml
    /// </summary>
    public partial class UserTypeConfigurationWindow : Window
    {
        Core.Core core;
        CanvasHelper canvasHelper;
        Element element;

        public UserTypeConfigurationWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public UserTypeConfigurationWindow(Element element)
        {
            InitializeComponent();
            this.element = element;
            Initialize();
        }

        private void AddVariableButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = element.UserObject is Class ? ShowAddParamObjectWindow() : ShowAddParamObjectWindow(false);
            if (addParamObjectWindow.isClosed)
            {
                Variable variable = new Variable()
                {
                    Access = addParamObjectWindow.Access_CB.Text,
                    Type = addParamObjectWindow.Type_CB.Text,
                    Name = addParamObjectWindow.Name_TB.Text
                };
                (element.UserObject as Class).Variables.Add(variable);
                Variables_LB.Items.Add(variable);
                canvasHelper.ReDraw(element.VisualObject);
            }
        }

        private void AddMethodButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = ShowAddParamObjectWindow();
            if (addParamObjectWindow.isClosed)
            {
                Method method = new Method()
                {
                    Access = addParamObjectWindow.Access_CB.Text,
                    Type = addParamObjectWindow.Type_CB.Text,
                    Name = addParamObjectWindow.Name_TB.Text
                };
                element.UserObject.Methods.Add(method);
                Methods_LB.Items.Add(method);
                canvasHelper.ReDraw(element.VisualObject);
            }
        }

        private void AddVariableToMethodButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = ShowAddParamObjectWindow();
            if (addParamObjectWindow.isClosed)
            {
                Variable variable = new Variable()
                {
                    Access = addParamObjectWindow.Access_CB.Text,
                    Type = addParamObjectWindow.Type_CB.Text,
                    Name = addParamObjectWindow.Name_TB.Text
                };

                var method = element.UserObject.Methods[Methods_LB.SelectedIndex] as Method;
                method.Parameters.Add(variable);
                canvasHelper.ReDraw(element.VisualObject);
            }
        }

        private void AddParentObjButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParentObj_CB.SelectedIndex == 0)
            {
                //MessageBox
                return;
            }
            //obj.AddParentObj()
        }

        /// <summary>
        /// Initialization form params
        /// </summary>
        private void Initialize()
        {
            canvasHelper = new CanvasHelper(ElementView);
            core = Core.Core.GetInstance();

            new List<string>() { Core.Core.CLASS, Core.Core.INTERFACE }
                                .ForEach(item => TypeObj_CB.Items.Add(item));

            var objects = core.Objects;
            if (objects.Count == 0) ParentObj_CB.IsEnabled = false;
            objects.ForEach(item => ParentObj_CB.Items.Add(new
            {
                ImagePath = item.UserObject.ICO_PATH,
                Name = item.UserObject.Name
            }));

            if (element == null)
            {
                Variables_GB.IsEnabled = false;
                Methods_GB.IsEnabled = false;
                return;
            }

            canvasHelper.Add(element.VisualObject, true);
            ParentObj_CB.Items.RemoveAt(objects.FindIndex(o => o.Equals(element.UserObject)));
            LoadParams();
        }

        private void LoadParams()
        {
            Name_TB.Text = element.UserObject.Name;
            TypeObj_CB.SelectedIndex = (element.UserObject is Class) ? 0 : 1;

            ParentsObj_LB.Items.Clear();
            List<UserType> list = element.UserObject.Parents;
            list.ForEach(item => ParentsObj_LB.Items.Add(item));

            Variables_LB.Items.Clear();
            Methods_LB.Items.Clear();

            (element.UserObject as Class)?.Variables.ForEach(item => Variables_LB.Items.Add(item));
            element.UserObject.Methods.ForEach(item => Methods_LB.Items.Add(item));
        }

        private AddParamObjectWindow ShowAddParamObjectWindow(bool withAccess = true)
        {
            AddParamObjectWindow addParamObjectWindow = new AddParamObjectWindow(withAccess);
            addParamObjectWindow.Owner = this;
            addParamObjectWindow.ShowDialog();

            return addParamObjectWindow;
        }

        private void DeleteVariableButton_Click(object sender, RoutedEventArgs e)
        {
            var item = Variables_LB.SelectedItem;
            if (item == null) return;

            (element.UserObject as Class)?.Variables.Remove((Variable)item);
            canvasHelper.ReDraw(element.VisualObject);
            Variables_LB.Items.Remove(item);
        }

        private void DeleteMethodButton_Click(object sender, RoutedEventArgs e)
        {
            var item = Methods_LB.SelectedItem;
            if (item == null) return;

            element.UserObject.Methods.Remove((Method)item);
            canvasHelper.ReDraw(element.VisualObject);
            Methods_LB.Items.Remove(item);
        }

        private void DeleteVariableInMethodButton_Click(object sender, RoutedEventArgs e)
        {
            var method = element.UserObject.Methods[Methods_LB.SelectedIndex] as Method;
            var item = VariablesInMethod_LB.SelectedItem;
            if (item == null) return;

            method.Parameters.Remove((Variable)item);
            canvasHelper.ReDraw(element.VisualObject);
            VariablesInMethod_LB.Items.Remove(item);
        }

        private void Methods_LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VariablesInMethod_LB.Items.Clear();
            if (Methods_LB.SelectedIndex != -1)
            {
                VariablesInMethod_GB.IsEnabled = true;

                Method method = element.UserObject.Methods[Methods_LB.SelectedIndex] as Method;
                method.Parameters.ForEach(item => VariablesInMethod_LB.Items.Add(item));
            }
            else
            {
                VariablesInMethod_GB.IsEnabled = false;
            }
        }

        private void TypeObj_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TypeObj_CB.SelectedItem)
            {
                case Core.InternalObject.Core.CLASS:
                    Variables_GB.IsEnabled = true;
                    Methods_GB.IsEnabled = true;
                    ChangeType();
                    break;
                case Core.InternalObject.Core.INTERFACE:
                    Variables_GB.IsEnabled = false;
                    Methods_GB.IsEnabled = true;
                    ChangeType(Core.InternalObject.Core.INTERFACE);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ChangeType(string type = Core.InternalObject.Core.CLASS)
        {
            GridView gridView = new GridView();
            //class
            if (type == Core.InternalObject.Core.CLASS)
            {
                gridView.Columns.Add(CreateColumn("Access", Properties.Resources.type, 50));
                gridView.Columns.Add(CreateColumn("Type", Properties.Resources.type, 50));
                gridView.Columns.Add(CreateColumn("Name", Properties.Resources.name, 100));
            }
            //interface
            else
            {
                gridView.Columns.Add(CreateColumn("Type", Properties.Resources.type, 70));
                gridView.Columns.Add(CreateColumn("Name", Properties.Resources.name, 130));
            }
            Methods_LB.View = gridView;
            TryToInitializeObject(type);
        }

        private void TryToInitializeObject(string type)
        {
            if (element == null)
            {
                element = new Element();
                if (type == Core.Core.CLASS)
                {
                    element.VisualObject = new VisualObject(new Class(Name_TB.Text), canvasHelper.Canvas);
                }
                else
                {
                    element.VisualObject = new VisualObject(new Interface(Name_TB.Text), canvasHelper.Canvas);
                }
                core.Objects.Add(element);
                canvasHelper.Add(element.VisualObject, true);
                return;
            }
            if (element.UserObject is Class && type == Core.Core.INTERFACE)
            {
                if (!ShowYesNoDialog("change", "to interface"))
                {
                    element.UserObject = element.UserObject.ToInterface();
                    canvasHelper.ReDraw(element.VisualObject);
                    LoadParams();
                }
                else TypeObj_CB.SelectedIndex = 0;
            }
            else if (element.UserObject is Interface && type == Core.Core.CLASS)
            {
                if (!ShowYesNoDialog("change", "to class"))
                {
                    element.UserObject = element.UserObject.ToClass();
                    canvasHelper.ReDraw(element.VisualObject);
                    LoadParams();
                }
                else TypeObj_CB.SelectedIndex = 1;
            }
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

        /// <summary>
        /// Show dialog with YesNo buttons
        /// </summary>
        /// <param name="title">Title of dialog</param>
        /// <param name="message">Message of dialog</param>
        /// <returns>User press No</returns>
        private bool ShowYesNoDialog(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No;
        }

        /// <summary>
        /// Validate object, before the closing
        /// </summary>
        /// <returns></returns>
        private bool ObjectValidate()
        {
            if (element == null) return false;
            element.UserObject.Name = Name_TB.Text;
            return !element.UserObject.Name.Equals("");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ObjectValidate())
            {
                string title = Properties.Resources.save_object_question;
                string message = Properties.Resources.save_object_message;
                e.Cancel = ShowYesNoDialog(title, message);
                if (!e.Cancel)
                {
                    //core.Remove(obj.Item1);
                    ElementView.Children.Clear();
                }
                return;
            }
            ElementView.Children.Clear();
        }
    }
}
