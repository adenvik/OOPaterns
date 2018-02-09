using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.Utils.Exceptions;

namespace OOPatterns.Core.InternalObject.UserType
{
    class Interface : IUserType
    {
        public const string ICO_PATH = "pack://application:,,,/OOPatterns;component/Images/interface_ico.png";
        public string Name { set; get; } = "";

        List<IParamObject> methods;
        List<IUserType> parents;

        public Interface()
        {
            methods = new List<IParamObject>();
            parents = new List<IUserType>();
        }

        public Interface(IUserType obj) : this()
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
            //throw new OOPatternsException();
        }

        public void RemoveVariable(Variable variable)
        {
            //throw new OOPatternsException();
        }

        public List<IParamObject> GetVariables()
        {
            //throw new OOPatternsException();
            return new List<IParamObject>();
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
            if (obj is Interface c)
            {
                if (Name.Equals(c.Name)) return true;
            }
            return false;
        }
    }
}
