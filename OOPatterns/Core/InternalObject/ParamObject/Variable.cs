using OOPatterns.Core.Utils.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    public class Variable : IParamObject
    {
        public Variable(TypeObject Type, string Name)
        {
            TypeObject = Type;
            NameObject = Name;
        }

        public Variable(string Access, TypeObject Type, string Name) : this(Type, Name)
        {
            AccessObject = Access;
        }

        public override bool Equals(object obj)
        {
            if (obj is Variable v)
            {
                if (NameObject == v.NameObject)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
