using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type.CSharp
{
    class CSharpMethodType : CSharpType, IMethodType
    {
        public new List<string> GetTypes()
        {
            List<string> result = base.GetTypes();
            result.AddRange(new[] { "var", "dynamic" });
            return result;
        }
    }
}
