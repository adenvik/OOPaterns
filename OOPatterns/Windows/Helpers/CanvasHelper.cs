using OOPatterns.Core.Utils.Exceptions;
using OOPatterns.Core.VisualObjects;
using OOPatterns.Core.VisualObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with canvas
    /// </summary>
    public class CanvasHelper
    {
        private bool isEventsEnabled = true;
        /// <summary>
        /// Enable or disable events on canvas
        /// </summary>
        public bool IsEventsEnabled
        {
            get => isEventsEnabled;
            set
            {
                isEventsEnabled = value;
                UpdateEvents();
            }
        } 

        /// <summary>
        /// Canvas, where drawing objects
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// Event fired when canvas selection changed
        /// </summary>
        public EventHandler OnSelectedChanged;

        /// <summary>
        /// Event fired when item double click
        /// </summary>
        public EventHandler OnDoubleClick;

        /// <summary>
        /// True, if mouse button down on object
        /// </summary>
        private bool isButtonDown = false;

        /// <summary>
        /// Delta for the moving object
        /// </summary>
        private Point delta;

        private VisualObject selectedItem;
        /// <summary>
        /// Current selected item
        /// </summary>
        public VisualObject SelectedItem
        {
            get => selectedItem;
            set
            {
                if ((selectedItem == null && value != null) || (selectedItem != null && value == null))
                {
                    OnSelectedChanged?.Invoke(value != null, EventArgs.Empty);
                }
                selectedItem?.Select(false);
                selectedItem = value;
            }
        }
        /// <summary>
        /// Preview position of selected item
        /// </summary>
        private Point? SelectedItemPosition;

        /// <summary>
        /// List of objects on the canvas
        /// </summary>
        public List<VisualObject> Objects { get; }

        /// <summary>
        /// New object
        /// </summary>
        private bool IsNew = false;

        /// <summary>
        /// Current selected relation
        /// </summary>
        private Relation SelectedRelation;

        /// <summary>
        /// List of relations on the canvas
        /// </summary>
        private List<Relation> relations;

        /// <summary>
        /// Name of relation for the adding
        /// </summary>
        private string RelationName;

        /// <summary>
        /// "From" object for the relation
        /// </summary>
        public VisualObject From { get; protected set; }

        /// <summary>
        /// "To" object for the relation
        /// </summary>
        private VisualObject To;

        /// <summary>
        /// Support point for drawing line, represents relation
        /// </summary>
        private Point StartPoint;

        /// <summary>
        /// Support line, represents relation
        /// </summary>
        private Line line;

        /// <summary>
        /// Logic on canvas when relation adding with help of DiagramToolbar
        /// </summary>
        private bool PreparationRelation = false;

        /// <summary>
        /// Event fired after click on canvas, if PreparationRelation is True
        /// </summary>
        public EventHandler OnRelationClick;

        public CanvasHelper(Canvas canvas)
        {
            Canvas = canvas;
            IsEventsEnabled = true;
            Objects = new List<VisualObject>();
            relations = new List<Relation>();
        }

        /// <summary>
        /// Add new visual object on the canvas
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="isCentered">Centered object on the canvas</param>
        public void Add(VisualObject obj, bool isCentered = false)
        {
            Objects.Add(obj);
            obj.Z = Objects.Count * 2;
            obj.IsCentered = isCentered;
            obj.Draw();
            SelectedItem = obj;
            IsNew = true;
        }

        /// <summary>
        /// Add new relation on the canvas
        /// </summary>
        /// <param name="name">Name of relation</param>
        public void AddRelation(string name)
        {
            RelationName = name;
            line = new Line();
            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = StartPoint.X + 10;
            line.Y2 = StartPoint.Y + 10;
            line.Stroke = Core.Core.GetInstance().ThemeHelper.NormalItemBrush;
            line.Opacity = 0.2d;
            Canvas.Children.Add(line);
        }

        /// <summary>
        /// Triggered logic for the adding relation
        /// </summary>
        /// <param name="name"></param>
        public void PreparateRelation(string name)
        {
            RelationName = name;
            PreparationRelation = true;
        }

        /// <summary>
        /// Create relation by name
        /// </summary>
        private void CreateRelation()
        {
            Relation relation = null;
            try
            {
                if (RelationName == Properties.Resources.aggregation)
                {
                    relation = new Aggregation(From, To, Canvas);
                }
                else if (RelationName == Properties.Resources.composition)
                {
                    relation = new Composition(From, To, Canvas);
                }
                else if (RelationName == Properties.Resources.dependency)
                {
                    relation = new Dependency(From, To, Canvas);
                }
                else if (RelationName == Properties.Resources.realization)
                {
                    relation = new Realization(From, To, Canvas);
                }
                relations.Add(relation);
                ReDraw(To);
                To.DrawRelations();
            }
            catch (OOPatternsException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Redraw object, if obj = null, try to redraw selected item
        /// </summary>
        /// <param name="obj">Oject for the redrawing</param>
        public void ReDraw(VisualObject obj = null)
        {
            if (obj == null && SelectedItem == null) return;
            else if (obj == null && SelectedItem != null) obj = SelectedItem;

            obj?.Initialize();
            obj?.Draw();
        }

        /// <summary>
        /// Remove object from the canvas
        /// </summary>
        /// <param name="obj">Object</param>
        public void Remove(VisualObject obj)
        {
            Objects.Remove(obj);
            obj.Destroy(ReDraw);
            if (obj == SelectedItem) SelectedItem = null;
            if (obj == From) From = null;
        }

        /// <summary>
        /// Restore items on the canvas
        /// </summary>
        public void Restore()
        {
            Objects.ForEach(obj => {
                obj.DestroyOnCanvas();
                obj.Draw();
                obj.DrawRelations();
            });
            if (SelectedItemPosition.HasValue && !IsNew && SelectedItem != null)
            {
                SelectedItem.X = SelectedItemPosition.Value.X;
                SelectedItem.Y = SelectedItemPosition.Value.Y;
                SelectedItem.DrawRelations();
                SelectedItemPosition = null;
            }
            if (IsNew) IsNew = !IsNew;
        }

        /// <summary>
        /// Remove all items from the canvas, and save selected item, if param true
        /// </summary>
        /// <param name="saveSelected"></param>
        public void Clear(bool saveSelected = false)
        {
            Canvas.Children.Clear();
            if (!saveSelected)
            {
                SelectedItem = null;
            }
            else
            {
                if (SelectedItem != null)
                {
                    SelectedItemPosition = new Point(SelectedItem.X, SelectedItem.Y);
                }
            }
        }

        /// <summary>
        /// Drawing selection on the item by name
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <returns></returns>
        public bool Select(string name)
        {
            Deselect();
            SelectedItem = Objects.Find(o => o.Name == name);
            if (SelectedItem == null) return false;
            SelectedItem.Select();
            SortZOrder();
            return true;
        }

        /// <summary>
        /// Remove selection on the current selected item
        /// </summary>
        public void Deselect()
        {
            SelectedItem?.Select(false);
            SelectedItem = null;

            SelectedRelation?.Select(false);
            SelectedRelation = null;
        }

        /// <summary>
        /// Update objects along Z axis
        /// </summary>
        public void SortZOrder()
        {
            Objects.Sort((x, y) => x.Z.CompareTo(y.Z));
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i] != SelectedItem)
                {
                    Objects[i].Z = i * 2;
                }
                else
                {
                    Objects[i].Z = Objects.Count * 2;
                }
            }
        }

        /// <summary>
        /// Add or remove events on the canvas
        /// </summary>
        private void UpdateEvents()
        {
            if (isEventsEnabled)
            {
                Canvas.MouseMove += Canvas_MouseMove;
                Canvas.MouseLeftButtonDown += Canvas_MouseDown;
                Canvas.MouseUp += Canvas_MouseUp;
                Canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
            }
            else
            {
                Canvas.MouseMove -= Canvas_MouseMove;
                Canvas.MouseLeftButtonDown -= Canvas_MouseDown;
                Canvas.MouseUp -= Canvas_MouseUp;
                Canvas.MouseRightButtonDown -= Canvas_MouseRightButtonDown;
                Canvas.ContextMenu = null;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(Canvas);
            if (!(e.OriginalSource is Canvas))
            {
                if (isButtonDown)
                {
                    SelectedItem.X = position.X - delta.X;
                    SelectedItem.Y = position.Y - delta.Y;
                    SelectedItem.DrawRelations();
                }
            }
            if (line != null)
            {
                line.X2 = position.X;
                line.Y2 = position.Y;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Canvas))
            {
                var elem = e.OriginalSource as FrameworkElement;
                if (PreparationRelation)
                {
                    PreparationRelation = false;
                    var voElem = Objects.Find(obj => obj.Name == elem.Name);
                    if (voElem != null)
                    {
                        From = voElem;
                        StartPoint = e.GetPosition(Canvas);
                        AddRelation(RelationName);
                    }
                    else ClearState();
                    OnRelationClick?.Invoke(this, EventArgs.Empty);
                    return;
                }
                if (line != null)
                {
                    To = Objects.Find(obj => obj.Name == elem.Name);
                    if (To == null)
                    {
                        Canvas.Children.Remove(line);
                        line = null;
                    }
                    else
                    {
                        if(To != From) CreateRelation();
                        Canvas.Children.Remove(line);
                        line = null;
                    }
                }
                else
                {
                    //if selected - relation
                    if (!Select(elem.Name))
                    {
                        if (SelectedRelation != null) SelectedRelation.Select(false);
                        SelectedRelation = relations.Find(r => r.Name == elem.Name);
                        SelectedRelation.Select(true);
                        return;
                    }
                    //else - visualObject
                    isButtonDown = true;
                    elem.CaptureMouse();

                    delta = new Point
                    {
                        X = e.GetPosition(Canvas).X - SelectedItem.X,
                        Y = e.GetPosition(Canvas).Y - SelectedItem.Y
                    };

                    if (e.ClickCount == 2)
                    {
                        isButtonDown = false;
                        OnDoubleClick?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                ClearState();
                if (PreparationRelation)
                {
                    PreparationRelation = false;
                    OnRelationClick?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void ClearState()
        {
            Deselect();
            if (line != null) Canvas.Children.Remove(line);
            From = null;
            line = null;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isButtonDown = false;
            if (!(e.OriginalSource is Canvas))
            {
                (e.OriginalSource as FrameworkElement).ReleaseMouseCapture();
            }
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearState();
            if (!(e.OriginalSource is Canvas))
            {
                var elem = Objects.Find(obj => obj.Name == (e.OriginalSource as FrameworkElement).Name);
                if (elem != null)
                {
                    From = elem;
                    StartPoint = e.GetPosition(Canvas);
                    Canvas.ContextMenu = Core.Core.GetInstance().ContextMenuHelper.ChildElementOnElementsViewContextMeny;
                }
                else
                {
                    From = null;
                    Canvas.ContextMenu = Core.Core.GetInstance().ContextMenuHelper.StandartElementsViewContextMeny;
                }
            }
            else
            {
                From = null;
                Canvas.ContextMenu = Core.Core.GetInstance().ContextMenuHelper.StandartElementsViewContextMeny;
            }
        }
    }
}
