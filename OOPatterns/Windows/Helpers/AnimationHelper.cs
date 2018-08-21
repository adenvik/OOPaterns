using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OOPatterns.Windows.Helpers
{
    /// <summary>
    /// Helper for working with animation in the Main window
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        /// Class for storage inforamtion about animation
        /// </summary>
        public class AnimationDetails
        {
            /// <summary>
            /// Enum of thickness property for the animation
            /// </summary>
            public enum ThicknessAnimationProperty
            {
                Left, Top, Right, Bottom
            }
            public ThicknessAnimationProperty? ThicknessProperty { get; set; } = null;

            /// <summary>
            /// Window animation property
            /// </summary>
            public DependencyProperty AnimateProperty { get; set; }

            /// <summary>
            /// Key for the fired animation
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Start position of the element
            /// </summary>
            public double StartValue { get; set; }

            /// <summary>
            /// End position of the element
            /// </summary>
            public double EndValue { get; set; }

            /// <summary>
            /// Time of animation
            /// </summary>
            public TimeSpan Time { get; set; } = TimeSpan.FromMilliseconds(200);

            /// <summary>
            /// Animated element
            /// </summary>
            public dynamic Element { get; set; }

            /// <summary>
            /// Current state of animation (forward or back)
            /// </summary>
            private bool State = true;

            /// <summary>
            /// Function for starting animation
            /// </summary>
            public void Animate()
            {
                AnimationTimeline animation = null;
                if (ThicknessProperty != null)
                {
                    var thickness = new Thickness();
                    thickness.Left = ThicknessProperty == ThicknessAnimationProperty.Left ? State ? EndValue : StartValue : Element.Margin.Left;
                    thickness.Top = ThicknessProperty == ThicknessAnimationProperty.Top ? State ? EndValue : StartValue : Element.Margin.Top;
                    thickness.Right = ThicknessProperty == ThicknessAnimationProperty.Right ? State ? EndValue : StartValue : Element.Margin.Right;
                    thickness.Bottom = ThicknessProperty == ThicknessAnimationProperty.Bottom ? State ? EndValue : StartValue : Element.Margin.Bottom;

                    animation = new ThicknessAnimation(thickness, Time);
                }
                else
                {
                    animation = new DoubleAnimation
                    {
                        From = State ? StartValue : EndValue,
                        To = State ? EndValue : StartValue,
                        Duration = Time
                    };
                }

                try
                {
                    Element.BeginAnimation(AnimateProperty, animation);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine($"{Key} Error");
                }

                State = !State;
            }
        }

        /// <summary>
        /// List of animations
        /// </summary>
        private List<AnimationDetails> animations;

        /// <summary>
        /// Main window with canimated controls
        /// </summary>
        private MainWindow Window;

        public AnimationHelper(MainWindow window)
        {
            animations = new List<AnimationDetails>();
            Window = window;
            PreparationAnimation();
        }

        public void Add(AnimationDetails animationDetails)
        {
            animations.Add(animationDetails);
        }

        /// <summary>
        /// Initialize animations on the window
        /// </summary>
        private void PreparationAnimation()
        {
            Add(new AnimationDetails
            {
                ThicknessProperty = AnimationDetails.ThicknessAnimationProperty.Right,
                Key = Window.RightToolbar.Name,
                StartValue = 0,
                EndValue = 200,
                AnimateProperty = FrameworkElement.MarginProperty,
                Element = Window.ElementsView
            });
            Add(new AnimationDetails
            {
                ThicknessProperty = AnimationDetails.ThicknessAnimationProperty.Right,
                Key = Window.RightToolbar.Name,
                StartValue = -201,
                EndValue = 0,
                AnimateProperty = FrameworkElement.MarginProperty,
                Element = Window.RightToolbar
            });

            Add(new AnimationDetails
            {
                Key = Window.Parents.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = FrameworkElement.HeightProperty,
                Element = Window.Parents
            });
            Window.Parents_Arrow.RenderTransform = new RotateTransform(0);
            Add(new AnimationDetails
            {
                Key = Window.Parents.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Window.Parents_Arrow.RenderTransform
            });
            Add(new AnimationDetails
            {
                Key = Window.Variables.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = FrameworkElement.HeightProperty,
                Element = Window.Variables
            });
            Window.Variables_Arrow.RenderTransform = new RotateTransform(0);
            Add(new AnimationDetails
            {
                Key = Window.Variables.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Window.Variables_Arrow.RenderTransform
            });
            Add(new AnimationDetails
            {
                Key = Window.Methods.Name,
                StartValue = 25,
                EndValue = 200,
                AnimateProperty = FrameworkElement.HeightProperty,
                Element = Window.Methods
            });
            Window.Methods_Arrow.RenderTransform = new RotateTransform(0);
            Add(new AnimationDetails
            {
                Key = Window.Methods.Name,
                StartValue = 0,
                EndValue = 180,
                AnimateProperty = RotateTransform.AngleProperty,
                Element = Window.Methods_Arrow.RenderTransform
            });

            (Window.Parents.Children[0] as Panel).MouseLeftButtonDown += AnimationHelper_MouseLeftButtonDown;
            (Window.Variables.Children[0] as Panel).MouseLeftButtonDown += AnimationHelper_MouseLeftButtonDown;
            (Window.Methods.Children[0] as Panel).MouseLeftButtonDown += AnimationHelper_MouseLeftButtonDown;
        }

        /// <summary>
        /// Event fired animation when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationHelper_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Animate(((sender as FrameworkElement).Parent as FrameworkElement).Name);
        }

        /// <summary>
        /// Fired animations by key
        /// </summary>
        /// <param name="animKeys"></param>
        public void Animate(params string[] animKeys)
        {
            foreach(string key in animKeys)
            {
                animations.FindAll(a => a.Key == key)
                          .ForEach(a => a.Animate());
            }
        }
    }
}
