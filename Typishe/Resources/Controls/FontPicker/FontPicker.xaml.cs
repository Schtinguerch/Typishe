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
using System.Drawing.Text;
using Typishe.Service;

namespace Typishe.Resources.Controls.FontPicker
{
    /// <summary>
    /// Логика взаимодействия для FontPicker.xaml
    /// </summary>
    public partial class FontPicker : UserControl
    {
        public delegate void FontChangedHandler(FontData fontData);
        public delegate void FontAcceptedHandler(FontData selectedBrush);
        public delegate void FontCanceledHandler(FontData standardBrush);

        public event FontChangedHandler FontChanged;
        public event FontAcceptedHandler FontAccepted;
        public event FontCanceledHandler FontCanceled;

        private bool _causeChanges = true;
        private FontData _standardFontData;

        private TextDecorationCollection[] _textDecorations = new TextDecorationCollection[]
        {
            null,
            TextDecorations.Strikethrough,
            TextDecorations.Baseline,
            TextDecorations.Underline,
            TextDecorations.OverLine,
        };

        private FontWeight[] _fontWeights = new FontWeight[]
        {
            FontWeight.FromOpenTypeWeight(1),
            FontWeight.FromOpenTypeWeight(100),
            FontWeight.FromOpenTypeWeight(200),
            FontWeight.FromOpenTypeWeight(300),
            FontWeight.FromOpenTypeWeight(400),
            FontWeight.FromOpenTypeWeight(500),
            FontWeight.FromOpenTypeWeight(600),
            FontWeight.FromOpenTypeWeight(700),
            FontWeight.FromOpenTypeWeight(800),
            FontWeight.FromOpenTypeWeight(900),
            FontWeight.FromOpenTypeWeight(999),
        };

        public bool EnablePreview
        {
            get => EnablePreviewComboBox.IsChecked == null ? false : (bool)EnablePreviewComboBox.IsChecked;
            set => EnablePreviewComboBox.IsChecked = value;
        }

        public FontPicker()
        {
            InitializeComponent();
            Show(new FontData()
            {
                FontFamily = Typishe.Setup.ApplicationSetup.VisualSetup.ExerciseFontFamily,
                FontSize = Typishe.Setup.ApplicationSetup.VisualSetup.ExerciseFontSize,
                FontWeight = Typishe.Setup.ApplicationSetup.VisualSetup.ExerciseFontWeight,
                TextDecorations = TextDecorations.Underline,
            });

            var fonts = new InstalledFontCollection();
            foreach (var font in fonts.Families)
            {
                FontFamilyComboBox.Items.Add(font.Name);
            }
        }

        public void Show(FontData fontData)
        {
            Visibility = Visibility.Visible;
            _standardFontData = fontData.Clone();

            _causeChanges = false;
            SetTextBlockViewByData(fontData);

            int index = -1;
            FontFamilyComboBox.Text = fontData.FontFamily.Source;

            foreach (var item in TextDecorationsComboBox.Items)
            {
                index += 1;
                if (fontData.TextDecorations == _textDecorations[index])
                {
                    TextDecorationsComboBox.SelectedIndex = index;
                    break;
                }
            }

            index = -1;
            foreach (var item in FontWeightComboBox.Items)
            {
                index += 1;
                if (fontData.FontWeight == _fontWeights[index])
                {
                    FontWeightComboBox.SelectedIndex = index;
                    break;
                }
            }

            FontSizeSlider.Value = fontData.FontSize;
            FontSizeTextBox.Text = fontData.FontSize.ToString("N2");

            _causeChanges = true;
        }

        private void TextDecorationsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            TestTextBlock.TextDecorations = _textDecorations[TextDecorationsComboBox.SelectedIndex];
            GetAndNotifyUpdatedFontData();
        }

        private void FontWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            TestTextBlock.FontWeight = (FontWeightComboBox.Items[FontWeightComboBox.SelectedIndex] as TextBlock).FontWeight;
            GetAndNotifyUpdatedFontData();
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_causeChanges)
            {
                return;
            }

            TestTextBlock.FontSize = FontSizeSlider.Value;
            GetAndNotifyUpdatedFontData();

            _causeChanges = false;
            FontSizeTextBox.Text = TestTextBlock.FontSize.ToString("N2");
            _causeChanges = true;
        }

        private void FontFamilyComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            TestTextBlock.FontFamily = new FontFamily(FontFamilyComboBox.Text);
            GetAndNotifyUpdatedFontData();
        }

        private void FontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_causeChanges)
            {
                return;
            }

            if (!FontSizeTextBox.Text.TryParse(out double fontSize))
            {
                return;
            }

            TestTextBlock.FontSize = fontSize;
            GetAndNotifyUpdatedFontData();
        }

        private void SetTextBlockViewByData(FontData data)
        {
            TestTextBlock.FontFamily = data.FontFamily;
            TestTextBlock.FontSize = data.FontSize;
            TestTextBlock.FontWeight = data.FontWeight;
            TestTextBlock.TextDecorations = data.TextDecorations;
        }

        private void GetAndNotifyUpdatedFontData(bool enforce = false)
        {
            if (!EnablePreview && !enforce)
            {
                return;
            }

            FontChanged?.Invoke(new FontData()
            {
                FontFamily = TestTextBlock.FontFamily,
                FontSize = TestTextBlock.FontSize,
                FontWeight = TestTextBlock.FontWeight,
                TextDecorations = TestTextBlock.TextDecorations,
            });
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            GetAndNotifyUpdatedFontData(true);
            this.Visibility = Visibility.Collapsed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FontChanged?.Invoke(_standardFontData);
            this.Visibility = Visibility.Collapsed;
        }
        public void ClearEventDelegates()
        {
            if (FontAccepted != null)
            {
                foreach (var eventMethod in FontAccepted.GetInvocationList())
                {
                    FontAccepted -= (FontAcceptedHandler)eventMethod;
                }
            }

            if (FontCanceled != null)
            {
                foreach (var eventMethod in FontCanceled.GetInvocationList())
                {
                    FontCanceled -= (FontCanceledHandler)eventMethod;
                }
            }

            if (FontChanged != null)
            {
                foreach (var eventMethod in FontChanged.GetInvocationList())
                {
                    FontChanged -= (FontChangedHandler)eventMethod;
                }
            }
        }
    }
}
