using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Utils.Type
{
    interface IType
    {
        /// <summary>
        /// Вернет список типов
        /// </summary>
        /// <returns></returns>
        List<TypeObject> GetTypes();

        /// <summary>
        /// Проверка типа в конкретном экземпляре
        /// </summary>
        /// <returns></returns>
        bool Exist(string Name);

        /// <summary>
        /// Вернет объект-тип, если он есть
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        TypeObject this[string Name]
        {
            get;
        }
    }
}
