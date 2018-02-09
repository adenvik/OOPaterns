using OOPatterns.Core.InternalObject.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace OOPatterns.Core.VisualObjects
{
    public interface IVisualObject
    {
        IVisualObject ApplyTo(Canvas canvas);
        void RemoveFrom(Canvas canvas);
        IVisualObject CenteredObject(bool value);
        void Move(Point startPoint, Point endPoint);
        IUserType GetUserType();
        IVisualObject SetZ(int value);
        int GetZ();
        IVisualObject UpdateFigure();
        //IVisualObject SetCanvas(Canvas canvas);
        //Path GetDrawable();
        //IVisualObject CenteredObject(bool value);
        //bool IsCentered();
        //IUserType GetUserType();
        //void MoveTo(Point startPosition, Point endPoint);
    }
}
