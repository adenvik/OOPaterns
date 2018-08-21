using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core
{
    /// <summary>
    /// Element of something
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Usertype object
        /// </summary>
        public UserType UserObject
        {
            get
            {
                return VisualObject?.Object;
            }
            set
            {
                VisualObject.DestroyOnCanvas();
                VisualObject = new VisualObject(value, VisualObject);
            }
        }

        /// <summary>
        /// Visual representation of usertype object
        /// </summary>
        public VisualObject VisualObject { get; set; }
    }
}
