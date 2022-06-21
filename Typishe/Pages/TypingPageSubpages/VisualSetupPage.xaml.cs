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

using Typishe.Setup;
using TypisheApi.Setup;

namespace Typishe.Pages.TypingPageSubpages
{
    /// <summary>
    /// Логика взаимодействия для ApplicationSettingsPage.xaml
    /// </summary>
    public partial class VisualSetupPage : Page, IRequestable
    {
        private Dictionary<Button, Action> _actionsByButton;
        private VisualSetup _standardVisualSetup;

        public VisualSetupPage()
        {
            InitializeComponent();
            _standardVisualSetup = ApplicationSetup.VisualSetup.Clone();

            #region SetActionByButtons
            _actionsByButton = new Dictionary<Button, Action>()
            {
                { EditBackgroundBrushButton, () => 
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.BackgroundBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.BackgroundBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.BackgroundBrush = brush;
                    
                    BrushPicker.Show(ApplicationSetup.VisualSetup.BackgroundBrush);
                }},

                { EditMenuBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.MenuBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.MenuBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.MenuBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.MenuBrush);
                }},

                { EditControlBackgroundBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.ControlBackgroundBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.ControlBackgroundBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.ControlBackgroundBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.ControlBackgroundBrush);
                }},
            };
            #endregion
        }

        public void ApplySettings()
        {
            throw new NotImplementedException();
        }

        public void DiscardSettings()
        {
            throw new NotImplementedException();
        }

        public void NewFile()
        {
            throw new NotImplementedException();
        }

        public void OpenFile()
        {
            throw new NotImplementedException();
        }

        public void SaveFile()
        {
            throw new NotImplementedException();
        }

        public void SaveFileAs()
        {
            throw new NotImplementedException();
        }

        private void EditBrushOnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var relativePosition = RelativePoint(button, BackgroundGrid);
            relativePosition.Y += 48d;

            Canvas.SetTop(BrushPicker, relativePosition.Y);
            BrushPicker.Show(new SolidColorBrush(Colors.Aqua));

            if (relativePosition.Y > 200d)
            {
                VerticalScroller.TargetVerticalOffset = relativePosition.Y - 60d;
            }

            if (_actionsByButton.TryGetValue(button, out var selectPickerAction))
            {
                selectPickerAction();
            }
        }

        private void EditFontOnClick(object sender, RoutedEventArgs e)
        {

        }

        private Point RelativePoint(UIElement element, UIElement relativeTo)
        {
            return element.TransformToAncestor(relativeTo).Transform(new Point(0, 0));
        }
    }
}
