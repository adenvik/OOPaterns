using OOPatterns.Core.Utils.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    public class Method : IParamObject
    {
        public List<Variable> Variables { get; }
        public string Body { get; set; }

        Method()
        {
            Variables = new List<Variable>();
        }

        public Method(TypeObject Type, string Name) : this()
        {
            TypeObject = Type;
            NameObject = Name;
        }

        public Method(string Access, TypeObject Type, string Name) : this(Type, Name)
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

        public override string ToString()
        {
            var result = base.ToString() + $"(";
            Variables.ForEach(v => result += $"{v.TypeObject.TypeInLanguage} {v.NameObject},");
            if (result.EndsWith(",")) result = result.Substring(0, result.Length - 1);
            return $"{result})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Method m)
            {
                if (NameObject == m.NameObject && Variables.Count == m.Variables.Count)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
