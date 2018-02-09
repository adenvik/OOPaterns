using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Modificators;
using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOPatterns.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserTypeConfigurationWindow.xaml
    /// </summary>
    public partial class UserTypeConfigurationWindow : Window
    {
        Core.InternalObject.Core core;
        Access access;
        Core.Utils.Type.Type type;
        CanvasWorker canvasWorker;

        //IUserType obj;
        Tuple<IUserType, IVisualObject> obj;

        public UserTypeConfigurationWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public UserTypeConfigurationWindow(Tuple<IUserType, IVisualObject> obj)
        {
            InitializeComponent();
            this.obj = obj;
            Initialize();
        }

        private void AddVariableButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = obj is Class ? ShowAddParamObjectWindow() : ShowAddParamObjectWindow(false);
            if (addParamObjectWindow.isClosed)
            {
                Variable variable = new Variable(addParamObjectWindow.Access_CB.Text,
                                                 type[addParamObjectWindow.Type_CB.Text],
                                                 addParamObjectWindow.Name_TB.Text);
                obj.Item1.AddVariable(variable);
                obj.Item2.UpdateFigure();
                Variables_LB.Items.Add(variable);
            }
        }

        private void AddMethodButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = ShowAddParamObjectWindow();
            if (addParamObjectWindow.isClosed)
            {
                Method method = new Method(addParamObjectWindow.Access_CB.Text,
                                           type[addParamObjectWindow.Type_CB.Text],
                                           addParamObjectWindow.Name_TB.Text);
                obj.Item1.AddMethod(method);
                obj.Item2.UpdateFigure();
                Methods_LB.Items.Add(method);
            }
        }

        private void AddVariableToMethodButton_Click(object sender, RoutedEventArgs e)
        {
            AddParamObjectWindow addParamObjectWindow = ShowAddParamObjectWindow();
            if (addParamObjectWindow.isClosed)
            {
                Variable variable = new Variable(addParamObjectWindow.Access_CB.Text,
                                                 type[addParamObjectWindow.Type_CB.Text],
                                                 addParamObjectWindow.Name_TB.Text);

                /*Method method = null; //TODO: Получение текущего метода
                dynamic currentMethod = Methods_LB.SelectedItem;
                method.AccessObject = access[currentMethod.Access];
                method.TypeObject = type[currentMethod.Type];*/
                Method method = obj.Item1.GetMethod(Methods_LB.SelectedIndex) as Method;
                method.AddVariable(variable);
                obj.Item2.UpdateFigure();
                VariablesInMethod_LB.Items.Add(variable);
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
            canvasWorker = new CanvasWorker(ElementView);

            core = Core.InternalObject.Core.GetInstance();

            type = core.Type;
            access = core.Access;

            new List<string>() { Core.InternalObject.Core.CLASS, Core.InternalObject.Core.INTERFACE }
                                .ForEach(item => TypeObj_CB.Items.Add(item));
            List<IUserType> objects = core.GetObjects();
            if (objects.Count == 0) ParentObj_CB.IsEnabled = false;
            objects.ForEach(item => ParentObj_CB.Items.Add(new {
                ImagePath = (item is Class) ? Class.ICO_PATH : Interface.ICO_PATH,
                Name = item.GetName()
            }));

            if(obj == null)
            {
                Variables_GB.IsEnabled = false;
                Methods_GB.IsEnabled = false;
                return;
            }
            canvasWorker.AddElement(obj.Item2);

            ParentObj_CB.Items.Remove(obj);
            
            LoadParams();
        }

        private void LoadParams()
        {
            ParentsObj_LB.Items.Clear();
            List<IUserType> list = obj.Item1.GetParents();
            list.ForEach(item => ParentsObj_LB.Items.Add(item));

            Variables_LB.Items.Clear();
            Methods_LB.Items.Clear();

            obj.Item1.GetVariables().ForEach(item => Variables_LB.Items.Add(item));
            obj.Item1.GetMethods().ForEach(item => Methods_LB.Items.Add(item));
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

            obj.Item1.RemoveVariable((Variable)item);
            obj.Item2.UpdateFigure();
            Variables_LB.Items.Remove(item);
        }

        private void DeleteMethodButton_Click(object sender, RoutedEventArgs e)
        {
            var item = Methods_LB.SelectedItem;
            if (item == null) return;

            obj.Item1.RemoveMethod((Method)item);
            obj.Item2.UpdateFigure();
            Methods_LB.Items.Remove(item);
        }

        private void DeleteVariableInMethodButton_Click(object sender, RoutedEventArgs e)
        {
            Method method = obj.Item1.GetMethod(Methods_LB.SelectedIndex) as Method;
            var item = VariablesInMethod_LB.SelectedItem;
            if (item == null) return;

            method.RemoveVariable((Variable)item);
            obj.Item2.UpdateFigure();
            VariablesInMethod_LB.Items.Remove(item);
        }

        private void Methods_LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VariablesInMethod_LB.Items.Clear();
            if (Methods_LB.SelectedIndex != -1)
            {
                VariablesInMethod_GB.IsEnabled = true;

                Method method = obj.Item1.GetMethod(Methods_LB.SelectedIndex) as Method;
                method.Variables.ForEach(item => VariablesInMethod_LB.Items.Add(item));
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
                gridView.Columns.Add(CreateColumn("AccessObject", Properties.Resources.type, 50));
                gridView.Columns.Add(CreateColumn("TypeObject.TypeName", Properties.Resources.type, 50));
                gridView.Columns.Add(CreateColumn("NameObject", Properties.Resources.name, 100));
            }
            //interface
            else
            {
                gridView.Columns.Add(CreateColumn("TypeObject.TypeName", Properties.Resources.type, 70));
                gridView.Columns.Add(CreateColumn("NameObject", Properties.Resources.name, 130));
            }
            Methods_LB.View = gridView;
            TryToInitializeObject(type);
        }

        private void TryToInitializeObject(string type)
        {
            if (obj == null)
            {
                if (type == Core.InternalObject.Core.CLASS)
                {
                    core.AddObject(new Class());
                }
                else
                {
                    core.AddObject(new Interface());
                }
                obj = core.Last();
                obj.Item1.SetName(Name_TB.Text);
                canvasWorker.AddElementToCenter(obj.Item2);
                canvasWorker.Draw();
                return;
            }
            if (obj.Item1 is Class && type == Core.InternalObject.Core.INTERFACE)
            {
                if (!ShowYesNoDialog("change", "to interface"))
                {
                    core.Remove(obj.Item1);
                    core.AddObject(new Interface(obj.Item1));
                    canvasWorker.RemoveElement(obj.Item2);

                    obj = core.Last();
                    canvasWorker.AddElementToCenter(obj.Item2);
                    LoadParams();
                }
                else TypeObj_CB.SelectedIndex = 0;
            }
            else if (obj.Item1 is Interface && type == Core.InternalObject.Core.CLASS)
            {
                if (!ShowYesNoDialog("change", "to class"))
                {
                    core.Remove(obj.Item1);
                    core.AddObject(new Class(obj.Item1));
                    canvasWorker.RemoveElement(obj.Item2);

                    obj = core.Last();
                    canvasWorker.AddElementToCenter(obj.Item2);
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
            if (obj == null) return false;
            obj.Item1.SetName(Name_TB.Text);
            return !obj.Item1.GetName().Equals("");
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
