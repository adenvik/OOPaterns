using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    abstract class VariableType : Type, IVariableType
    {
        public new List<string> GetTypes()
        {
            List<string> result = base.GetTypes();
            result.Remove("void");
            return result;
        }
    }
}
