namespace OOPatterns.Core.InternalObject.UserType
{
    public class Interface : UserType
    {
        public override string ICO_PATH { get; } = "pack://application:,,,/OOPatterns;component/Images/interface_ico.png";

        public Interface(UserType userType) : base(userType)
        {

        }

        public Interface(string name = Core.INTERFACE) : base(name)
        {

        }

        public override Class ToClass()
        {
            return new Class(this);
        }

        public override Interface ToInterface()
        {
            return this;
        }
    }
}
