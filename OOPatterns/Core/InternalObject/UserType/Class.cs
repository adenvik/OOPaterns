using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOPatterns.Core.InternalObject.ParamObject;

namespace OOPatterns.Core.InternalObject.UserType
{
    class Class : IUserType
    {
        public const string ICO_PATH = "pack://application:,,,/OOPatterns;component/Images/class_ico.png";
        public string Name { set; get; } = "";

        List<IParamObject> variables;
        List<IParamObject> methods;
        List<IUserType> parents;

        public Class()
        {
            variables = new List<IParamObject>();
            methods = new List<IParamObject>();
            parents = new List<IUserType>();
        }

        public Class(IUserType obj) : this()
        {
            Name = obj.GetName();
            methods.AddRange(obj.GetMethods());
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetImagePath()
        {
            return ICO_PATH;
        }

        public void AddParentObj(IUserType parent)
        {
            parents.Add(parent);
        }

        public void RemoveParentObj(IUserType parent)
        {
            parents.Remove(parent);
        }

        public List<IUserType> GetParents()
        {
            return parents;
        }
        //--------------------------------------------
        public void AddVariable(Variable variable)
        {
            variables.Add(variable);
        }

        public void RemoveVariable(Variable variable)
        {
            variables.Remove(variable);
        }

        public List<IParamObject> GetVariables()
        {
            return variables;
        }
        //--------------------------------------------
        public void AddMethod(Method method)
        {
            methods.Add(method);
        }

        public void RemoveMethod(Method method)
        {
            methods.Remove(method);
        }

        public IParamObject GetMethod(int index)
        {
            return methods[index];
        }

        public List<IParamObject> GetMethods()
        {
            return methods;
        }

        public override bool Equals(object obj)
        {
            if (obj is Class c)
            {
                if (Name.Equals(c.Name)) return true;
            }
            return false;
        }
    }
}
