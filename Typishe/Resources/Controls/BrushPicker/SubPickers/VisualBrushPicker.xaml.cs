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
using System.Windows.Markup;
using System.Xml;
using System.IO;
using Typishe.Setup;

namespace Typishe.Resources.Controls.BrushPicker.SubPickers
{
    /// <summary>
    /// Логика взаимодействия для VisualBrushPicker.xaml
    /// </summary>
    public partial class VisualBrushPicker : Page
    {
        public ColorPicker Picker { get; set; }

        public VisualBrushPicker(ColorPicker picker, VisualBrush? defaultBrush = null)
        {
            Picker = picker;
            InitializeComponent();

            if (defaultBrush != null)
            {
                PreviewBrushRectangle.Fill = Picker.SelectedBrush = defaultBrush;
            }
        }

        private void ApplyVisualBrushButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedBrush = new VisualBrush(ConvertXamlToVisual(XamlCodeTextBox.Text));
                PreviewBrushRectangle.Fill = Picker.SelectedBrush = selectedBrush;
            }
            
            catch
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error: cannot convent XAML-code into brush");
            }
        }

        private Visual ConvertXamlToVisual(string xamlCode)
        {
            var header= "<Grid Background=\"Transparent\" Width=\"100\" Height=\"100\"\n" +
                        "      xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\n" +
                        "      xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n" +
                        "      xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\">\n";

            xamlCode = $"{header}{xamlCode}</Grid>";

            var stringReader = new StringReader(xamlCode);
            var xmlReader = XmlReader.Create(stringReader);

            var grid = XamlReader.Load(xmlReader) as Grid;
            return grid;
        }
    }
}
