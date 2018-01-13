using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Modificators
{
    public class Access
    {
        public static string PUBLIC = "public";
        public static string PROTECTED = "protected";
        public static string PRIVATE = "private";

        public List<string> GetAccess()
        {
            return new List<string>() {PUBLIC, PROTECTED, PRIVATE};
        }
    }
}
