using OOPatterns.Core.Helpers;
using OOPatterns.Core.InternalObject.ParamObject;
using OOPatterns.Core.InternalObject.UserType;
using OOPatterns.Core.Utils.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static OOPatterns.Core.Helpers.Enums;

namespace OOPatterns.Core.VisualObjects.Relations
{
    /// <summary>
    /// Represents aggregation relation between two objects
    /// </summary>
    public class Aggregation : Relation
    {
        /// <summary>
        /// Variable in To object
        /// </summary>
        private Variable variable;

        public Aggregation(VisualObject from, VisualObject to, Canvas canvas) : base(from, to, canvas)
        {
            if (To.Object is Interface)
            {
                To.Relations.Remove(this);
                From.Relations.Remove(this);
                throw new OOPatternsException(this is Composition ? Properties.Resources.compositionException : Properties.Resources.aggregationException);
            }

            Name = $"{nameof(Relation)}_{nameof(Aggregation)}_{Id}";
            AddVariable();
            Draw();
        }

        /// <summary>
        /// Create variable and added it to the "To" object
        /// </summary>
        protected void AddVariable()
        {
            variable = new Variable
            {
                Access = Access.PRIVATE,
                Type = From.Name,
                Name = From.Name.ToLower()
            };

            (To.Object as Class).Variables.Add(variable);
        }

        /// <summary>
        /// Draw realtion on the canvas
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            if (!(To.Object is Class) || (To.Object as Class).Variables.Find(v => v == variable) == null)
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
            (To.Object as Class).Variables.Remove(variable);
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
                    collection.Add(new LineSegment(new Point(To.X - Delta * 1.5f, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X - Delta, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta / 2f, To.Y + To.Height / 2f + Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X - Delta, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X - Delta * 1.5f, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Top:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta * 1.5f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta / 4f, To.Y - Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 4f, To.Y - Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y - Delta * 1.5f), true));
                    break;
                case AnchorType.Right:
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta * 1.5f, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f - Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width, To.Y + To.Height / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta / 2f, To.Y + To.Height / 2f + Delta / 4f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta, To.Y + To.Height / 2f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width + Delta * 1.5f, To.Y + To.Height / 2f), true));
                    break;
                case AnchorType.Bottom:
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta * 1.5f), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f - Delta / 4f, To.Y + To.Height + Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f + Delta / 4f, To.Y + To.Height + Delta / 2f), true));
                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta), true));

                    collection.Add(new LineSegment(new Point(To.X + To.Width / 2f, To.Y + To.Height + Delta * 1.5f), true));
                    break;
            }

            pathFigure.Segments = collection;
            return pathFigure;
        }
    }
}
