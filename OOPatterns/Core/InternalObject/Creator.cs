using OOPatterns.Core.Utils.Modificators;
using OOPatterns.Core.Utils.Modificators.CSharp;
using OOPatterns.Core.Utils.Type.FactoryMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.InternalObject
{
    class Creator
    {
        public static int CSHARP = 0;
        public static int JAVA = 1;


        public TypeFactory TypeFactory { get; }
        public Access Access { get; }


        public Creator(int language)
        {
            switch (language)
            {
                case 1:
                    //TO DO
                case 2:
                    //TO DO
                default:
                case 0:
                    TypeFactory = new CSharpTypeFactory();
                    Access = new CSharpAccess();
                    break;
            }
        }
    }
}
