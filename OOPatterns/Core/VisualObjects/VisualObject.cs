using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    class VisualObject : IVisualObject
    {
        private static int CountId = 0;
        private int id;

        private IUserType userType;
        public string OBJECT_NAME= "";

        public double X { get; private set; } = 0;
        public double Y { get; private set; } = 0;
        public double Width { get; private set; } = 0;
        public double Height { get; private set; } = 0;
        
        private double FontSize = 12;
        private string FontFamily = "Arial";

        private string VARIABLES = Properties.Resources.variables.ToLower();
        private string METHODS = Properties.Resources.methods.ToLower();
        private double PROPERTIES_TEXT_SIZE = 10;

        private Path path;
        private Image image;
        private List<TextBlock> textBlocks;
        private Canvas canvas;

        private bool isCentered = false;
        private int currentZ = 0;
        private double scaleFactor = 1;
        private double imageSize = 12;
        private double delta = 5;
        private double textSize = 0;

        public VisualObject(IUserType userType)
        {
            this.userType = userType;
            id = CountId;
            CountId++;
            OBJECT_NAME = "VisualObject" + id;
            
            textSize = GetTextSize(FontFamily).Height;
            InitializeImage();
            InitializeFigure();
        }

        private void InitializeImage()
        {
            image = new Image();
            image.Width = imageSize;
            image.Height = imageSize;
            image.Source = new BitmapImage(new Uri(userType.GetImagePath()));
        }

        private IVisualObject InitializeFigure()
        {
            CalculateSize();
            path = new Path();
            textBlocks = new List<TextBlock>();
            path.Stroke = Brushes.Black;

            Canvas.SetLeft(path, X);
            Canvas.SetTop(path, Y);

            PathFigureCollection collection = new PathFigureCollection();
            double currentY = Y + textSize + delta * 2;
            collection.Add(DrawRect(X, Y, X + Width, currentY));
            DrawText(userType.GetName(), X, Y);

            Canvas.SetLeft(image, X + Width - imageSize - delta);
            Canvas.SetTop(image, Y + delta);

            switch (userType)
            {
                case Class c:
                    path.Fill = Brushes.SkyBlue;

                    double endY = currentY + userType.GetVariables().Count * textSize + delta * 2 + PROPERTIES_TEXT_SIZE;
                    collection.Add(GetRect(VARIABLES, userType.GetVariables(), X, currentY, endY));
                    currentY = endY;
                    break;
                /*case Structure s:
                    path.Fill = Brushes.PaleGreen;

                    endY = currentY + UserType.GetVariables().Count * textSize + delta * 2;
                    collection.Add(GetVariablesRect(X, currentY, endY, canvas));
                    currentY = endY;
                    break;*/
                case Interface i:
                    path.Fill = Brushes.SandyBrown;
                    break;
            }
            collection.Add(GetRect(METHODS, userType.GetMethods(), X, currentY));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = collection;

            path.Data = pathGeometry;

            SetZ(currentZ);
            SetName();
            return this;
        }

        private void SetName()
        {
            path.Name = OBJECT_NAME;
            image.Name = OBJECT_NAME;
            for (int i = 0; i < textBlocks.Count; i++)
            {
                textBlocks[i].Name = OBJECT_NAME;
            }
        }

        private void DrawRectText(string text, double y)
        {
            var normalFontSize = FontSize;
            FontSize = PROPERTIES_TEXT_SIZE;

            var size = GetTextSize(text);
            DrawText(text, X + Width - size.Width - delta, y - delta);

            FontSize = normalFontSize;
        }

        public PathFigure GetRect(string hint, List<IParamObject> list, double startX, double startY, double? endY = null)
        {
            var maxY = endY.HasValue ? endY.Value : startY + textSize * list.Count + delta * 2 + PROPERTIES_TEXT_SIZE;
            var rect = DrawRect(startX, startY, startX + Width, maxY);
            DrawRectText(hint, startY);
            startY += textSize / 2 + delta;
            for (int i = 0; i < list.Count; i++)
            {
                double x = startX;
                double y = startY + textSize * i;
                DrawText(list[i].ToString(), x, y);
            }
            return rect;
        }

        private PathFigure DrawRect(double startX, double startY, double endX, double endY)
        {
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(startX, startY);

            LineSegment lineTop = new LineSegment();
            lineTop.Point = new Point(endX, startY);

            LineSegment lineRight = new LineSegment();
            lineRight.Point = new Point(endX, endY);

            LineSegment lineBottom = new LineSegment();
            lineBottom.Point = new Point(startX, endY);

            LineSegment lineLeft = new LineSegment();
            lineLeft.Point = new Point(startX, startY);

            PathSegmentCollection collection = new PathSegmentCollection();
            collection.Add(lineTop);
            collection.Add(lineRight);
            collection.Add(lineBottom);
            collection.Add(lineLeft);

            pathFigure.Segments = collection;

            return pathFigure;
        }

        private void DrawText(string text, double x, double y)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = FontSize;
            textBlock.FontFamily = new FontFamily(FontFamily);
            Canvas.SetLeft(textBlock, x + delta);
            Canvas.SetTop(textBlock, y + delta);
            Canvas.SetZIndex(textBlock, 1);
            textBlocks.Add(textBlock);
        }

        private Size GetTextSize(string text)
        {
            FormattedText ft = new FormattedText(text, new CultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(FontFamily), FontStyles.Normal,
                FontWeights.Normal, new FontStretch()),
                FontSize,
                Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }

        private void CalculateSize()
        {
            var VARIABLES_TEXT_WIDTH = GetTextSize(VARIABLES).Width + delta * 2;
            var METHODS_TEXT_WIDTH = GetTextSize(METHODS).Width + delta * 2;
            var OBJECT_NAME_WIDTH = GetTextSize(userType.GetName()).Width + imageSize + delta * 2;

            double maxWidth = VARIABLES_TEXT_WIDTH > METHODS_TEXT_WIDTH && METHODS_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       VARIABLES_TEXT_WIDTH :
                       METHODS_TEXT_WIDTH > VARIABLES_TEXT_WIDTH && VARIABLES_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       METHODS_TEXT_WIDTH : OBJECT_NAME_WIDTH;

            double textCount = 1;
            if (!(userType is Interface))
            {
                var variables = userType.GetVariables();
                foreach (var v in variables)
                {
                    var width = GetTextSize(v.ToString()).Width + delta * 2;
                    if (maxWidth < width) maxWidth = width;
                }
                textCount += variables.Count;
            }
            var methods = userType.GetMethods();
            foreach (var m in methods)
            {
                var width = GetTextSize(m.ToString()).Width + delta * 2;
                if (maxWidth < width) maxWidth = width;
            }
            textCount += methods.Count;
            Width = maxWidth;
            Height = textCount * (textSize + delta * 2);
            Height += userType is Interface ? (PROPERTIES_TEXT_SIZE + delta * 2) : 2 * (PROPERTIES_TEXT_SIZE + delta * 2);
        }

        private double Scale(double value)
        {
            return value * scaleFactor;
        }

        public IVisualObject ApplyTo(Canvas canvas)
        {
            this.canvas = canvas;
            if (isCentered)
            {
                double newX = canvas.ActualWidth / 2 - Width / 2;
                double newY = canvas.ActualHeight / 2 - Height / 2;
                Move(new Point(X, Y), new Point(newX, newY));
            }
            canvas.Children.Add(path);
            canvas.Children.Add(image);
            textBlocks.ForEach(tb => canvas.Children.Add(tb));
            return this;
        }

        public void RemoveFrom(Canvas canvas)
        {
            int elementsCount = textBlocks.Count + 2; // <--textBlocks.Count + (image + path)
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                if ((canvas.Children[i] as FrameworkElement).Name == OBJECT_NAME)
                {
                    canvas.Children.RemoveAt(i);
                    i--;
                    elementsCount--;
                }
                if (elementsCount == 0) break;
            }
        }

        public IVisualObject CenteredObject(bool value)
        {
            isCentered = value;
            return this;
        }

        public IUserType GetUserType()
        {
            return userType;
        }

        public void Move(Point startPoint, Point endPoint)
        {
            double deltaX = endPoint.X - startPoint.X;
            double deltaY = endPoint.Y - startPoint.Y;

            var pathLeft = Canvas.GetLeft(path) + deltaX;
            var pathTop = Canvas.GetTop(path) + deltaY;

            if (pathLeft + deltaX < 0 || pathLeft + Width + deltaX > canvas.ActualWidth)
            {
                deltaX = 0;
            }
            if(pathTop + deltaY < 0 || pathTop + Height + deltaY > canvas.ActualHeight)
            {
                deltaY = 0;
            }

            Canvas.SetLeft(path, Canvas.GetLeft(path) + deltaX);
            Canvas.SetTop(path, Canvas.GetTop(path) + deltaY);

            Canvas.SetLeft(image, Canvas.GetLeft(image) + deltaX);
            Canvas.SetTop(image, Canvas.GetTop(image) + deltaY);

            textBlocks.ForEach(tb => 
            {
                Canvas.SetLeft(tb, Canvas.GetLeft(tb) + deltaX);
                Canvas.SetTop(tb, Canvas.GetTop(tb) + deltaY);
            });
        }

        public IVisualObject SetZ(int value)
        {
            currentZ = value;
            Canvas.SetZIndex(path, currentZ);
            Canvas.SetZIndex(image, currentZ + 1);
            textBlocks.ForEach(tb => Canvas.SetZIndex(tb, currentZ + 1));
            return this;
        }

        public int GetZ()
        {
            return currentZ;
        }

        public IVisualObject UpdateFigure()
        {
            RemoveFrom(canvas);
            InitializeFigure().ApplyTo(canvas);
            return this;
        }
    }
}
