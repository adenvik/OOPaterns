using OOPatterns.Core.Helpers;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    /// <summary>
    /// Represents method in usertype object
    /// </summary>
    public class Method : IParamObject
    {
        /// <summary>
        /// Access level of method
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// Type of method
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of method
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameters of method
        /// </summary>
        public List<IParamObject> Parameters { get; set; } = new List<IParamObject>();

        /// <summary>
        /// Body of method
        /// </summary>
        public string Content { get; set; }

        public override string ToString()
        {
            string result =  $"{Type} {Name}(";

            Parameters.ForEach(p => result += $"{p.Type} {p.Name}, ");
            if (result.EndsWith(", ")) result = result.Substring(0, result.Length - 2);

            return $"{result})";
        }
    }
}
