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
        public string Name { set; get; } = "";

        List<Variable> variables;
        List<Method> methods;
        List<IUserType> parents;

        public Class()
        {
            variables = new List<Variable>();
            methods = new List<Method>();
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

        public List<Variable> GetVariables()
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

        public Method GetMethod(int index)
        {
            return methods[index];
        }

        public List<Method> GetMethods()
        {
            return methods;
        }
    }
}
