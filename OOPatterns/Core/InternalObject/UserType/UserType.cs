using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using System;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.UserType
{
    public abstract class UserType : Unique
    { 
        public string Access { get; set; }
        public string Name { get; set; }
        public List<IParamObject> Methods { get; set; } = new List<IParamObject>();
        public List<UserType> Parents { get; set; } = new List<UserType>();
        public EventHandler OnParamChange { get; set; }
        public abstract string ICO_PATH { get; }

        public UserType(string name)
        {
            Name = name;
            GenerateId();
        }

        public UserType(UserType userType)
        {
            Methods.AddRange(userType.Methods);
            Id = userType.Id;
            Name = userType.Name;
        }

        public abstract Class ToClass();
        public abstract Interface ToInterface();
    }
}
