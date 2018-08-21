using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static OOPatterns.Core.Helpers.Enums;

namespace OOPatterns.Core.VisualObjects.Relations
{
    /// <summary>
    /// Represent dependency relation between two objects
    /// </summary>
    public class Dependency : Relation
    {
        public Dependency(VisualObject from, VisualObject to, Canvas canvas) : base(from, to, canvas)
        {
            Name = $"{nameof(Relation)}_{nameof(Dependency)}_{Id}";
            Draw();
        }

        /// <summary>
        /// Draw realtion on the canvas
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            //Path.StrokeDashArray = new DoubleCollection(new double[] { 3, 2 });
        }

        /// <summary>
        /// Construct pathfigure near to object
        /// </summary>
        /// <returns></returns>
        protected override PathFigure DrawTo()
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
                    collection.Add(new LineSegment(new Point(To.X - Delta, To.Y + To.Height / 2f), true));
                    
                    collection.Add(new LineSegment(new Point(To.X, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 4f, To.Y + To.Height / 2f - Delta / 6f), true));
                    collection.Add(new LineSegment(new Point(To.X, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 4f, To.Y + To.Height / 2f + Delta / 6f), true));
                    collection.Add(new LineSegment(new Point(To.X, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X - Delta, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Top:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));
                    
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta /6f, To.Y - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 6f, To.Y - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));
                    break;
                case AnchorType.Right:
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));
                    
                    collection.Add(new LineSegment(new Point(To.X + To.Width, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 4f, To.Y + To.Height / 2f - Delta / 6f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 4f, To.Y + To.Height / 2f + Delta / 6f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Bottom:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));
                    
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta / 6f, To.Y + To.Height + Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 6f, To.Y + To.Height + Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));
                    break;
            }

            pathFigure.Segments = collection;
            return pathFigure;
        }
    }
}
