using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.VisualObjects.Relations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOPatterns.Core.VisualObjects
{
    /// <summary>
    /// Visual object on a diagram
    /// </summary>
    public class VisualObject
    {
        private double x = 0;
        /// <summary>
        /// Position of an object along the X axis
        /// </summary>
        public double X
        {
            get => x;
            set
            {
                x = value;
                if (x < 0) x = 0;
                if (x + Width > Canvas.ActualWidth) x = Canvas.ActualWidth - Width;
                MoveTo(X, Y);
            }
        }

        private double y = 0;
        /// <summary>
        /// Position of an object along the Y axis
        /// </summary>
        public double Y
        {
            get => y;
            set
            {
                y = value;
                if (y < 0) y = 0;
                if (y + Height > Canvas.ActualHeight) y = Canvas.ActualHeight - Height;
                MoveTo(X, Y);
            }
        }

        /// <summary>
        /// Width of the object
        /// </summary>
        public double Width { get; set; } = 0;

        /// <summary>
        /// Height of the object
        /// </summary>
        public double Height { get; set; } = 0;

        /// <summary>
        /// Whether the object should be centered
        /// </summary>
        public bool IsCentered { get; set; } = false;

        private int z = 0;
        /// <summary>
        /// Position of an object along the Z axis
        /// </summary>
        public int Z
        {
            get => z;
            set
            {
                z = value;
                UpdateZ();
            }
        }

        /// <summary>
        /// Internal usertype object being represented
        /// </summary>
        public UserType Object { get; }

        /// <summary>
        /// Name of visual object
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Canvas, where object drawing
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// List of relations
        /// </summary>
        public List<Relation> Relations { get; }

        /// <summary>
        /// Path figure for represents visual object
        /// </summary>
        private Path Path;

        /// <summary>
        /// Image for represents type of internal object
        /// </summary>
        private Image Image;

        /// <summary>
        /// List of textblocks, displaying information
        /// </summary>
        private List<Text> TextBlocks = new List<Text>();

        /// <summary>
        /// Limitation count of symbols in the textblock
        /// </summary>
        private int MaxSymbolsCount = 20;

        /// <summary>
        /// Limitation width of an object
        /// </summary>
        private double MaxWidht;

        public VisualObject(UserType obj, Canvas canvas)
        {
            Relations = new List<Relation>();
            Name = obj.Name;

            Object = obj;
            Canvas = canvas;
            InitializeImage();
            Initialize();
        }

        public VisualObject(UserType obj, VisualObject visualObject)
        {
            Relations = visualObject.Relations;
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

        /// <summary>
        /// Redrawing all relations
        /// </summary>
        public void DrawRelations()
        {
            for(int i = 0; i < Relations.Count; i++)
            {
                var relation = Relations[i];
                relation.Draw();
                if (relation.Status == Enums.RelationStatus.Removed) i--;
            }
        }

        /// <summary>
        /// Initialize image, for displaying type
        /// </summary>
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

        /// <summary>
        /// Function to move object to fixed x, y position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveTo(double x, double y)
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

        /// <summary>
        /// Correctly placing visual objects on the canvas
        /// </summary>
        private void UpdateZ()
        {
            if (Canvas == null) return;

            Canvas.SetZIndex(Path, Z);
            Canvas.SetZIndex(Image, Z + 1);
            TextBlocks.ForEach(tb => Canvas.SetZIndex(tb, Z + 1));
        }

        /// <summary>
        /// Initialize object:
        ///  -destroy objects with current name on canvas;
        ///  -calculate new width, height;
        ///  -drawing path figures;
        ///  -drawing text.
        /// </summary>
        public void Initialize()
        {
            if(Canvas != null)
            {
                DestroyOnCanvas();
                TextBlocks.Clear();
            }

            CalculateWidth();
            var core = Core.GetInstance();

            Path = new Path();
            Path.Stroke = core.ThemeHelper.NormalItemBrush;
            Path.Fill = Object is Class ? core.ThemeHelper.ClassGradient : core.ThemeHelper.InterfaceGradient;
            Canvas.SetLeft(Path, 0);
            Canvas.SetTop(Path, 0);

            PathFigureCollection collection = new PathFigureCollection();
            PathGeometry pathGeometry = new PathGeometry();

            string text = "";
            for (int i = 0; i < MaxSymbolsCount + 2; i++) text += "T";
            var size = VisualProperties.GetTextSize(text, VisualProperties.FontSize);
            double textHeight = size.Height;
            MaxWidht = size.Width;
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
            Height = startY + height;
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

        /// <summary>
        /// Remove visual objects on the canvas with current name
        /// </summary>
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

        /// <summary>
        /// Destroy object and relations
        /// </summary>
        public void Destroy(Action<VisualObject> reDraw)
        {
            DestroyOnCanvas();
            for(int i = 0; i < Relations.Count; i++)
            {
                var obj = Relations[i].To;
                Relations[i].Destroy();
                reDraw(obj);
                i--;
            }
        }

        /// <summary>
        /// Draw visual object on the canvas
        /// </summary>
        public void Draw()
        {
            if (IsCentered)
            {
                double newX = Canvas.ActualWidth / 2 - Width / 2;
                double newY = Canvas.ActualHeight / 2 - Height / 2;
                X = newX;
                Y = newY;
            }
            MoveTo(X, Y);
            Canvas.Children.Add(Path);
            Canvas.Children.Add(Image);
            TextBlocks.ForEach(tb => Canvas.Children.Add(tb));
        }

        /// <summary>
        /// Draw rect in the specified position with specified height
        /// </summary>
        /// <param name="x">Position along the X axis</param>
        /// <param name="y">Position along the Y axis</param>
        /// <param name="height">Height of the rect</param>
        /// <returns></returns>
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

        /// <summary>
        /// Draw text in the specified position
        /// </summary>
        /// <param name="x">Position along the X axis</param>
        /// <param name="y">Position along the Y axis</param>
        /// <param name="text">Text</param>
        /// <param name="fontSize">Font size</param>
        /// <param name="alignment">Aligment of text</param>
        private void DrawText(double x, double y, string text, double fontSize, AlignmentX alignment = AlignmentX.Left)
        {
            var textBlock = new Text();
            textBlock.Name = Name;
            if(text.Length > MaxSymbolsCount)
            {
                text = $"{text.Substring(0, MaxSymbolsCount)}..";
            }
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

        /// <summary>
        /// Calculate width of an object
        /// </summary>
        private void CalculateWidth()
        {
            var VARIABLES_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.VARIABLES, VisualProperties.PROPERTIES_TEXT_SIZE).Width + VisualProperties.Delta * 2;
            var METHODS_TEXT_WIDTH = VisualProperties.GetTextSize(VisualProperties.METHODS, VisualProperties.PROPERTIES_TEXT_SIZE).Width + VisualProperties.Delta * 2;
            var OBJECT_NAME_WIDTH = VisualProperties.GetTextSize(Object.Name, VisualProperties.FontSize).Width + VisualProperties.ImageSize + VisualProperties.Delta * 2;

            double maxWidth = VARIABLES_TEXT_WIDTH > METHODS_TEXT_WIDTH && VARIABLES_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       VARIABLES_TEXT_WIDTH :
                       METHODS_TEXT_WIDTH > VARIABLES_TEXT_WIDTH && METHODS_TEXT_WIDTH > OBJECT_NAME_WIDTH ?
                       METHODS_TEXT_WIDTH : OBJECT_NAME_WIDTH;
            
            //Variables
            switch (Object)
            {
                case Class c:
                    foreach (var v in c.Variables)
                    {
                        var width = VisualProperties.GetTextSize(v.ToString(), VisualProperties.FontSize).Width + VisualProperties.Delta * 2;
                        if (maxWidth < width) maxWidth = width;
                    }
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
            if (maxWidth > MaxWidht) maxWidth = MaxWidht;
            Width = maxWidth;
        }

        /// <summary>
        /// Select or deselect object, with change stroke color
        /// </summary>
        /// <param name="value"></param>
        public void Select(bool value = true)
        {
            var core = Core.GetInstance();
            if (value) Path.Stroke = core.ThemeHelper.SelectedItemBrush;
            else Path.Stroke = core.ThemeHelper.NormalItemBrush;
        }

        /// <summary>
        /// TextBlock with X, Y positions
        /// </summary>
        class Text : TextBlock
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
