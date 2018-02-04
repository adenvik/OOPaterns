using OOPatterns.Core.Utils.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    public abstract class IParamObject
    {
        public string AccessObject { set; get; }
        public TypeObject TypeObject { set; get; }
        public string NameObject { set; get; }

        public override string ToString()
        {
            return $"{AccessObject} {TypeObject.TypeInLanguage} {NameObject}";
        }

        public abstract override bool Equals(object obj);
    }
}
