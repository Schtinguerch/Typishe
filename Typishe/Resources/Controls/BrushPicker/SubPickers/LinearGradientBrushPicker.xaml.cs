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
using Typishe.Service;

namespace Typishe.Resources.Controls.BrushPicker.SubPickers
{
    /// <summary>
    /// Логика взаимодействия для LinearGradientBrushPicker.xaml
    /// </summary>
    public partial class LinearGradientBrushPicker : Page
    {
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

        public ColorPicker Picker { get; set; }

        public LinearGradientBrushPicker(ColorPicker picker, LinearGradientBrush? defaultBrush = null)
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
                    UpdateOutput(new LinearGradientBrush()
                    {
                        StartPoint = new Point(0d, 0d),
                        EndPoint = new Point(1d, 1d),
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
            var currentBrush = Picker.SelectedBrush as LinearGradientBrush;

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

        private void UpdateOutput(LinearGradientBrush brush)
        {
            Picker.SelectedBrush = PointsRectangle.Fill = brush;

            var clone = brush.Clone();
            clone.StartPoint = new Point(0d, 0d);
            clone.EndPoint = new Point(1d, 0d);
            clone.SpreadMethod = GradientSpreadMethod.Pad;

            GradientLineRectangle.Fill = clone;

            FirstColorSlider.BorderBrush = new SolidColorBrush(brush.GradientStops[0].Color);
            SecondColorSlider.BorderBrush = new SolidColorBrush(brush.GradientStops[1].Color);

            FirstColorSlider.Value = brush.GradientStops[0].Offset;
            SecondColorSlider.Value = brush.GradientStops[1].Offset;

            _causeChanges = false;
            StartPointXTextbox.Text = brush.StartPoint.X.ToString();
            StartPointYTextBox.Text = brush.StartPoint.Y.ToString();

            EndPointXTextBox.Text = brush.EndPoint.X.ToString();
            EndPointYTextBox.Text = brush.EndPoint.Y.ToString();

            SpreadMethodComboBox.SelectedIndex = _spreadMethodNames.IndexOf(brush.SpreadMethod);
            InterpolationMethodComboBox.SelectedIndex = _interpolationMethodNames.IndexOf(brush.ColorInterpolationMode);
            _causeChanges = true;

            StartPositionEllipse.Fill = new SolidColorBrush(brush.GradientStops[0].Color);
            EndPositionEllipse.Fill = new SolidColorBrush(brush.GradientStops[1].Color);

            var startPosition = brush.StartPoint;
            var endPosition = brush.EndPoint;

            startPosition.X *= 100d;
            startPosition.X -= 6d;

            startPosition.Y *= 100d;
            startPosition.Y -= 6d;

            endPosition.X *= 100d;
            endPosition.X -= 6d;

            endPosition.Y *= 100d;
            endPosition.Y -= 6d;

            Canvas.SetTop(EndPositionEllipse, endPosition.Y);
            Canvas.SetLeft(EndPositionEllipse, endPosition.X);

            Canvas.SetTop(StartPositionEllipse, startPosition.Y);
            Canvas.SetLeft(StartPositionEllipse, startPosition.X);
        }

        private void EndPositionEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var mousePosition = e.GetPosition(PointsCanvas);
            var endPoint = new Point(
                mousePosition.X / PointsCanvas.ActualWidth,
                mousePosition.Y / PointsCanvas.ActualHeight);

            
            brush.EndPoint = endPoint;
            UpdateOutput(brush);
        }

        private void StartPositionEllipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var mousePosition = e.GetPosition(PointsCanvas);
            var startPoint = new Point(
                mousePosition.X / PointsCanvas.ActualWidth,
                mousePosition.Y / PointsCanvas.ActualHeight);

            brush.StartPoint = startPoint;
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

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            brush.SpreadMethod = _spreadMethodNames[SpreadMethodComboBox.SelectedIndex];

            UpdateOutput(brush);
        }

        private void InterpolationMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            brush.ColorInterpolationMode = _interpolationMethodNames[InterpolationMethodComboBox.SelectedIndex];

            UpdateOutput(brush);
        }

        private void FirstColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var brush = Picker.SelectedBrush as LinearGradientBrush;
            if (brush == null)
            {
                return;
            }

            brush.GradientStops[0].Offset = FirstColorSlider.Value;
            UpdateOutput(brush);
        }

        private void SecondColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var brush = Picker.SelectedBrush as LinearGradientBrush;
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

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var point = brush.StartPoint;

            point.X = startPointX;
            brush.StartPoint = point;
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

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var point = brush.StartPoint;

            point.Y = startPointY;
            brush.StartPoint = point;
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

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var point = brush.EndPoint;

            point.X = endPointX;
            brush.EndPoint = point;
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

            var brush = Picker.SelectedBrush as LinearGradientBrush;
            var point = brush.EndPoint;

            point.Y = endPointY;
            brush.EndPoint = point;
            UpdateOutput(brush);
        }
    }
}
