namespace OOPatterns.Core.InternalObject.ParamObject
{
    public class Variable : IParamObject
    {
        public string Access { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}
