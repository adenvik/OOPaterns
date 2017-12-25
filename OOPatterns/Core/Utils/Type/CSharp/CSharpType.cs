using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type.CSharp
{
    class CSharpType : Type, IType
    {
        public static int VAR = 16;
        public static int DYNAMIC = 17;

        public new List<string> GetTypes()
        {
            List<string> result = base.GetTypes();
            result.AddRange(new[] {"var", "dynamic"});
            return result;
        }
    }
}
