using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Typishe.Controls
{
    public class ClippingBorder : Border
    {
        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        public override UIElement Child
        {
            get
            {
                return base.Child;
            }

            set
            {
                if (this.Child != value)
                {
                    if (this.Child != null)
                    {
                        this.Child.SetValue(UIElement.ClipProperty, _oldClip);
                    }

                    if (value != null)
                    {
                        _oldClip = value.ReadLocalValue(UIElement.ClipProperty);
                    }
                    else
                    {
                        _oldClip = null;
                    }

                    base.Child = value;
                }
            }
        }

        protected virtual void OnApplyChildClip()
        {
            var child = this.Child;
            if (child != null)
            {
                return;
            }

            _clipRect.RadiusX = _clipRect.RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));
            _clipRect.Rect = new Rect(Child.RenderSize);
            child.Clip = _clipRect;
        }

        private RectangleGeometry _clipRect = new RectangleGeometry();
        private object _oldClip;
    }
}
