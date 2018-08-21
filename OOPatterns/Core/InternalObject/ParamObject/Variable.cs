namespace OOPatterns.Core.InternalObject.ParamObject
{
    /// <summary>
    /// Represents variable in usertype object
    /// </summary>
    public class Variable : IParamObject
    {
        /// <summary>
        /// Access of variable
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// Type of variable
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of variable
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}
