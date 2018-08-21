using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Helpers
{
    /// <summary>
    /// Access level for different parameters
    /// </summary>
    public static class Access
    {
        public static string PUBLIC = "public";
        public static string PROTECTED = "protected";
        public static string PRIVATE = "private";
        
        /// <summary>
        /// Returns array of access level
        /// </summary>
        /// <returns></returns>
        public static string[] GetAccess()
        {
            return new[] { PRIVATE, PROTECTED, PUBLIC };
        }
    }
}
