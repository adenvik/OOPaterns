using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace OOPatterns.Windows.Helpers
{
    public class AnimationHelper
    {
        public class AnimationDetails
        {
            public enum ThicknessAnimationProperty
            {
                Left, Top, Right, Bottom
            }
            public ThicknessAnimationProperty? ThicknessProperty { get; set; } = null;
            public DependencyProperty AnimateProperty { get; set; }
            public string Key { get; set; }
            public double StartValue { get; set; }
            public double EndValue { get; set; }
            public TimeSpan Time { get; set; } = TimeSpan.FromMilliseconds(200);
            public dynamic Element { get; set; }
            private bool State = true;

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

        List<AnimationDetails> animations;

        public AnimationHelper()
        {
            animations = new List<AnimationDetails>();
        }

        public void Add(AnimationDetails animationDetails)
        {
            animations.Add(animationDetails);
        }

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
