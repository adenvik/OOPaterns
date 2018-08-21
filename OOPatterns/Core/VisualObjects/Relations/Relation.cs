using OOPatterns.Core.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static OOPatterns.Core.Helpers.Enums;

namespace OOPatterns.Core.VisualObjects.Relations
{
    /// <summary>
    /// Represents relation between to usettype objects
    /// </summary>
    public abstract class Relation : Unique
    {
        /// <summary>
        /// Canvas, where relation drawing
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// From object
        /// </summary>
        public VisualObject From { get; }

        /// <summary>
        /// To object
        /// </summary>
        public VisualObject To { get; }

        /// <summary>
        /// Anchor in from object
        /// </summary>
        public AnchorType FromAnchor { get; set; } = AnchorType.Auto;

        /// <summary>
        /// Anchor in to object
        /// </summary>
        public AnchorType ToAnchor { get; set; } = AnchorType.Auto;

        /// <summary>
        /// Name of relation
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Current status of the relation
        /// </summary>
        public RelationStatus Status { get; protected set; } = RelationStatus.Active;

        /// <summary>
        /// Path figure
        /// </summary>
        protected Path Path { get; set; }

        /// <summary>
        /// Size of relation ends
        /// </summary>
        protected float Delta { get; } = 20;

        public Relation(VisualObject from, VisualObject to, Canvas canvas)
        {
            From = from;
            To = to;
            From.Relations.Add(this);
            To.Relations.Add(this);
            Canvas = canvas;
        }

        /// <summary>
        /// Initialize relation, construct path between two objects
        /// </summary>
        protected virtual void Initialize()
        {
            Path = new Path();
            Path.Name = Name;
            Path.Stroke = Core.GetInstance().ThemeHelper.NormalItemBrush;

            PathFigureCollection collection = new PathFigureCollection();
            collection.Add(DrawFrom());
            foreach (var figure in DrawTo().Segments)
            {
                collection[0].Segments.Add(figure);
            }
            collection[0].Segments.Add(collection[0].Segments[0]);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = collection;
            Path.Data = pathGeometry;
        }

        /// <summary>
        /// Select or deselect object, with change stroke color
        /// </summary>
        /// <param name="value"></param>
        public void Select(bool value = true)
        {
            if (value)
            {
                Path.Stroke = Core.GetInstance().ThemeHelper.SelectedItemBrush;
                Path.StrokeThickness = 2;
            }
            else
            {
                Path.Stroke = Core.GetInstance().ThemeHelper.NormalItemBrush;
                Path.StrokeThickness = 1;
            }
        }

        /// <summary>
        /// Construct pathfigure near from object
        /// </summary>
        /// <returns></returns>
        protected virtual PathFigure DrawFrom()
        {
            PathFigure pathFigure = new PathFigure();
            PathSegmentCollection collection = new PathSegmentCollection();

            AnchorType from = FromAnchor;
            if (FromAnchor == AnchorType.Auto)
            {
                from = GetAnchor(From, To);
            }

            switch (from)
            {
                case AnchorType.Left:
                    pathFigure.StartPoint = new Point(From.X, From.Y + From.Height / 2f);
                    collection.Add(new LineSegment(new Point(pathFigure.StartPoint.X - Delta / 2f, pathFigure.StartPoint.Y), true));
                    break;
                case AnchorType.Top:
                    pathFigure.StartPoint = new Point(From.X + From.Width / 2f, From.Y);
                    collection.Add(new LineSegment(new Point(pathFigure.StartPoint.X, pathFigure.StartPoint.Y - Delta / 2f), true));
                    break;
                case AnchorType.Right:
                    pathFigure.StartPoint = new Point(From.X + From.Width, From.Y + From.Height / 2f);
                    collection.Add(new LineSegment(new Point(pathFigure.StartPoint.X + Delta / 2f, pathFigure.StartPoint.Y), true));
                    break;
                case AnchorType.Bottom:
                    pathFigure.StartPoint = new Point(From.X + From.Width / 2f, From.Y + From.Height);
                    collection.Add(new LineSegment(new Point(pathFigure.StartPoint.X, pathFigure.StartPoint.Y + Delta / 2f), true));
                    break;
            }

            pathFigure.Segments = collection;
            return pathFigure;
        }

        /// <summary>
        /// Construct pathfigure near to object
        /// </summary>
        /// <returns></returns>
        protected virtual PathFigure DrawTo()
        {
            PathFigure pathFigure = new PathFigure();
            PathSegmentCollection collection = new PathSegmentCollection();

            AnchorType to = ToAnchor;
            if (ToAnchor == AnchorType.Auto)
            {
                to = GetAnchor(To, From);
            }

            switch (to)
            {
                case AnchorType.Left:
                    collection.Add(new LineSegment(new Point(To.X -  Delta / 2f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 4f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Top:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta / 2f), true));
                    break;
                case AnchorType.Right:
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 4f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Bottom:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta / 2f), true));
                    break;
            }

            pathFigure.Segments = collection;
            return pathFigure;
        }

        /// <summary>
        /// Returns current anchor for the "first" object
        /// </summary>
        /// <param name="first">From object</param>
        /// <param name="second">To object</param>
        /// <returns></returns>
        protected AnchorType GetAnchor(VisualObject first, VisualObject second)
        {
            if (first.X > second.X + second.Width)
            {
                if (first.Y > second.Y + second.Y)
                {
                    return AnchorType.Top;
                }
                else if (first.Y + first.Height < second.Y)
                {
                    return AnchorType.Bottom;
                }
                else return AnchorType.Left;
            }
            else if (first.X + first.Width < second.X)
            {
                if (first.Y > second.Y + second.Y)
                {
                    return AnchorType.Top;
                }
                else if (first.Y + first.Height < second.Y)
                {
                    return AnchorType.Bottom;
                }
                else return AnchorType.Right;
            }
            else
            {
                if (first.Y > second.Y + second.Height)
                {
                    return AnchorType.Top;
                }
                else return AnchorType.Bottom;
            }
        }

        /// <summary>
        /// Draw realtion on the canvas
        /// </summary>
        public virtual void Draw()
        {
            DestroyOnCanvas();
            Initialize();
            Canvas.Children.Add(Path);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public virtual void Destroy()
        {
            DestroyOnCanvas();
            From.Relations.Remove(this);
            To.Relations.Remove(this);
        }

        /// <summary>
        /// Remove visual objects on the canvas with current name
        /// </summary>
        protected void DestroyOnCanvas()
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
    }
}
