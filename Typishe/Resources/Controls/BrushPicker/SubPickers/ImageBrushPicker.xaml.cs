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
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Typishe.Resources.Controls.BrushPicker.SubPickers
{
    /// <summary>
    /// Логика взаимодействия для ImageBrushPicker.xaml
    /// </summary>
    public partial class ImageBrushPicker : Page
    {
        private bool _causeChanges = false;
        private Regex _digitRegex = new Regex("[^0-9]+");

        public ColorPicker Picker { get; set; }

        private List<TileMode> _timeModeNames = new List<TileMode>()
        {
            TileMode.None,
            TileMode.Tile,
            TileMode.FlipX,
            TileMode.FlipY,
            TileMode.FlipXY,
        };

        private List<Stretch> _stretchNames = new List<Stretch>()
        {
            Stretch.None,
            Stretch.Fill,
            Stretch.Uniform,
            Stretch.UniformToFill,
        };

        public ImageBrushPicker(ColorPicker picker, ImageBrush? defaultBrush = null)
        {
            Picker = picker;
            Loaded += (s, e) =>
            {
                if (defaultBrush != null)
                {
                    Picker.SelectedBrush = CurrentImageBrush = defaultBrush;
                }

                else
                {
                    ImageUriTextBox.Text = "https://www.99images.com/download-image/953078/2560x1600";
                    Picker.SelectedBrush = CurrentImageBrush;
                }

                _causeChanges = false;
                TileModeComboBox.SelectedIndex = _timeModeNames.IndexOf(CurrentImageBrush.TileMode);
                StretchComboBox.SelectedIndex = _stretchNames.IndexOf(CurrentImageBrush.Stretch);

                ViewportScaleXSlider.Value = CurrentImageBrush.Viewbox.Width * 100d;
                ViewportScaleYSlider.Value = CurrentImageBrush.Viewbox.Height * 100d;

                ViewportScaleXTextBox.Text = (CurrentImageBrush.Viewbox.Width * 100d).ToString();
                ViewportScaleYTextBox.Text = (CurrentImageBrush.Viewbox.Height * 100d).ToString();
                _causeChanges = true;
            };

            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif;)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                ImageUriTextBox.Text = dialog.FileName;
                Picker.SelectedBrush = CurrentImageBrush;
            }
        }

        private void TileModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var brush = CurrentImageBrush;
            brush.TileMode = _timeModeNames[TileModeComboBox.SelectedIndex];

            Picker.SelectedBrush = brush;
        }

        private void StretchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var brush = CurrentImageBrush;
            brush.Stretch = _stretchNames[StretchComboBox.SelectedIndex];

            Picker.SelectedBrush = brush;
        }

        private void ViewportScaleXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var viewportScaleX = int.Parse(ViewportScaleXTextBox.Text);
            ViewportScaleXSlider.Value = viewportScaleX;
        }

        private void ViewportScaleXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var viewport = CurrentImageBrush.Viewport;
            CurrentImageBrush.Viewport = new Rect(viewport.X, viewport.Y, ViewportScaleXSlider.Value / 100d, viewport.Height);
            Picker.SelectedBrush = CurrentImageBrush;

            _causeChanges = false;
            ViewportScaleXTextBox.Text = ViewportScaleXSlider.Value.ToString();
            _causeChanges = true;
        }

        private void ViewportScaleYTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var viewportScaleY = int.Parse(ViewportScaleYTextBox.Text);
            ViewportScaleYSlider.Value = viewportScaleY;
        }

        private void ViewportScaleYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_causeChanges)
            {
                return;
            }

            var viewport = CurrentImageBrush.Viewport;
            CurrentImageBrush.Viewport = new Rect(viewport.X, viewport.Y, viewport.Width, ViewportScaleYSlider.Value / 100d);
            Picker.SelectedBrush = CurrentImageBrush;

            _causeChanges = false;
            ViewportScaleYTextBox.Text = ViewportScaleYSlider.Value.ToString();
            _causeChanges = true;
        }

        private void ValidateTextInputOnDigits(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _digitRegex.IsMatch(e.Text);
        }
    }
}
