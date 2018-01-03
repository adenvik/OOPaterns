using OOPatterns.Core.Utils.Modificators;
using OOPatterns.Core.Utils.Type;
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

        Utils.Type.Type type;

        public Creator(int language)
        {
            switch (language)
            {
                case 1:
                    //TO DO
                    type = new Utils.Type.Type(TypeInitializer.Language.Java);
                    break;
                case 0:
                default:
                    type = new Utils.Type.Type();
                    break;
            }
        }
    }
}
