using System.Windows.Controls;

namespace OOPatterns.Core.VisualObjects.Relations
{
    /// <summary>
    /// Represents composition relation between two objects
    /// </summary>
    public class Composition : Aggregation
    {
        public Composition(VisualObject from, VisualObject to, Canvas canvas) : base(from, to, canvas)
        {
            DestroyOnCanvas();
            Name = $"{nameof(Relation)}_{nameof(Composition)}_{Id}";
            Draw();
        }

        /// <summary>
        /// Draw realtion on the canvas
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            if (Status == Helpers.Enums.RelationStatus.Removed) return;

            Path.Fill = Core.GetInstance().ThemeHelper.NormalItemBrush;
        }
    }
}
