using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.UserType
{
    public class Class : UserType
    {
        public override string ICO_PATH { get; } = "pack://application:,,,/OOPatterns;component/Images/class_ico.png";
        public List<IParamObject> Variables { get; set; } = new List<IParamObject>();

        public Class(UserType userType) : base(userType)
        {
        }

        public Class(string name) : base(name)
        {

        }

        public override Class ToClass()
        {
            return this;
        }

        public override Interface ToInterface()
        {
            return new Interface(this);
        }
    }
}
