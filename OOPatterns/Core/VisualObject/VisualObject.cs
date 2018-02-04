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

namespace OOPatterns.Core.VisualObject
{
    class VisualObject : IVisualObject
    {
        public double X { set; get; } = 0;
        public double Y { set; get; } = 0;

        private double width = 0;
        private double height = 0;
        private double scaleFactor = 1;

        private double delta = 5;
        private double textSize = 0;

        private double FontSize = 12;
        private string FontFamily = "Arial";

        private IUserType UserType;
        private bool isCentered = false;

        private List<TextBlock> textBlocks;
        private Image image;
        private double imageSize = 10;

        private string VARIABLES = Properties.Resources.variables.ToLower();
        private string METHODS = Properties.Resources.methods.ToLower();
        private double PROPERTIES_TEXT_SIZE = 10;

        public VisualObject(IUserType userType)
        {
            UserType = userType;
            textSize = GetTextSize("Sample text").Height;
            textBlocks = new List<TextBlock>();
        }

        public VisualObject(IUserType userType, string imagePath) : this(userType)
        {
            image = new Image();
            image.Width = imageSize;
            image.Height = imageSize;
            image.Source = new BitmapImage(new Uri(imagePath));
        }

        public Path GetDrawable(Canvas canvas)
        {
            CalculateSize();
            if (isCentered)
            {
                Y = canvas.ActualHeight / 2 - height / 2;
                X = canvas.ActualWidth / 2 - width / 2;
            }

            Canvas.SetLeft(image, X + width - imageSize - delta);
            Canvas.SetTop(image, Y + delta);
            Canvas.SetZIndex(image, 2);
            canvas.Children.Add(image);

            Path path = new Path();
            path.Stroke = Brushes.Black;

            PathFigureCollection collection = new PathFigureCollection();
            double currentY = Y + textSize + delta * 2;
            collection.Add(DrawRect(X, Y, X + width, currentY));
            DrawText(UserType.GetName(), X, Y, canvas);

            switch (UserType)
            {
                case Class c:
                    path.Fill = Brushes.SkyBlue;

                    double endY = currentY + UserType.GetVariables().Count * textSize + delta * 2 + PROPERTIES_TEXT_SIZE;
                    collection.Add(GetVariablesRect(X, currentY, endY, canvas));
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
            collection.Add(GetMethodsRect(X, currentY, canvas));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = collection;
            
            path.Data = pathGeometry;

            return path;
        }

        public PathFigure GetVariablesRect(double startX, double startY, double endY, Canvas canvas)
        {
            var variables = UserType.GetVariables();
            var rect = DrawRect(startX, startY, startX + width, endY);
            DrawRectText(VARIABLES, startY, canvas);
            startY += textSize / 2 + delta;
            for (int i = 0; i < variables.Count; i++)
            {
                double x = startX;
                double y = startY + textSize * i;
                DrawText(variables[i].ToString(), x, y, canvas);
            }
            return rect;
        }

        private void DrawRectText(string text, double y, Canvas canvas)
        {
            var normalFontSize = FontSize;
            FontSize = PROPERTIES_TEXT_SIZE;

            var size = GetTextSize(text);
            DrawText(text, X + width - size.Width - delta, y - delta, canvas);

            FontSize = normalFontSize;
        }

        public PathFigure GetMethodsRect(double startX, double startY, Canvas canvas)
        {
            var methods = UserType.GetMethods();
            double endY = startY + textSize * methods.Count + delta * 2 + PROPERTIES_TEXT_SIZE;
            var rect = DrawRect(startX, startY, startX + width, endY);
            DrawRectText(METHODS, startY, canvas);
            for (int i = 0; i < methods.Count; i++)
            {
                double x = startX;
                double y = startY + textSize * i;
                DrawText(methods[i].ToString(), x, y, canvas);
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

        private void DrawText(string text, double x, double y, Canvas canvas)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = FontSize;
            textBlock.FontFamily = new FontFamily(FontFamily);
            Canvas.SetLeft(textBlock, x + delta);
            Canvas.SetTop(textBlock, y + delta);
            Canvas.SetZIndex(textBlock, 1);
            textBlocks.Add(textBlock);
            canvas.Children.Add(textBlock);
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
            var OBJECT_NAME_WIDTH = GetTextSize(UserType.GetName()).Width + imageSize + delta * 2;

            double maxWidth = VARIABLES_TEXT_WIDTH > METHODS_TEXT_WIDTH && METHODS_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                              VARIABLES_TEXT_WIDTH :
                              METHODS_TEXT_WIDTH > VARIABLES_TEXT_WIDTH && VARIABLES_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                              METHODS_TEXT_WIDTH : OBJECT_NAME_WIDTH;

            double textCount = 1;
            if (!(UserType is Interface))
            {
                var variables = UserType.GetVariables();
                foreach(var v in variables)
                {
                    var width = GetTextSize(v.ToString()).Width + delta * 2;
                    if (maxWidth < width) maxWidth = width;
                }
                textCount += variables.Count;
            }
            var methods = UserType.GetMethods();
            foreach (var m in methods)
            {
                var width = GetTextSize(m.ToString()).Width + delta * 2;
                if (maxWidth < width) maxWidth = width;
            }
            textCount += methods.Count;
            this.width = maxWidth;
            this.height = textCount * (textSize + delta * 2);
        }

        private double Scale(double value)
        {
            return value * scaleFactor;
        }

        private string GetCutString(string s, double part)
        {
            int size = (int)(s.Length * part);
            return $"{s.Substring(0, size)}..";
        }

        public IVisualObject CenteredObject(bool value)
        {
            isCentered = value;
            return this;
        }

        public bool IsCentered()
        {
            return isCentered;
        }

        public IUserType GetUserType()
        {
            return UserType;
        }
    }
}
