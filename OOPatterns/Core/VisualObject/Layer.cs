using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.VisualObject
{
    abstract class Layer
    {
        List<IVisualObject> Objects;

        public Layer()
        {
            Objects = new List<IVisualObject>();
        }

        public void AddElement(IVisualObject obj)
        {
            Objects.Add(obj);
            Draw();
        }

        public abstract void Draw();
    }
}
