using OOPatterns.Core.Helpers;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.ParamObject
{
    public class Method : IParamObject
    {
        public string Access { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<IParamObject> Parameters { get; set; } = new List<IParamObject>();
        public string Content { get; set; }

        public override string ToString()
        {
            string result =  $"{Type} {Name}(";

            Parameters.ForEach(p => result += $"{p.Type} {p.Name}, ");
            if (result.EndsWith(",")) result = result.Substring(0, result.Length - 1);

            return $"{result})";
        }
    }
}
