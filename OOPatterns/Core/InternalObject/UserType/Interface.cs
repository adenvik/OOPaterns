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
        public string Name { set; get; } = "";

        List<Method> methods;
        List<IUserType> parents;

        public Interface()
        {
            methods = new List<Method>();
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

        public List<Variable> GetVariables()
        {
            //throw new OOPatternsException();
            return new List<Variable>();
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

        public Method GetMethod(int index)
        {
            return methods[index];
        }

        public List<Method> GetMethods()
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
