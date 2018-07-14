using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOPatterns.Core.VisualObjects
{
    public class VisualObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        private int z = 0;
        public int Z
        {
            get => z;
            set
            {
                z = value;
                UpdateZ();
            }
        }

        public UserType Object { get; }
        public string Name { get; }
        public Canvas Canvas { get; }

        private Path Path;
        private Image Image;
        private List<TextBlock> TextBlocks = new List<TextBlock>();

        public VisualObject(UserType obj, Canvas canvas)
        {
            Object = obj;
            Name = obj.Name;
            Canvas = canvas;
            InitializeImage();
            CalculateSize();
            Initialize();
            SetName();
        }

        private void InitializeImage()
        {
            try
            {
                Image = new Image();
                Image.Width = VisualProperties.ImageSize;
                Image.Height = VisualProperties.ImageSize;
                Image.Source = new BitmapImage(new Uri(Object.ICO_PATH));
            }
            catch (Exception e)
            {
                Core.GetInstance().Logger.Log($"{e.Message}");
            }
        }

        private void SetName()
        {
            Path.Name = Name;
            Image.Name = Name;
            for (int i = 0; i < TextBlocks.Count; i++)
            {
                TextBlocks[i].Name = Name;
            }
        }

        public void Move(Point startPoint, Point endPoint)
        {
            double deltaX = endPoint.X - startPoint.X;
            double deltaY = endPoint.Y - startPoint.Y;

            var pathLeft = Canvas.GetLeft(Path);// + deltaX;
            var pathTop = Canvas.GetTop(Path);// + deltaY;

            if (pathLeft + deltaX < 0 || pathLeft + Width + deltaX > Canvas.ActualWidth)
            {
                deltaX = 0;
            }
            if (pathTop + deltaY < 0 || pathTop + Height + deltaY > Canvas.ActualHeight)
            {
                deltaY = 0;
            }
            Canvas.SetLeft(Path, Canvas.GetLeft(Path) + deltaX);
            Canvas.SetTop(Path, Canvas.GetTop(Path) + deltaY);

            Canvas.SetLeft(Image, Canvas.GetLeft(Image) + deltaX);
            Canvas.SetTop(Image, Canvas.GetTop(Image) + deltaY);

            TextBlocks.ForEach(tb =>
            {
                Canvas.SetLeft(tb, Canvas.GetLeft(tb) + deltaX);
                Canvas.SetTop(tb, Canvas.GetTop(tb) + deltaY);
            });

            X = Canvas.GetLeft(Path);
            Y = Canvas.GetTop(Path);
        }

        private void UpdateZ()
        {
            if (Canvas == null) return;

            Canvas.SetZIndex(Path, Z);
            Canvas.SetZIndex(Image, Z + 1);
            TextBlocks.ForEach(tb => Canvas.SetZIndex(tb, Z + 1));
        }

        public void Initialize()
        {
            if(Canvas != null)
            {
                DestroyOnCanvas();
                TextBlocks.Clear();
            }

            Path = new Path();
            Path.Stroke = VisualProperties.Normal;
            Path.Fill = Object is Class ? VisualProperties.Class : VisualProperties.Interface;
            Canvas.SetLeft(Path, 0);
            Canvas.SetTop(Path, 0);

            PathFigureCollection collection = new PathFigureCollection();
            PathGeometry pathGeometry = new PathGeometry();

            double textHeight = VisualProperties.GetTextSize(Object.Name).Height;
            double startY = Y;
            double height = textHeight + VisualProperties.Delta * 2;
            //draw title
            collection.Add(DrawRect(0, startY, height));
            DrawText(X, Y, Object.Name, VisualProperties.FontSize);
            Canvas.SetLeft(Image, X + Width - VisualProperties.ImageSize - VisualProperties.Delta);
            Canvas.SetTop(Image, Y + VisualProperties.Delta);
            //draw variables
            startY += height;
            if (Object is Class c){
                height = (c.Variables.Count + 1) * textHeight + VisualProperties.Delta * 2;
                collection.Add(DrawRect(X, startY, height));
                DrawText(X, startY, VisualProperties.VARIABLES, VisualProperties.PROPERTIES_TEXT_SIZE, AlignmentX.Right);
                startY += textHeight;
                c.Variables.ForEach(v => 
                {
                    DrawText(X, startY, v.ToString(), VisualProperties.FontSize);
                    startY += textHeight;
                });
                startY += - (c.Variables.Count + 1) * textHeight + height;
            }
            //draw methods
            height = (Object.Methods.Count + 1) * textHeight + VisualProperties.Delta * 2;
            collection.Add(DrawRect(X, startY, height));
            DrawText(X, startY, VisualProperties.METHODS, VisualProperties.PROPERTIES_TEXT_SIZE, AlignmentX.Right);
            startY += textHeight;
            Object.Methods.ForEach(m =>
            {
                DrawText(X, startY, m.ToString(), VisualProperties.FontSize);
                startY += textHeight;
            });

            pathGeometry.Figures = collection;
            Path.Data = pathGeometry;
            UpdateZ();
        }

        public void DestroyOnCanvas()
        {
            for (int i = 0; i < Canvas.Children.Count; i++)
            {
                if ((Canvas.Children[i] as FrameworkElement).Name == Name)
                {
                    Canvas.Children.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(bool centered = false)
        {
            if (centered)
            {
                double newX = Canvas.Width / 2 - Width / 2;
                double newY = Canvas.Height / 2 - Height / 2;
                Move(new Point(X, Y), new Point(newX, newY));
            }

            Canvas.Children.Add(Path);
            Canvas.Children.Add(Image);
            TextBlocks.ForEach(tb => Canvas.Children.Add(tb));
        }

        private PathFigure DrawRect(double x, double y, double height)
        {
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(x, y);

            LineSegment lineTop = new LineSegment();
            lineTop.Point = new Point(x + Width, y);

            LineSegment lineRight = new LineSegment();
            lineRight.Point = new Point(x + Width, y + height);

            LineSegment lineBottom = new LineSegment();
            lineBottom.Point = new Point(x, y + height);

            LineSegment lineLeft = new LineSegment();
            lineLeft.Point = new Point(x, y);

            PathSegmentCollection collection = new PathSegmentCollection();
            collection.Add(lineTop);
            collection.Add(lineRight);
            collection.Add(lineBottom);
            collection.Add(lineLeft);

            pathFigure.Segments = collection;

            return pathFigure;
        }

        private void DrawText(double x, double y, string text, double fontSize, AlignmentX alignment = AlignmentX.Left)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
            textBlock.FontFamily = new FontFamily(VisualProperties.FontFamily);
            switch (alignment)
            {
                case AlignmentX.Left:
                    Canvas.SetLeft(textBlock, x + VisualProperties.Delta);
                    break;
                case AlignmentX.Right:
                    var textWidht = VisualProperties.GetTextSize(text).Width;
                    Canvas.SetLeft(textBlock, X + Width - textWidht - VisualProperties.Delta);
                    break;
            }
            Canvas.SetTop(textBlock, y + VisualProperties.Delta);
            Canvas.SetZIndex(textBlock, Z + 1);
            TextBlocks.Add(textBlock);
        }

        private void CalculateSize()
        {
            var VARIABLES_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.VARIABLES).Width + VisualProperties.Delta * 2;
            var METHODS_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.METHODS).Width + VisualProperties.Delta * 2;
            var OBJECT_NAME_WIDTH = VisualProperties.GetTextSize(Object.Name).Width + VisualProperties.ImageSize + VisualProperties.Delta * 2;

            double maxWidth = VARIABLES_TEXT_WIDTH > METHODS_TEXT_WIDTH && METHODS_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       VARIABLES_TEXT_WIDTH :
                       METHODS_TEXT_WIDTH > VARIABLES_TEXT_WIDTH && VARIABLES_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       METHODS_TEXT_WIDTH : OBJECT_NAME_WIDTH;

            double textCount = 1;
            double textHeight = VisualProperties.GetTextSize(VisualProperties.VARIABLES).Height;
            //Variables
            switch (Object)
            {
                case Class c:
                    foreach (var v in c.Variables)
                    {
                        var width = VisualProperties.GetTextSize(v.ToString()).Width + VisualProperties.Delta * 2;
                        if (maxWidth < width) maxWidth = width;
                    }
                    textCount += c.Variables.Count;
                    break;
                case Structure s:
                    /*foreach (var v in s.Variables)
                    {
                        var width = GetTextSize(v.ToString()).Width + delta * 2;
                        if (maxWidth < width) maxWidth = width;
                    }
                    textCount += s.Variables.Count;*/
                    break;
            }
            //Methods
            foreach (var m in Object.Methods)
            {
                var width = VisualProperties.GetTextSize(m.ToString()).Width + VisualProperties.Delta * 2;
                if (maxWidth < width) maxWidth = width;
            }
            textCount += Object.Methods.Count;

            Width = maxWidth;
            Height = textCount * (textHeight + VisualProperties.Delta * 2);
            Height += Object is Interface ? (VisualProperties.PROPERTIES_TEXT_SIZE + VisualProperties.Delta * 2): 
                                        2 * (VisualProperties.PROPERTIES_TEXT_SIZE + VisualProperties.Delta * 2);
        }

        public void Select(bool value = true)
        {
            if (value) Path.Stroke = VisualProperties.Selected;
            else Path.Stroke = VisualProperties.Normal;
        }
    }
}
