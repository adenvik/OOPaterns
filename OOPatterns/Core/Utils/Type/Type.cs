using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    abstract class Type : IType
    {
        public static int OBJECT = 0;
        public static int BOOL = 1;
        public static int BYTE = 2;
        public static int SBYTE = 3;
        public static int CHAR = 4;
        public static int DECIMAL = 5;
        public static int DOUBLE = 6;
        public static int FLOAT = 7;
        public static int INT = 8;
        public static int UINT = 9;
        public static int LONG = 10;
        public static int ULONG = 11;
        public static int SHORT = 12;
        public static int USHORT = 13;
        public static int STRING = 14;
        public static int VOID = 15;

        public List<string> GetTypes()
        {
            return new List<string>() { "object", "bool", "byte", "sbyte",
                                        "char", "decimal", "double", "float",
                                        "int", "uint", "long", "ulong",
                                        "short", "ushort", "string", "void"};
        }
    }
}
