using OOPatterns.Core.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    class Type : IType
    {
        List<TypeObject> types;

        public TypeObject this[string Name]
        {
            get
            {
                TypeObject result = types.Find(t => t.TypeName == Name);
                if (result == null) throw new OOPatternsException();
                return result;
            }
        }

        public Type(TypeInitializer.Language language = TypeInitializer.Language.CSharp)
        {
            new TypeInitializer(types, language);
        }

        public bool Exist(string Name)
        {
            return types.Find(t => t.TypeName == Name) != null ? true : false;
        }

        public List<TypeObject> GetTypes()
        {
            return types;
        }
    }
}
