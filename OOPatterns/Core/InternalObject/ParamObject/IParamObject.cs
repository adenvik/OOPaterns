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
        public int TypeObject { set; get; }
        public string NameObject { set; get; }
    }
}
