using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OOPatterns.Core.VisualObjects
{
    class CanvasWorker
    {
        private Canvas canvas;
        private List<IVisualObject> objects;
        IVisualObject selected;

        public CanvasWorker(Canvas canvas)
        {
            this.canvas = canvas;
            objects = new List<IVisualObject>();
        }
        
        public void AddElement(IVisualObject obj)
        {
            objects.Add(obj);
            obj.SetZ(objects.Count * 2);
            obj.ApplyTo(canvas);
        }

        public void AddElementToCenter(IVisualObject obj)
        {
            objects.Add(obj);
            obj.SetZ(objects.Count * 2);
            obj.CenteredObject(true).ApplyTo(canvas);
        }

        public void RemoveElement(IVisualObject obj)
        {
            obj.RemoveFrom(canvas);
            objects.Remove(obj);
        }

        public void Clear()
        {
            canvas.Children.Clear();
        }

        public void Draw(string name = null)
        {
            if (name == null) objects.ForEach(obj => obj.UpdateFigure());
            else objects.Find(obj => (obj as VisualObject).OBJECT_NAME == name)?.UpdateFigure();
        }

        public void SelectElement(string name)
        {
            selected = objects.Find(obj => (obj as VisualObject).OBJECT_NAME == name);
            SortZOrder();
        }

        private void SortZOrder()
        {
            objects.Sort((x, y) => x.GetZ().CompareTo(y.GetZ()));
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != selected)
                {
                    objects[i].SetZ(i * 2);
                }
                else
                {
                    objects[i].SetZ(objects.Count * 2);
                }
            }
        }
        

        public void MoveElement(Point startPoint, Point endPoint)
        {
            if (selected == null) return;
            selected.Move(startPoint, endPoint);
        }
    }
}
