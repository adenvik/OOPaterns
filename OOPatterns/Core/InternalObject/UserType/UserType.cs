using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using System.Collections.Generic;

namespace OOPatterns.Core.InternalObject.UserType
{
    /// <summary>
    /// Represent usertype object
    /// </summary>
    public abstract class UserType : Unique
    { 
        /// <summary>
        /// Access of usertype object
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// Name of usertype object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of methods
        /// </summary>
        public List<IParamObject> Methods { get; set; } = new List<IParamObject>();

        /// <summary>
        /// List of parents
        /// </summary>
        public List<UserType> Parents { get; set; } = new List<UserType>();

        /// <summary>
        /// Path to the icon, represents specific usertype
        /// </summary>
        public abstract string ICO_PATH { get; }

        public UserType(string name)
        {
            Name = name;
            if(name == Core.CLASS || name == Core.INTERFACE)
            {
                Name = $"{name}{Id}";
            }
        }

        public UserType(UserType userType)
        {
            Methods.AddRange(userType.Methods);
            Id = userType.Id;
            Name = userType.Name;
        }

        /// <summary>
        /// Change object to the class
        /// </summary>
        /// <returns></returns>
        public abstract Class ToClass();

        /// <summary>
        /// Change object to the interface
        /// </summary>
        /// <returns></returns>
        public abstract Interface ToInterface();
    }
}
