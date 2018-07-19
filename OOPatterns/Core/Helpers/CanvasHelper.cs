using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OOPatterns.Core.Helpers
{
    public class CanvasHelper
    {
        public Canvas Canvas { get; }
        public VisualObject SelectedItem { get; set; }
        private List<VisualObject> Objects;

        public CanvasHelper(Canvas canvas)
        {
            Canvas = canvas;
            Objects = new List<VisualObject>();
        }

        public void Add(VisualObject obj, bool isCentered = false)
        {
            Objects.Add(obj);
            obj.Z = Objects.Count * 2;
            obj.IsCentered = isCentered;
            obj.Draw();
        }

        public void ReDraw(VisualObject obj)
        {
            obj.Initialize();
            obj.Draw();
        }

        public void Remove(VisualObject obj)
        {
            Objects.Remove(obj);
            obj.DestroyOnCanvas();
        }

        public void Clear()
        {
            Canvas.Children.Clear();
        }

        public void Select(string name)
        {
            Deselect();
            SelectedItem = Objects.Find(o => o.Name == name);
            SelectedItem.Select();
            SortZOrder();
        }

        public void Deselect()
        {
            if (SelectedItem != null)
            {
                SelectedItem.Select(false);
            }
        }

        public void SortZOrder()
        {
            Objects.Sort((x, y) => x.Z.CompareTo(y.Z));
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i] != SelectedItem)
                {
                    Objects[i].Z = i * 2;
                }
                else
                {
                    Objects[i].Z = Objects.Count * 2;
                }
            }
        }
    }
}
