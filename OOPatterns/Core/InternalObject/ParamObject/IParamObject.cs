using OOPatterns.Core.Utils.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    abstract class IParamObject
    {
        public int AccessObject { set; get; }
        public TypeObject TypeObject { set; get; }
        public string NameObject { set; get; }

        public abstract override bool Equals(object obj);
    }
}
