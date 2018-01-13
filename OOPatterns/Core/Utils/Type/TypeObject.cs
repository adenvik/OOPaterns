using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    public class TypeObject
    {
        /// <summary>
        /// Название типа
        /// </summary>
        public string TypeName { get; protected set; }

        /// <summary>
        /// Название, которое специфично для языка
        /// </summary>
        public string TypeInLanguage { get; protected set; }

        public TypeObject(string TypeName)
        {
            this.TypeName = TypeName;
            TypeInLanguage = TypeName;
        }

        public TypeObject(string TypeName, string TypeInLanguage)
        {
            this.TypeName = TypeName;
            this.TypeInLanguage = TypeInLanguage;
        }
    }
}
