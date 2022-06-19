using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Typishe.Service;

namespace Typishe.Resources.Controls.BrushPicker.SubPickers
{
    /// <summary>
    /// Логика взаимодействия для RadialGradientBrushPicker.xaml
    /// </summary>
    public partial class RadialGradientBrushPicker : Page
    {
        public ColorPicker Picker { get; set; }

        private bool _causeChanges = true;
        private int _seletedColorIndex = 0;

        private ColorPicker _gradientStopColorPicker;
        private SolidColorBrushPicker _gradientStopPicker;

        private List<GradientSpreadMethod> _spreadMethodNames = new List<GradientSpreadMethod>
        {
            GradientSpreadMethod.Pad,
            GradientSpreadMethod.Repeat,
            GradientSpreadMethod.Reflect,
        };

        private List<ColorInterpolationMode> _interpolationMethodNames = new List<ColorInterpolationMode>
        {
            ColorInterpolationMode.ScRgbLinearInterpolation,
            ColorInterpolationMode.SRgbLinearInterpolation,
        };

        public RadialGradientBrushPicker(ColorPicker picker, RadialGradientBrush? defaultBrush = null)
        {
            this.Loaded += (s, e) =>
            {
                _gradientStopColorPicker = new ColorPicker();
                _gradientStopPicker = new SolidColorBrushPicker(_gradientStopColorPicker);

                SolidColorPickerFrame.Navigate(_gradientStopPicker);
                _gradientStopColorPicker.SelectedColorChanged += GradientStopColorChanged;

                if (defaultBrush != null)
                {
                    UpdateOutput(defaultBrush);
                }
                else
                {
                    UpdateOutput(new RadialGradientBrush()
                    {
                        RadiusX = 0.5d, 
                        RadiusY = 0.5d,
                        GradientOrigin = new Point(0.5d, 0.5d),
                        GradientStops = new GradientStopCollection()
                    {
                        new GradientStop(Colors.Aqua, 0d),
                        new GradientStop(Colors.Purple, 1d),
                    }
                    });
                }
            };

            Picker = picker;
            InitializeComponent();
        }

        private void GradientStopColorChanged(Brush selectedBrush)
        {
            var brush = selectedBrush as SolidColorBrush;
            var currentBrush = Picker.SelectedBrush as RadialGradientBrush;

            if (_seletedColorIndex == 0)
            {
                currentBrush.GradientStops[0].Color = brush.Color;
                UpdateOutput(currentBrush);
            }

            else
            {
                currentBrush.GradientStops[1].Color = brush.Color;
                UpdateOutput(currentBrush);
            }
        }

        private void UpdateOutput(RadialGradientBrush brush)
        {
            Picker.SelectedBrush = PointsRectangle.Fill = brush;

            var cloneRadial = brush.Clone();
            var clone = new LinearGradientBrush()
            {
                StartPoint = new Point(0d, 0d),
                EndPoint = new Point(1d, 1d),

                ColorInterpolationMode = cloneRadial.ColorInterpolationMode,
                GradientStops = cloneRadial.GradientStops,
            };

            clone.SpreadMethod = GradientSpreadMethod.Pad;

            GradientLineRectangle.Fill = clone;

            FirstColorSlider.BorderBrush = new SolidColorBrush(brush.GradientStops[0].Color);
            SecondColorSlider.BorderBrush = new SolidColorBrush(brush.GradientStops[1].Color);

            FirstColorSlider.Value = brush.GradientStops[0].Offset;
            SecondColorSlider.Value = brush.GradientStops[1].Offset;

            _causeChanges = false;
            StartPointXTextbox.Text = brush.GradientOrigin.X.ToString();
            StartPointYTextBox.Text = brush.GradientOrigin.Y.ToString();

            EndPointXTextBox.Text = brush.RadiusX.ToString();
            EndPointYTextBox.Text = brush.RadiusY.ToString();

            SpreadMethodComboBox.SelectedIndex = _spreadMethodNames.IndexOf(brush.SpreadMethod);
            InterpolationMethodComboBox.SelectedIndex = _interpolationMethodNames.IndexOf(brush.ColorInterpolationMode);
            _causeChanges = true;

            StartPositionEllipse.Fill = new SolidColorBrush(brush.GradientStops[0].Color);
            EndPositionEllipse.Fill = new SolidColorBrush(brush.GradientStops[1].Color);

            var startPosition = brush.GradientOrigin;
            var endPosition = new Point(brush.RadiusX + startPosition.X, brush.RadiusY + startPosition.Y);

            startPosition.X *= 100d;
            startPosition.Y *= 100d;

            endPosition.X *= 100d;
            endPosition.Y *= 100d;

            Canvas.SetTop(StartPositionEllipse, startPosition.Y);
            Canvas.SetLeft(StartPositionEllipse, startPosition.X);

            Canvas.SetTop(EndPositionEllipse, endPosition.Y);
            Canvas.SetLeft(EndPositionEllipse, endPosition.X);
        }

        private void EndPositionEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            var mousePosition = e.GetPosition(PointsCanvas);

            mousePosition.X -= EndPositionEllipse.ActualWidth / 2;
            mousePosition.Y -= EndPositionEllipse.ActualHeight / 2;

            var endPoint = new Point(
                mousePosition.X / PointsCanvas.ActualWidth,
                mousePosition.Y / PointsCanvas.ActualHeight);

            var startPoint = new Point(
                Canvas.GetLeft(StartPositionEllipse) / PointsCanvas.ActualWidth,
                Canvas.GetTop(StartPositionEllipse) / PointsCanvas.ActualHeight);

            var radius = new Point(
                Math.Abs(endPoint.X - startPoint.X),
                Math.Abs(endPoint.Y - startPoint.Y));

            brush.GradientOrigin = startPoint;
            brush.RadiusX = radius.X;
            brush.RadiusY = radius.Y;

            UpdateOutput(brush);
        }

        private void StartPositionEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            var mousePosition = e.GetPosition(PointsCanvas);

            mousePosition.X -= StartPositionEllipse.ActualWidth / 2;
            mousePosition.Y -= StartPositionEllipse.ActualHeight / 2;

            var startPoint = new Point(
                mousePosition.X / PointsCanvas.ActualWidth,
                mousePosition.Y / PointsCanvas.ActualHeight);

            var endPoint = new Point(
                Canvas.GetLeft(EndPositionEllipse) / PointsCanvas.ActualWidth,
                Canvas.GetTop(EndPositionEllipse) / PointsCanvas.ActualWidth);

            var radius = new Point(
                Math.Abs(endPoint.X - startPoint.X),
                Math.Abs(endPoint.Y - startPoint.Y));

            brush.GradientOrigin = startPoint;
            brush.RadiusX = radius.X;
            brush.RadiusY = radius.Y;

            UpdateOutput(brush);
        }

        private void Slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _seletedColorIndex = 0;
            _gradientStopPicker.SetSelectedColor(FirstColorSlider.BorderBrush as SolidColorBrush);
        }

        private void Slider_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            _seletedColorIndex = 1;
            _gradientStopPicker.SetSelectedColor(SecondColorSlider.BorderBrush as SolidColorBrush);
        }

        private void SpreadMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            brush.SpreadMethod = _spreadMethodNames[SpreadMethodComboBox.SelectedIndex];

            UpdateOutput(brush);
        }

        private void InterpolationMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            brush.ColorInterpolationMode = _interpolationMethodNames[InterpolationMethodComboBox.SelectedIndex];

            UpdateOutput(brush);
        }

        private void FirstColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var brush = Picker.SelectedBrush as RadialGradientBrush;
            if (brush == null)
            {
                return;
            }

            brush.GradientStops[0].Offset = FirstColorSlider.Value;
            UpdateOutput(brush);
        }

        private void SecondColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var brush = Picker.SelectedBrush as RadialGradientBrush;
            if (brush == null)
            {
                return;
            }

            brush.GradientStops[1].Offset = SecondColorSlider.Value;
            UpdateOutput(brush);
        }

        private void StartPointXTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges || string.IsNullOrWhiteSpace(StartPointXTextbox.Text))
            {
                return;
            }

            if (!StartPointXTextbox.Text.TryParse(out double startPointX))
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            var point = brush.GradientOrigin;

            point.X = startPointX;
            brush.GradientOrigin = point;
            UpdateOutput(brush);
        }

        private void StartPointYTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges || string.IsNullOrWhiteSpace(StartPointYTextBox.Text))
            {
                return;
            }

            if (!StartPointYTextBox.Text.TryParse(out double startPointY))
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            var point = brush.GradientOrigin;

            point.Y = startPointY;
            brush.GradientOrigin = point;
            UpdateOutput(brush);
        }

        private void EndPointXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges || string.IsNullOrWhiteSpace(EndPointXTextBox.Text))
            {
                return;
            }

            if (!EndPointXTextBox.Text.TryParse(out double endPointX))
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            brush.RadiusX = endPointX;

            UpdateOutput(brush);
        }

        private void EndPointYTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges || string.IsNullOrWhiteSpace(EndPointYTextBox.Text))
            {
                return;
            }

            if (!EndPointYTextBox.Text.TryParse(out double endPointY))
            {
                return;
            }

            var brush = Picker.SelectedBrush as RadialGradientBrush;
            brush.RadiusY = endPointY;

            UpdateOutput(brush);
        }
    }
}
