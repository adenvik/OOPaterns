using System;

namespace OOPatterns.Core.InternalObject.UserType
{
    public class Structure : UserType
    {
        public override string ICO_PATH => "";

        public Structure(UserType userType) : base(userType)
        {

        }

        public Structure(string name) : base(name)
        {

        }


        public override Class ToClass()
        {
            throw new NotImplementedException();
        }

        public override Interface ToInterface()
        {
            throw new NotImplementedException();
        }
    }
}
