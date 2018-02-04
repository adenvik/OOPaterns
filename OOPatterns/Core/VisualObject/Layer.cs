using OOPatterns.Core.InternalObject.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOPatterns.Core.VisualObject
{
    class Layer
    {
        public string Name { get; } = "";
        public List<IVisualObject> Objects { get; }
        public Canvas Canvas { get; private set; }

        public Layer(string name)
        {
            Objects = new List<IVisualObject>();
            Name = name;
        }

        public void SetCanvas(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void AddElement(IVisualObject obj)
        {
            Objects.Add(obj);
        }

        public void AddElementToCenter(IVisualObject obj)
        {
            Objects.Add(obj.CenteredObject(true));
        }

        public void RemoveElement(IVisualObject obj)
        {
            Objects.Remove(obj);
        }

        public IVisualObject FindElement(IUserType userType)
        {
            return Objects.Find(obj => obj.GetUserType().Equals(userType));
        }

        public void Draw()
        {
            Canvas.Children.Clear();
            Objects.ForEach(obj => Canvas.Children.Add(obj.GetDrawable(Canvas)));
        }
    }
}
