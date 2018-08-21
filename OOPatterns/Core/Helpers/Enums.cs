using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.Helpers
{
    /// <summary>
    /// Different enums
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Languages for codegeneration module
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// C#
            /// </summary>
            CSHARP = 0,

            /// <summary>
            /// Java
            /// </summary>
            JAVA = 1
        }

        /// <summary>
        /// Position of anchoring relation to object
        /// </summary>
        public enum AnchorType
        {
            /// <summary>
            /// Automatic find best position
            /// </summary>
            Auto,

            /// <summary>
            /// Relation fixed on left anchor of an object
            /// </summary>
            Left,

            /// <summary>
            /// Relation fixed on top anchor of an object
            /// </summary>
            Top,

            /// <summary>
            /// Relation fixed on right anchor of an object
            /// </summary>
            Right,

            /// <summary>
            /// Relation fixed on bottom anchor of an object
            /// </summary>
            Bottom
        }

        /// <summary>
        /// Status of the relation
        /// </summary>
        public enum RelationStatus
        {
            /// <summary>
            /// Relation drawed
            /// </summary>
            Active,

            /// <summary>
            /// Relation removed, and not drawn
            /// </summary>
            Removed
        }
    }
}
