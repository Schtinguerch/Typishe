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
using Typishe.Resources.Controls;
using Typishe.Setup;
using TypisheApi.Setup;

namespace Typishe.Pages.TypingPageSubpages
{
    /// <summary>
    /// Логика взаимодействия для ApplicationSettingsPage.xaml
    /// </summary>
    public partial class VisualSetupPage : Page, IRequestable
    {
        private Dictionary<Button, Action> _actionsByButton, _fontActionByButton;
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

                { EditControlBorderBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.ControlBorderBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.ControlBorderBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.ControlBorderBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.ControlBorderBrush);
                }},

                { EditAccentBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.AccentBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.AccentBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.AccentBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.AccentBrush);
                }},

                { EditWarningAccentBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.AccentWarningBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.AccentWarningBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.AccentWarningBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.AccentWarningBrush);
                }},

                { EditBackgroundFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.BackgroundFontBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.BackgroundFontBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.BackgroundFontBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.BackgroundFontBrush);
                }},

                { EditMenuFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.MenuFontBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.MenuFontBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.MenuFontBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.MenuFontBrush);
                }},

                { EditControlFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.ControlFontBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.ControlFontBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.ControlFontBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.ControlFontBrush);
                }},

                { EditAccentFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.AccentFontBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.AccentFontBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.AccentFontBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.AccentFontBrush);
                }},

                { EditExerciseFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.ExerciseCharactersBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.ExerciseCharactersBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.ExerciseCharactersBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.ExerciseCharactersBrush);
                }},

                { EditRaidedExerciseFontBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.RaidedExerciseCharactersBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.RaidedExerciseCharactersBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.RaidedExerciseCharactersBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.RaidedExerciseCharactersBrush);
                }},

                { EditAdditionalWallpaperBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.AdditionalWallpaperBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.AdditionalWallpaperBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.AdditionalWallpaperBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.AdditionalWallpaperBrush);
                }},

                { EditKeyboardBackgroundBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.KeyboardBackgroundBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.KeyboardBackgroundBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.KeyboardBackgroundBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.KeyboardBackgroundBrush);
                }},

                { EditKeyboardBorderBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.KeyboardBorderBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.KeyboardBorderBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.KeyboardBorderBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.KeyboardBorderBrush);
                }},

                { EditKeyboardHighlightBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.KeyboardHighlightBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.KeyboardHighlightBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.KeyboardHighlightBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.KeyboardHighlightBrush);
                }},

                { EditKeyboardHighlightBorderBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.KeyboardHighlightBorderBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.KeyboardHighlightBorderBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.KeyboardHighlightBorderBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.KeyboardHighlightBorderBrush);
                }},

                { EditKeyboardMistakeBrushButton, () =>
                {
                    BrushPicker.ClearEventDelegates();
                    BrushPicker.Picker.SelectedColorChanged += (brush) =>
                    {
                        if (BrushPicker.EnablePreview)
                        {
                            ApplicationSetup.VisualSetup.KeyboardMistakeBrush = brush;
                        }
                    };

                    BrushPicker.ColorAccepted += (brush) => ApplicationSetup.VisualSetup.KeyboardMistakeBrush = brush;
                    BrushPicker.ColorCanceled += (brush) => ApplicationSetup.VisualSetup.KeyboardMistakeBrush = brush;

                    BrushPicker.Show(ApplicationSetup.VisualSetup.KeyboardMistakeBrush);
                }},
            };

            _fontActionByButton = new Dictionary<Button, Action>()
            {
                { EditGeneralFontButton, () =>
                {
                    FontPicker.ClearEventDelegates();
                    var fontChangedMethod = new Action<FontData>((fontData) =>
                    {
                        ApplicationSetup.VisualSetup.GeneralFontFamily = fontData.FontFamily;
                        ApplicationSetup.VisualSetup.GeneralFontSize = fontData.FontSize;
                        ApplicationSetup.VisualSetup.GeneralFontWeight = fontData.FontWeight;
                        ApplicationSetup.VisualSetup.GeneralFontDecorations = fontData.TextDecorations;
                    });

                    FontPicker.FontChanged += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontAccepted += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontCanceled += (fontData) => fontChangedMethod(fontData);

                    FontPicker.Show(new FontData()
                    {
                        FontFamily = ApplicationSetup.VisualSetup.GeneralFontFamily,
                        FontSize = ApplicationSetup.VisualSetup.GeneralFontSize,
                        FontWeight = ApplicationSetup.VisualSetup.GeneralFontWeight,
                        TextDecorations = ApplicationSetup.VisualSetup.GeneralFontDecorations,
                    });
                }},

                { EditKeyboardFontButton, () =>
                {
                    FontPicker.ClearEventDelegates();
                    var fontChangedMethod = new Action<FontData>((fontData) =>
                    {
                        ApplicationSetup.VisualSetup.KeyboardFontFamily = fontData.FontFamily;
                        ApplicationSetup.VisualSetup.KeyboardFontSize = fontData.FontSize;
                        ApplicationSetup.VisualSetup.KeyboardFontWeight = fontData.FontWeight;
                        ApplicationSetup.VisualSetup.KeyboardFontDecorations = fontData.TextDecorations;
                    });

                    FontPicker.FontChanged += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontAccepted += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontCanceled += (fontData) => fontChangedMethod(fontData);

                    FontPicker.Show(new FontData()
                    {
                        FontFamily = ApplicationSetup.VisualSetup.KeyboardFontFamily,
                        FontSize = ApplicationSetup.VisualSetup.KeyboardFontSize,
                        FontWeight = ApplicationSetup.VisualSetup.KeyboardFontWeight,
                        TextDecorations = ApplicationSetup.VisualSetup.KeyboardFontDecorations,
                    });
                }},

                { EditExerciseFontButton, () =>
                {
                    FontPicker.ClearEventDelegates();
                    var fontChangedMethod = new Action<FontData>((fontData) =>
                    {
                        ApplicationSetup.VisualSetup.ExerciseFontFamily = fontData.FontFamily;
                        ApplicationSetup.VisualSetup.ExerciseFontSize = fontData.FontSize;
                        ApplicationSetup.VisualSetup.ExerciseFontWeight = fontData.FontWeight;
                    });

                    FontPicker.FontChanged += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontAccepted += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontCanceled += (fontData) => fontChangedMethod(fontData);

                    FontPicker.Show(new FontData()
                    {
                        FontFamily = ApplicationSetup.VisualSetup.ExerciseFontFamily,
                        FontSize = ApplicationSetup.VisualSetup.ExerciseFontSize,
                        FontWeight = ApplicationSetup.VisualSetup.ExerciseFontWeight,
                        TextDecorations = null,
                    });
                }},

                { EditRaidedExerciseFontButton, () =>
                {
                    FontPicker.ClearEventDelegates();
                    var fontChangedMethod = new Action<FontData>((fontData) =>
                    {
                        ApplicationSetup.VisualSetup.RaidedExerciseFontFamily = fontData.FontFamily;
                        ApplicationSetup.VisualSetup.RaidedExerciseFontSize = fontData.FontSize;
                        ApplicationSetup.VisualSetup.RaidedExerciseFontWeight = fontData.FontWeight;
                    });

                    FontPicker.FontChanged += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontAccepted += (fontData) => fontChangedMethod(fontData);
                    FontPicker.FontCanceled += (fontData) => fontChangedMethod(fontData);

                    FontPicker.Show(new FontData()
                    {
                        FontFamily = ApplicationSetup.VisualSetup.RaidedExerciseFontFamily,
                        FontSize = ApplicationSetup.VisualSetup.RaidedExerciseFontSize,
                        FontWeight = ApplicationSetup.VisualSetup.RaidedExerciseFontWeight,
                        TextDecorations = null,
                    });
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
            //BrushPicker.Show(new SolidColorBrush(Colors.Aqua));

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
            var button = sender as Button;
            var relativePosition = RelativePoint(button, BackgroundGrid);
            relativePosition.Y += 48d;

            Canvas.SetTop(FontPicker, relativePosition.Y);

            if (relativePosition.Y > 200d)
            {
                VerticalScroller.TargetVerticalOffset = relativePosition.Y - 60d;
            }

            if (_fontActionByButton.TryGetValue(button, out var selectPickerAction))
            {
                selectPickerAction();
            }
        }

        private void UseAdditionalBackgroundCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void EnableBackgroundParallaxEffect_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void EditAdditionalWallpaperEffectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private Point RelativePoint(UIElement element, UIElement relativeTo)
        {
            return element.TransformToAncestor(relativeTo).Transform(new Point(0, 0));
        }
    }
}
