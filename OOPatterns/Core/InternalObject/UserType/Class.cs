using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.UserType
{
    /// <summary>
    /// Represent class
    /// </summary>
    public class Class : UserType
    {
        /// <summary>
        /// Path to the class icon
        /// </summary>
        public override string ICO_PATH { get; } = "pack://application:,,,/OOPatterns;component/Images/class_ico.png";

        /// <summary>
        /// List of variables
        /// </summary>
        public List<IParamObject> Variables { get; set; } = new List<IParamObject>();

        public Class(UserType userType) : base(userType) {}

        public Class(string name = Core.CLASS) : base(name) {}

        public override Class ToClass() => this;

        public override Interface ToInterface() => new Interface(this);
    }
}
