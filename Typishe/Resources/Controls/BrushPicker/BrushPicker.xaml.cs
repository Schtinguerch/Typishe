using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Typishe.Resources.Controls.BrushPicker.SubPickers;

namespace Typishe.Resources.Controls.BrushPicker
{
    /// <summary>
    /// Логика взаимодействия для BrushPicker.xaml
    /// </summary>
    public partial class BrushPicker : UserControl
    {
        public delegate void ColorAcceptedHandler(Brush selectedBrush);
        public delegate void ColorCanceledHandler(Brush standardBrush);

        public event ColorAcceptedHandler ColorAccepted;
        public event ColorCanceledHandler ColorCanceled;

        public ColorPicker Picker { get; init; } = new ColorPicker();
        public bool EnablePreview
        {
            get => EnablePreviewComboBox.IsChecked == null? false : (bool) EnablePreviewComboBox.IsChecked;
            set => EnablePreviewComboBox.IsChecked = value;
        }

        public BrushPicker()
        {
            InitializeComponent();
        }

        public void Show(Brush standardBrush)
        {
            Visibility = Visibility.Visible;
            Picker.StandardBrush = standardBrush;

            Action action = standardBrush switch
            {
                var b when b is SolidColorBrush => () => OpenSolidColorBrushPicker(b as SolidColorBrush),
                var b when b is LinearGradientBrush => () => OpenLinearGradientBrushPicker(b as LinearGradientBrush),
                var b when b is RadialGradientBrush => () => OpenRadialGradientBrushPicker(b as RadialGradientBrush),
                var b when b is ImageBrush => () => OpenImageBrushPicker(b as ImageBrush),
                var b when b is VisualBrush => () => OpenVisualBrushPicker(b as VisualBrush),
                _ => () => { }
            };

            action();
        }

        private void OpenSolidColorBrushPicker(SolidColorBrush? standardBrush = null)
        {
            var solidColorPicker = new SolidColorBrushPicker(Picker, standardBrush);
            BrushPickerFrame.Navigate(solidColorPicker);
        }

        private void OpenLinearGradientBrushPicker(LinearGradientBrush? standardBrush = null)
        {
            var linearGradientPicker = new LinearGradientBrushPicker(Picker, standardBrush);
            BrushPickerFrame.Navigate(linearGradientPicker);
        }

        private void OpenRadialGradientBrushPicker(RadialGradientBrush? standardBrush = null)
        {
            var radialGradientPicker = new RadialGradientBrushPicker(Picker, standardBrush);
            BrushPickerFrame.Navigate(radialGradientPicker);
        }

        private void OpenImageBrushPicker(ImageBrush? standardBrush = null)
        {
            var imagePicker = new ImageBrushPicker(Picker, standardBrush);
            BrushPickerFrame.Navigate(imagePicker);
        }

        private void OpenVisualBrushPicker(VisualBrush? standardBrush = null)
        {
            var visualBrushPicker = new VisualBrushPicker(Picker, standardBrush);
            BrushPickerFrame.Navigate(visualBrushPicker);
        }

        private void SolidColorBrushButton_Click(object sender, RoutedEventArgs e) => OpenSolidColorBrushPicker();
        private void LinearGradientBrushButton_Click(object sender, RoutedEventArgs e) => OpenLinearGradientBrushPicker();
        private void RadialGradientBrushButton_Click(object sender, RoutedEventArgs e) => OpenRadialGradientBrushPicker();
        private void ImageBrushButton_Click(object sender, RoutedEventArgs e) => OpenImageBrushPicker();
        private void VisualBrushButton_Click(object sender, RoutedEventArgs e) => OpenVisualBrushPicker();

        private void ApplyBrushButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            ColorAccepted?.Invoke(Picker.SelectedBrush);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            ColorCanceled?.Invoke(Picker.StandardBrush);
        }
    }
}
