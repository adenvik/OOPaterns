using OOPatterns.Core.InternalObject.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace OOPatterns.Core.VisualObject
{
    interface IVisualObject
    {
        Path GetDrawable(Canvas canvas);
        IVisualObject CenteredObject(bool value);
        bool IsCentered();
        IUserType GetUserType();
    }
}
