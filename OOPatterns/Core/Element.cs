using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core
{
    public class Element
    {
        public UserType UserObject
        {
            get
            {
                return VisualObject?.Object;
            }
            set
            {
                VisualObject = new VisualObject(value, VisualObject);
            }
        }
        public VisualObject VisualObject { get; set; }
    }
}
