using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    class TypeInitializer
    {
        public enum Language
        {
            CSharp = 0,
            Java = 1
        }

        private List<TypeObject> types;
        private Language language;

        public TypeInitializer(List<TypeObject> Types, Language Language = Language.CSharp)
        {
            types = Types;
            this.language = Language;
            Initialize();
        }

        private void Initialize()
        {
            //Общие типы
            InitDefault();
            //Собственные типы
            InitCustom();
        }

        private void InitDefault()
        {
            types.Add(new TypeObject("int"));
            types.Add(new TypeObject("double"));
            types.Add(new TypeObject("float"));
            types.Add(new TypeObject("byte"));
            types.Add(new TypeObject("char"));
            types.Add(new TypeObject("long"));
            types.Add(new TypeObject("void"));
        }

        private void InitCustom()
        {
            switch (language)
            {
                case Language.Java:
                    types.Add(new TypeObject("bool", "boolean"));
                    types.Add(new TypeObject("string", "String"));
                    break;
                case Language.CSharp:
                default:
                    types.Add(new TypeObject("bool"));
                    types.Add(new TypeObject("string"));
                    break;
            }
        }
    }
}
