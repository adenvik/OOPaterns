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
        private double x = 0;
        public double X
        {
            get => x;
            set
            {
                x = value;
                MoveTo(X, Y);
            }
        }
        private double y = 0;
        public double Y
        {
            get => y;
            set
            {
                y = value;
                MoveTo(X, Y);
            }
        }
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public bool IsCentered { get; set; } = false;

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
        private List<Text> TextBlocks = new List<Text>();

        public VisualObject(UserType obj, Canvas canvas)
        {
            Object = obj;
            Name = obj.Name;
            Canvas = canvas;
            InitializeImage();
            Initialize();
        }

        public VisualObject(UserType obj, VisualObject visualObject)
        {
            Object = obj;
            Name = obj.Name;

            X = visualObject.X;
            Y = visualObject.Y;
            Z = visualObject.Z;
            IsCentered = visualObject.IsCentered;
            Canvas = visualObject.Canvas;
            InitializeImage();
            Initialize();
        }

        private void InitializeImage()
        {
            try
            {
                Image = new Image();
                Image.Width = VisualProperties.ImageSize;
                Image.Height = VisualProperties.ImageSize;
                Image.Name = Name;
                Image.Source = new BitmapImage(new Uri(Object.ICO_PATH));
            }
            catch (Exception e)
            {
                Core.GetInstance().Logger.Log($"{e.Message}");
            }
        }

        public void Move(Point startPoint, Point endPoint)
        {
            double deltaX = endPoint.X - startPoint.X;
            double deltaY = endPoint.Y - startPoint.Y;

            var pathLeft = Canvas.GetLeft(Path);
            var pathTop = Canvas.GetTop(Path);
            if (pathLeft + deltaX < 0 || pathLeft + Width + deltaX > Canvas.ActualWidth)
            {
                deltaX = 0;
            }
            if (pathTop + deltaY < 0 || pathTop + Height + deltaY > Canvas.ActualHeight)
            {
                deltaY = 0;
            }
            X = deltaX;
            Y = deltaY;
        }

        public void MoveTo(double x, double y)
        {
            Canvas.SetLeft(Path, x);
            Canvas.SetTop(Path, y);

            Canvas.SetLeft(Image, x + Width - VisualProperties.ImageSize - VisualProperties.Delta);
            Canvas.SetTop(Image, y + VisualProperties.Delta);

            TextBlocks.ForEach(tb =>
            {
                Canvas.SetLeft(tb, tb.X + x);
                Canvas.SetTop(tb, tb.Y + y);
            });
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
            CalculateSize();

            Path = new Path();
            Path.Stroke = VisualProperties.Normal;
            Path.Fill = Object is Class ? VisualProperties.Class : VisualProperties.Interface;
            Canvas.SetLeft(Path, 0);
            Canvas.SetTop(Path, 0);

            PathFigureCollection collection = new PathFigureCollection();
            PathGeometry pathGeometry = new PathGeometry();

            double textHeight = VisualProperties.GetTextSize(Object.Name, VisualProperties.FontSize).Height;
            double startY = 0;
            double height = textHeight + VisualProperties.Delta * 2;
            //draw title
            collection.Add(DrawRect(0, startY, height));
            DrawText(0, 0, Object.Name, VisualProperties.FontSize);
            Canvas.SetLeft(Image, Width - VisualProperties.ImageSize - VisualProperties.Delta);
            Canvas.SetTop(Image, VisualProperties.Delta);
            //draw variables
            startY += height;
            if (Object is Class c){
                height = (c.Variables.Count + 1) * textHeight + VisualProperties.Delta * 2;
                collection.Add(DrawRect(0, startY, height));
                DrawText(0, startY, VisualProperties.VARIABLES, VisualProperties.PROPERTIES_TEXT_SIZE, AlignmentX.Right);
                startY += textHeight;
                c.Variables.ForEach(v => 
                {
                    DrawText(0, startY, v.ToString(), VisualProperties.FontSize);
                    startY += textHeight;
                });
                startY += - (c.Variables.Count + 1) * textHeight + height;
            }
            //draw methods
            height = (Object.Methods.Count + 1) * textHeight + VisualProperties.Delta * 2;
            collection.Add(DrawRect(0, startY, height));
            DrawText(0, startY, VisualProperties.METHODS, VisualProperties.PROPERTIES_TEXT_SIZE, AlignmentX.Right);
            startY += textHeight;
            Object.Methods.ForEach(m =>
            {
                DrawText(0, startY, m.ToString(), VisualProperties.FontSize);
                startY += textHeight;
            });

            pathGeometry.Figures = collection;
            Path.Data = pathGeometry;
            Path.Name = Name;
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

        public void Draw()
        {
            if (IsCentered)
            {
                double newX = Canvas.ActualWidth / 2 - Width / 2;
                double newY = Canvas.ActualHeight / 2 - Height / 2;
                X = newX;
                Y = newY;
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
            var textBlock = new Text();
            textBlock.Name = Name;
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
            textBlock.FontFamily = new FontFamily(VisualProperties.FontFamily);
            switch (alignment)
            {
                case AlignmentX.Left:
                    Canvas.SetLeft(textBlock, x + VisualProperties.Delta);
                    textBlock.X = x + VisualProperties.Delta;
                    break;
                case AlignmentX.Right:
                    var textWidht = VisualProperties.GetTextSize(text, fontSize).Width;
                    Canvas.SetLeft(textBlock, Width - textWidht - VisualProperties.Delta);
                    textBlock.X = Width - textWidht - VisualProperties.Delta;
                    break;
            }
            textBlock.Y = y + VisualProperties.Delta;
            Canvas.SetTop(textBlock, y + VisualProperties.Delta);
            Canvas.SetZIndex(textBlock, Z + 1);
            TextBlocks.Add(textBlock);
        }

        private void CalculateSize()
        {
            var VARIABLES_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.VARIABLES, VisualProperties.PROPERTIES_TEXT_SIZE).Width + VisualProperties.Delta * 2;
            var METHODS_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.METHODS, VisualProperties.PROPERTIES_TEXT_SIZE).Width + VisualProperties.Delta * 2;
            var OBJECT_NAME_WIDTH = VisualProperties.GetTextSize(Object.Name, VisualProperties.FontSize).Width + VisualProperties.ImageSize + VisualProperties.Delta * 2;

            double maxWidth = VARIABLES_TEXT_WIDTH > METHODS_TEXT_WIDTH && VARIABLES_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       VARIABLES_TEXT_WIDTH :
                       METHODS_TEXT_WIDTH > VARIABLES_TEXT_WIDTH && METHODS_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       METHODS_TEXT_WIDTH : OBJECT_NAME_WIDTH;
            
            double textCount = 1;
            double textHeight = VisualProperties.GetTextSize(VisualProperties.VARIABLES, VisualProperties.FontSize).Height;
            //Variables
            switch (Object)
            {
                case Class c:
                    foreach (var v in c.Variables)
                    {
                        var width = VisualProperties.GetTextSize(v.ToString(), VisualProperties.FontSize).Width + VisualProperties.Delta * 2;
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
                var width = VisualProperties.GetTextSize(m.ToString(), VisualProperties.FontSize).Width + VisualProperties.Delta * 2;
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

        class Text : TextBlock
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
