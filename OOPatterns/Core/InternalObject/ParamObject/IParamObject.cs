namespace OOPatterns.Core.InternalObject.ParamObject
{
    /// <summary>
    /// Represents parametrized object
    /// </summary>
    public interface IParamObject
    {
        string Access { get; set; }
        string Type { get; set; }
        string Name { get; set; }
    }
}
