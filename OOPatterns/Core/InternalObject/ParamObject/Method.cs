using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    class Method : IParamObject
    {
        public List<Variable> Variables { get; }
        public string Body { get; set; }

        Method()
        {
            Variables = new List<Variable>();
        }

        public Method(int Type, string Name) : this()
        {
            TypeObject = Type;
            NameObject = Name;
        }

        public Method(int Access, int Type, string Name) : this(Type, Name)
        {
            AccessObject = Access;
        }

        public void AddVariable(Variable variable)
        {
            Variables.Add(variable);
        }

        public void RemoveVariable(Variable variable)
        {
            Variables.Remove(variable);
        }

        public void RemoveVariableAt(int index)
        {
            if (index < Variables.Count)
            {
                Variables.RemoveAt(index);
            }
        }
    }
}
