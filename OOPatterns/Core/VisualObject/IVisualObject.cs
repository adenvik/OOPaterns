using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPatterns.Core.VisualObject
{
    interface IVisualObject
    {
        void MoveTo(Layer layer);
        void Draw();
    }
}
