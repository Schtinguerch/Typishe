using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;

using Typishe.Service;
using Typishe.Setup;

namespace Typishe.Resources.Controls.BrushPicker.SubPickers
{
    /// <summary>
    /// Логика взаимодействия для SolidColorBrushPicker.xaml
    /// </summary>
    public partial class SolidColorBrushPicker : Page
    {
        private bool _causeChanges = true;
        private Regex _digitRegex = new Regex("[^0-9]+");

        public ColorPicker Picker { get; set; }

        public SolidColorBrushPicker(ColorPicker picker, SolidColorBrush? defaultBrush = null)
        {
            Picker = picker;
            this.Loaded += (s, e) =>
            {
                if (defaultBrush != null)
                {
                    Picker.SelectedBrush = defaultBrush;
                }
                else
                {
                    Picker.SelectedBrush = new SolidColorBrush(Colors.Aqua);
                }

                OpacitySlider.Value = 1d;
            };

            InitializeComponent();
        }

        public void SetSelectedColor(SolidColorBrush brush)
        {
            PreviewColorRectangle.Fill = Picker.SelectedBrush = brush;
            var color = brush.Color;
            color.A = 255;

            OpacityRectangle.Fill = new SolidColorBrush(color);
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            GetColorFromCanvas();
            SetEllipseOnCanvas();
            UpdateTextOutput();
        }

        private void ColorPickerCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GetColorFromCanvas();
            SetEllipseOnCanvas();
            UpdateTextOutput();
        }

        private void SetEllipseOnCanvas()
        {
            var position = Mouse.GetPosition(ColorPickerCanvas);

            Canvas.SetLeft(ColorPickerEllipse, position.X - ColorPickerEllipse.ActualWidth / 2d);
            Canvas.SetTop(ColorPickerEllipse, position.Y - ColorPickerEllipse.ActualHeight / 2d);
        }

        private void GetColorFromCanvas(bool useMousePosition = true)
        {
            var winFormsColor = 
                Pipette.GetColorAt(useMousePosition ? 
                    Pipette.GetMousePosition() :
                    new System.Drawing.Point(
                        (int) (Canvas.GetLeft(ColorPickerEllipse) + ColorPickerEllipse.ActualWidth / 2d + ColorPickerCanvas.TranslatePoint(new Point(0, 0), CenterNode.AppWindow).X + CenterNode.AppWindow.Left),
                        (int) (Canvas.GetTop(ColorPickerEllipse) +ColorPickerEllipse.ActualHeight / 2d + ColorPickerCanvas.TranslatePoint(new Point(0, 0), CenterNode.AppWindow).Y + CenterNode.AppWindow.Top)));

            var wpfColor = new Color()
            {
                R = winFormsColor.R,
                G = winFormsColor.G,
                B = winFormsColor.B,
                A = (byte)(255 * OpacitySlider.Value),
            };

            var solidColorBrush = new SolidColorBrush()
            {
                Color = wpfColor
            };

            wpfColor.A = 255;
            PreviewColorRectangle.Fill = Picker.SelectedBrush = solidColorBrush;
            OpacityRectangle.Fill = new SolidColorBrush(wpfColor);
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSelectedColor();
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var hueBrush = FindResource("HueBrush") as LinearGradientBrush;

            var selectedBaseColor = hueBrush.GradientStops.GetRelativeColor(1 - ColorSlider.Value);
            Resources["CurrentColor"] = selectedBaseColor;

            if (!_causeChanges)
            {
                return;
            }

            GetColorFromCanvas(false);
            UpdateTextOutput();
        }

        private void UpdateSelectedColor()
        {
            var newBrush = Picker.SelectedBrush.Clone();

            var solidColorBrush = Picker.SelectedBrush.Clone() as SolidColorBrush;
            var color = solidColorBrush.Color;
            color.A = (byte)(255 * OpacitySlider.Value);

            solidColorBrush.Color = color;
            PreviewColorRectangle.Fill = Picker.SelectedBrush = solidColorBrush;

            color.A = 255;
            OpacityRectangle.Fill = new SolidColorBrush(color);
            UpdateTextOutput();
        }

        private void UpdateTextOutput()
        {
            _causeChanges = false;
            var color = (Picker.SelectedBrush as SolidColorBrush).Color;

            HexCodeTextBox.Text = Pipette.ToHex(color);
            RedValueTextBox.Text = color.R.ToString();
            GreenValueTextBox.Text = color.G.ToString();
            BlueValueTextBox.Text = color.B.ToString();
            AlphaValueTextBox.Text = color.A.ToString();
           
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                var hsvValue = color.ToHsv();
                var offsetX = hsvValue.Saturation * 0.92 * ColorPickerCanvas.ActualWidth + 0.04 * ColorPickerCanvas.ActualWidth - ColorPickerEllipse.ActualWidth / 2d;
                var offsetY = (1 - hsvValue.Brightness) * 0.92 * ColorPickerCanvas.ActualHeight + 0.04 * ColorPickerCanvas.ActualHeight - ColorPickerEllipse.ActualHeight / 2d;
                var hueSliderValue = 1 - hsvValue.Hue / 360d;

                ColorSlider.Value = hueSliderValue;
                Canvas.SetLeft(ColorPickerEllipse, offsetX);
                Canvas.SetTop(ColorPickerEllipse, offsetY);
            }
           

            _causeChanges = true;
        }

        private void HexCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            try
            {
                var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(HexCodeTextBox.Text);
                if (brush == null)
                {
                    CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: invalid hex-code");
                    return;
                }

                PreviewColorRectangle.Fill = Picker.SelectedBrush = brush;
                UpdateSelectedColor();
            }

            catch
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: invalid hex-code");
                return;
            }
        }

        private void RedValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var success = int.TryParse(RedValueTextBox.Text, out int redValue);
            if (!success || redValue > 255)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: the color value cannot be > 255");
                return;
            }

            var solidColorBrush = Picker.SelectedBrush.Clone() as SolidColorBrush;
            var color = solidColorBrush.Color;
            color.R = (byte)redValue;
            solidColorBrush.Color = color;

            PreviewColorRectangle.Fill = Picker.SelectedBrush = solidColorBrush;
            UpdateSelectedColor();
        }

        private void GreenValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var success = int.TryParse(GreenValueTextBox.Text, out int greenValue);
            if (!success || greenValue > 255)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: the color value cannot be > 255");
                return;
            }

            var solidColorBrush = Picker.SelectedBrush.Clone() as SolidColorBrush;
            var color = solidColorBrush.Color;
            color.G = (byte) greenValue;
            solidColorBrush.Color = color;

            PreviewColorRectangle.Fill = Picker.SelectedBrush = solidColorBrush;
            UpdateSelectedColor();
        }

        private void BlueValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var success = int.TryParse(BlueValueTextBox.Text, out int blueValue);
            if (!success || blueValue > 255)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: the color value cannot be > 255");
                return;
            }

            var solidColorBrush = Picker.SelectedBrush.Clone() as SolidColorBrush;
            var color = solidColorBrush.Color;
            color.B = (byte)blueValue;
            solidColorBrush.Color = color;

            PreviewColorRectangle.Fill = Picker.SelectedBrush = solidColorBrush;
            UpdateSelectedColor();
        }

        private void AlphaValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var success = int.TryParse(AlphaValueTextBox.Text, out int alphaValue);
            if (!success || alphaValue > 255)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: the color value cannot be > 255");
                return;
            }

            OpacitySlider.Value = alphaValue / 255d;
            UpdateSelectedColor();
        }

        private void ValidateTextInputOnDigits(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _digitRegex.IsMatch(e.Text);
        }
    }
}
