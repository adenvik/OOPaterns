using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOPatterns.Core.VisualObject
{
    class CanvasWorker
    {
        private Layer[] layers;

        public CanvasWorker(bool isDraggable = false)
        {
            InitLayers(isDraggable);
        }

        public CanvasWorker BindMainLayer(Canvas canvas)
        {
            layers[0].SetCanvas(canvas);
            return this;
        }

        public CanvasWorker BindDragLayer(Canvas canvas)
        {
            if (layers.Length < 2) throw new OOPatternsException();

            layers[1].SetCanvas(canvas);
            return this;
        }

        private void InitLayers(bool isDraggable)
        {
            if (isDraggable)
            {
                layers = new Layer[2];
                layers[1] = new Layer("dragLayer");
            }
            else
            {
                layers = new Layer[1];
            }
            layers[0] = new Layer("mainLayer");
        }

        public void ReplaceElement(IUserType oldElement, IUserType newElement, string imagePath, string layerName = "mainLayer")
        {
            Layer currentLayer = layers[0];
            var objForReplace = currentLayer.FindElement(oldElement);
            if (objForReplace == null) return;
            bool isCentered = objForReplace.IsCentered();
            if (isCentered) currentLayer.AddElementToCenter(new VisualObject(newElement, imagePath));
            else
            {
                if (objForReplace is VisualObject v)
                {
                    var elem = new VisualObject(newElement, imagePath);
                    elem.X = v.X;
                    elem.Y = v.Y;
                    currentLayer.AddElement(elem);
                }
            }
            currentLayer.RemoveElement(objForReplace);
            currentLayer.Draw();
        }

        public void AddElement(IUserType userType, string imagePath, string layerName = "mainLayer")
        {
            layers[0].AddElement(new VisualObject(userType, imagePath));
            layers[0].Draw();
        }

        public void AddElementToCenter(IUserType userType, string imagePath, string layerName = "mainLayer")
        {
            layers[0].AddElementToCenter(new VisualObject(userType, imagePath));
            layers[0].Draw();
        }

        public void Draw(string layerName = "mainLayer")
        {
            var layer = layers[0];
            layer.Draw();
        }
    }
}
