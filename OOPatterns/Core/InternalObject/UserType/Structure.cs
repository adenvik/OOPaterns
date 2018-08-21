using System;

namespace OOPatterns.Core.InternalObject.UserType
{
    /// <summary>
    /// Represents structure
    /// </summary>
    public class Structure : UserType
    {
        /// <summary>
        /// Path to the structure icon
        /// </summary>
        public override string ICO_PATH => "";

        public Structure(UserType userType) : base(userType) {}

        public Structure(string name) : base(name) {}


        public override Class ToClass() => throw new NotImplementedException();

        public override Interface ToInterface() => throw new NotImplementedException();
    }
}
