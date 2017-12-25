using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    class Variable : IParamObject
    {
        public Variable(int Type, string Name)
        {
            TypeObject = Type;
            NameObject = Name;
        }

        public Variable(int Access, int Type, string Name) : this(Type, Name)
        {
            AccessObject = Access;
        }
    }
}
