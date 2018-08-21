namespace OOPatterns.Core.InternalObject.UserType
{
    /// <summary>
    /// Represents interface
    /// </summary>
    public class Interface : UserType
    {
        /// <summary>
        /// Path to interface icon
        /// </summary>
        public override string ICO_PATH { get; } = "pack://application:,,,/OOPatterns;component/Images/interface_ico.png";

        public Interface(UserType userType) : base(userType) {}

        public Interface(string name = Core.INTERFACE) : base(name) {}

        public override Class ToClass() => new Class(this);

        public override Interface ToInterface() => this;
    }
}
