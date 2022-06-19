using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Typishe.Setup;

namespace Typishe.Visuality
{
    public class ParallaxEffect
    {
        private Grid _parallaxedGrid;

        private TranslateTransform _transform;

        public double MoveMultiplier { get; set; } = -0.05;

        public Grid ParallaxedGrid
        {
            get => _parallaxedGrid;
            set
            {
                if (_parallaxedGrid != null)
                {
                    _transform.X = _transform.Y = 0d;
                }

                if (value == null)
                {
                    return;
                }

                _parallaxedGrid = value;
                if (ApplicationSetup.VisualSetup.EnableParallaxEffect)
                {
                    CenterNode.AppWindow.PreviewMouseMove += MouseMoveMethod;
                }

                _transform = (_parallaxedGrid.RenderTransform as TransformGroup).Children[1] as TranslateTransform;
                _transform.X = _transform.Y = 0d;
            }
        }

        private void MouseMoveMethod(object sender, MouseEventArgs e)
        {
            var mousePoint = e.GetPosition(_parallaxedGrid);
            ShowEffect(mousePoint);
        }

        public void ShowEffect(Point mousePosition)
        {
            var offsetX = mousePosition.X - CenterNode.AppWindow.ActualWidth / 2d;
            var offsetY = mousePosition.Y - CenterNode.AppWindow.ActualHeight / 2d;

            _transform.X = offsetX * MoveMultiplier;
            _transform.Y = offsetY * MoveMultiplier;
        }

        private void HandleEnabledChanging(bool enabled)
        {
            if (_parallaxedGrid == null)
            {
                return;
            }

            if (enabled)
            {
                CenterNode.AppWindow.PreviewMouseMove += MouseMoveMethod;
            }

            else
            {
                CenterNode.AppWindow.PreviewMouseMove -= MouseMoveMethod;
                _transform.X = _transform.Y = 0d;
            }
        }

        public ParallaxEffect() 
        {
            ApplicationSetup.VisualSetup.ParallaxEnabledChanged += HandleEnabledChanging;
        }

        public ParallaxEffect(Grid parallaxedGrid)
        {
            ParallaxedGrid = parallaxedGrid;
            ApplicationSetup.VisualSetup.ParallaxEnabledChanged += HandleEnabledChanging;
        }
    }
}
