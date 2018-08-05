using OOPatterns.Core.VisualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOPatterns.Windows.Helpers
{
    public class CanvasHelper
    {
        private bool isEventsEnabled = true;
        public bool IsEventsEnabled
        {
            get => isEventsEnabled;
            set
            {
                isEventsEnabled = value;
                UpdateEvents();
            }
        } 

        public Canvas Canvas { get; }
        public EventHandler OnSelectedChanges;

        private bool isButtonDown = false;

        private VisualObject selectedItem;
        public VisualObject SelectedItem
        {
            get => selectedItem;
            set
            {
                if((selectedItem == null && value != null) || (selectedItem != null && value == null))
                {
                    OnSelectedChanges?.Invoke(value != null, EventArgs.Empty);
                }
                selectedItem = value;
            }
        }
        private List<VisualObject> objects;

        public CanvasHelper(Canvas canvas)
        {
            Canvas = canvas;
            IsEventsEnabled = true;
            objects = new List<VisualObject>();
        }

        public void Add(VisualObject obj, bool isCentered = false)
        {
            objects.Add(obj);
            obj.Z = objects.Count * 2;
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
            objects.Remove(obj);
            obj.DestroyOnCanvas();
        }

        public void Clear()
        {
            Canvas.Children.Clear();
        }

        public void Select(string name)
        {
            Deselect();
            SelectedItem = objects.Find(o => o.Name == name);
            SelectedItem.Select();
            SortZOrder();
        }

        public void Deselect()
        {
            SelectedItem?.Select(false);
        }

        public void SortZOrder()
        {
            objects.Sort((x, y) => x.Z.CompareTo(y.Z));
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != SelectedItem)
                {
                    objects[i].Z = i * 2;
                }
                else
                {
                    objects[i].Z = objects.Count * 2;
                }
            }
        }

        private void UpdateEvents()
        {
            if (isEventsEnabled)
            {
                Canvas.MouseMove += Canvas_MouseMove;
                Canvas.MouseLeftButtonDown += Canvas_MouseDown;
                Canvas.MouseUp += Canvas_MouseUp;
            }
            else
            {
                Canvas.MouseMove -= Canvas_MouseMove;
                Canvas.MouseLeftButtonDown -= Canvas_MouseDown;
                Canvas.MouseUp -= Canvas_MouseUp;
            }
        }
        
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(Canvas);
            if (!(e.OriginalSource is Canvas))
            {
                if (isButtonDown)
                {
                    SelectedItem?.MoveTo(position.X, position.Y);
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Canvas))
            {
                isButtonDown = true;
                var elem = e.OriginalSource as FrameworkElement;
                elem.CaptureMouse();
                Select(elem.Name);
                if (e.ClickCount == 2)
                {
                    //TODO
                    Core.Core.GetInstance().Logger.Log("Double Click");
                }
            }
            else
            {
                //if (selectedItem != null) OnSelectedChanges?.Invoke(false, EventArgs.Empty);
                Deselect();
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isButtonDown = false;
            if (!(e.OriginalSource is Canvas))
            {
                (e.OriginalSource as FrameworkElement).ReleaseMouseCapture();
            }
        }
    }
}
