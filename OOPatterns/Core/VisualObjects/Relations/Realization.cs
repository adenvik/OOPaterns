using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static OOPatterns.Core.Helpers.Enums;

namespace OOPatterns.Core.VisualObjects.Relations
{
    /// <summary>
    /// Represent realization relation between two objects
    /// </summary>
    public class Realization : Relation
    {
        public Realization(VisualObject from, VisualObject to, Canvas canvas) : base(from, to, canvas)
        {
            Name = $"{nameof(Relation)}_{nameof(Realization)}_{Id}";
            From.Object.Parents.Add(To.Object);
            Draw();
        }

        /// <summary>
        /// Draw realtion on the canvas
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            Path.StrokeThickness = 1;
            Path.StrokeDashArray = new DoubleCollection(new double[] { 2, 1 });

            if(From.Object.Parents.Find(p => p.Name == To.Name) == null)
            {
                Destroy();
                Status = RelationStatus.Removed;
            }
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            From.Object.Parents.Remove(To.Object);
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
                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f + Delta / 4f), true));

                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Top:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 4f, To.Y - Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta / 4f, To.Y - Delta / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));
                    break;
                case AnchorType.Right:
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f), true));
                    
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f + Delta / 4f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Bottom:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 4f, To.Y + To.Height + Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta / 4f, To.Y + To.Height + Delta / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));
                    break;
            }

            pathFigure.Segments = collection;
            return pathFigure;
        }
    }
}
