using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace TypisheApi.Setup
{
    public class VisualSetup : INotifyPropertyChanged
    {
        #region Backrgound Functionality
        public delegate void ParallaxEnabledHandler(bool enabled);
        public event ParallaxEnabledHandler ParallaxEnabledChanged;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private Color _shadowColor;
        private Brush 
            _bloomShadowBrush,
            _additionalWallpaperBrush,
            _backgroundBrush, 
            _backgroundFontBrush, 
            _menuBrush, 
            _menuFontBrush, 
            _controlBorderBrush, 
            _controlBackgroundBrush, 
            _controlFontBrush, 
            _accentBrush, 
            _accentFontBrush, 
            _accentWarningBrush, 
            _exerciseCharactersBrush, 
            _raidedExersiseCharactersBrush;

        private FontFamily
            _generalFontFamily,
            _keyboardFontFamily,
            _exerciseFontFamily,
            _raidedExerciseFontFamily;

        private double
            _generalFontSize,
            _keyboardFontSize,
            _keyboardSmallKeysFontSize,
            _exerciseFontSize,
            _raidedExerciseFontSize,
            _keyCornerRadius,
            _keyBorderThickness,
            _blurRadius,
            _typingSucceedSoundVolume,
            _typingMistakeSoundVolume,
            _mouseClickSoundVolume,
            _exerciseBeginsSoundVolume,
            _exerciseEndSoundVolume;

        private FontWeight
            _generalFontWeight,
            _keyboardFontWeight,
            _exerciseFontWeight,
            _raidedExerciseFontWeight;

        private TextDecorationCollection
            _generalTextDecoration,
            _keyboardTextDecoration;

        private Brush
            _keyboardBackgroundBrush,
            _keyboardBorderBrush,
            _keyboardHighlightBrush,
            _keyboardHighlightBorderBrush,
            _keyboardMistakeBrush,
            _keyboardFontBrush;
        
        public VisualSetup Clone()
        {
            var cloneSetup = new VisualSetup();

            cloneSetup.GeneralFontFamily = GeneralFontFamily;
            cloneSetup.GeneralFontSize = GeneralFontSize;
            cloneSetup.GeneralFontWeight = GeneralFontWeight;
            cloneSetup.GeneralFontDecorations = GeneralFontDecorations;

            cloneSetup.KeyboardFontFamily = KeyboardFontFamily;
            cloneSetup.KeyboardFontSize = KeyboardFontSize;
            cloneSetup.KeyboardSmallKeysFontSize = KeyboardSmallKeysFontSize;
            cloneSetup.KeyboardFontWeight = KeyboardFontWeight;
            cloneSetup.KeyboardFontDecorations = KeyboardFontDecorations;

            cloneSetup.ExerciseFontFamily = ExerciseFontFamily;
            cloneSetup.ExerciseFontSize = ExerciseFontSize;
            cloneSetup.ExerciseFontWeight = ExerciseFontWeight;

            cloneSetup.RaidedExerciseFontFamily = RaidedExerciseFontFamily;
            cloneSetup.RaidedExerciseFontSize = RaidedExerciseFontSize;
            cloneSetup.RaidedExerciseFontWeight = RaidedExerciseFontWeight;

            cloneSetup.BloomShadowBrush = BloomShadowBrush;

            cloneSetup.BackgroundBrush = BackgroundBrush;
            cloneSetup.BackgroundFontBrush = BackgroundFontBrush;
            
            cloneSetup.MenuBrush = MenuBrush;
            cloneSetup.MenuFontBrush = MenuFontBrush;

            cloneSetup.ControlBorderBrush = ControlBorderBrush;
            cloneSetup.ShadowColor = ShadowColor;

            cloneSetup.ControlBackgroundBrush = ControlBackgroundBrush;
            cloneSetup.ControlFontBrush = ControlFontBrush;

            cloneSetup.AccentBrush = AccentBrush;
            cloneSetup.AccentFontBrush = AccentFontBrush;

            cloneSetup.ExerciseCharactersBrush = ExerciseCharactersBrush;
            cloneSetup.RaidedExerciseCharactersBrush = RaidedExerciseCharactersBrush;


            cloneSetup.KeyboardBackgroundBrush = KeyboardBackgroundBrush;
            cloneSetup.KeyboardBorderBrush = KeyboardBorderBrush;
            cloneSetup.KeyboardFontBrush = KeyboardFontBrush;
            cloneSetup.KeyboardHighlightBrush = KeyboardHighlightBrush;
            cloneSetup.KeyboardMistakeBrush = KeyboardMistakeBrush;


            cloneSetup.AdditionalWallpaperBrush = AdditionalWallpaperBrush;
            cloneSetup.AdditionalBackgroundWallpaperEffect = AdditionalBackgroundWallpaperEffect;
            cloneSetup.EnableParallaxEffect = EnableParallaxEffect;
            cloneSetup.BlurRadius = BlurRadius;

            cloneSetup.TypingSucceedSoundPath = TypingSucceedSoundPath;
            cloneSetup.TypingMistakeSoundPath = TypingMistakeSoundPath;
            cloneSetup.MouseClickSoundPath = MouseClickSoundPath;
            cloneSetup.ExerciseBeginsSoundPath = ExerciseBeginsSoundPath;
            cloneSetup.ExerciseEndSoundPath = ExerciseEndSoundPath;

            cloneSetup.TypingSucceedSoundVolume = TypingSucceedSoundVolume;
            cloneSetup.TypingMistakeSoundVolume = TypingMistakeSoundVolume;
            cloneSetup.MouseClickSoundVolume = MouseClickSoundVolume;
            cloneSetup.ExerciseBeginsSoundVolume = ExerciseBeginsSoundVolume;
            cloneSetup.ExerciseEndSoundVolume = ExerciseEndSoundVolume;

            return cloneSetup;
        }
        public string SetupName { get; set; }
        #endregion

        #region Fonts
        public FontFamily GeneralFontFamily
        {
            get => _generalFontFamily;
            set
            {
                _generalFontFamily = value;
                OnPropertyChanged(nameof(GeneralFontFamily));
            }
        }

        public double GeneralFontSize
        {
            get => _generalFontSize;
            set
            {
                _generalFontSize = value;
                OnPropertyChanged(nameof(GeneralFontSize));
            }
        }

        public FontWeight GeneralFontWeight
        {
            get => _generalFontWeight;
            set
            {
                _generalFontWeight = value;
                OnPropertyChanged(nameof(GeneralFontWeight));
            }
        }

        public TextDecorationCollection GeneralFontDecorations
        {
            get => _generalTextDecoration; 
            set
            {
                _generalTextDecoration = value;
                OnPropertyChanged(nameof(GeneralFontDecorations));
            }
        }

        public FontFamily KeyboardFontFamily 
        {
            get => _keyboardFontFamily;
            set 
            {
                _keyboardFontFamily = value;
                OnPropertyChanged(nameof(KeyboardFontFamily));
            }
        }

        public double KeyboardFontSize 
        {
            get => _keyboardFontSize;
            set 
            {
                _keyboardFontSize = value;
                OnPropertyChanged(nameof(KeyboardFontSize));
            }
        }

        public double KeyboardSmallKeysFontSize 
        {
            get => _keyboardSmallKeysFontSize;
            set 
            {
                _keyboardSmallKeysFontSize = value;
                OnPropertyChanged(nameof(KeyboardSmallKeysFontSize));
            }
        }

        public FontWeight KeyboardFontWeight 
        {
            get => _keyboardFontWeight;
            set 
            {
                _keyboardFontWeight = value;
                OnPropertyChanged(nameof(KeyboardFontWeight));
            }
        }

        public TextDecorationCollection KeyboardFontDecorations
        {
            get => _keyboardTextDecoration;
            set
            {
                _keyboardTextDecoration = value;
                OnPropertyChanged(nameof(KeyboardFontDecorations));
            }
        }

        public FontFamily ExerciseFontFamily
        {
            get => _exerciseFontFamily;
            set
            {
                _exerciseFontFamily = value;
                OnPropertyChanged(nameof(ExerciseFontFamily));
            }
        }

        public double ExerciseFontSize
        {
            get => _exerciseFontSize;
            set
            {
                _exerciseFontSize = value;
                OnPropertyChanged(nameof(ExerciseFontSize));
            }
        }

        public FontWeight ExerciseFontWeight
        {
            get => _exerciseFontWeight;
            set
            {
                _exerciseFontWeight = value;
                OnPropertyChanged(nameof(ExerciseFontWeight));
            }
        }

        public FontFamily RaidedExerciseFontFamily
        {
            get => _raidedExerciseFontFamily;
            set
            {
                _raidedExerciseFontFamily = value;
                OnPropertyChanged(nameof(RaidedExerciseFontFamily));
            }
        }

        public double RaidedExerciseFontSize
        {
            get => _raidedExerciseFontSize;
            set
            {
                _raidedExerciseFontSize = value;
                OnPropertyChanged(nameof(RaidedExerciseFontSize));
            }
        }

        public FontWeight RaidedExerciseFontWeight
        {
            get => _raidedExerciseFontWeight;
            set
            {
                _raidedExerciseFontWeight = value;
                OnPropertyChanged(nameof(RaidedExerciseFontWeight));
            }
        }
        #endregion

        #region General Brushes

        public Brush BloomShadowBrush
        {
            get => _bloomShadowBrush;
            set
            {
                _bloomShadowBrush = value;
                OnPropertyChanged(nameof(BloomShadowBrush));
            }
        }

        public Brush BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                _backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }

        public Brush BackgroundFontBrush
        {
            get => _backgroundFontBrush;
            set
            {
                _backgroundFontBrush = value;
                OnPropertyChanged(nameof(BackgroundFontBrush));
            }
        }

        public Brush MenuBrush
        {
            get => _menuBrush;
            set
            {
                _menuBrush = value;
                OnPropertyChanged(nameof(MenuBrush));
            }
        }

        public Brush MenuFontBrush
        {
            get => _menuFontBrush; 
            set
            {
                _menuFontBrush = value;
                OnPropertyChanged(nameof(MenuFontBrush));
            }
        }

        public Brush ControlBorderBrush
        {
            get => _controlBorderBrush;
            set
            {
                _controlBorderBrush = value;
                OnPropertyChanged(nameof(ControlBorderBrush));
            }
        }

        public Color ShadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                OnPropertyChanged(nameof(ShadowColor));
            }
        }

        public Brush ControlBackgroundBrush
        {
            get => _controlBackgroundBrush; 
            set
            {
                _controlBackgroundBrush = value;
                OnPropertyChanged(nameof(ControlBackgroundBrush));
            }
        }

        public Brush ControlFontBrush
        {
            get => _controlFontBrush;
            set
            {
                _controlFontBrush = value;
                OnPropertyChanged(nameof(ControlFontBrush));
            }
        }

        public Brush AccentBrush
        {
            get => _accentBrush;
            set
            {
                _accentBrush = value;
                OnPropertyChanged(nameof(AccentBrush));
            }
        }

        public Brush AccentFontBrush
        { 
            get => _accentFontBrush; set
            {
                _accentFontBrush = value;
                OnPropertyChanged(nameof(AccentFontBrush));
            } 
        }

        public Brush AccentWarningBrush
        {
            get => _accentWarningBrush;
            set
            {
                _accentWarningBrush = value;
                OnPropertyChanged(nameof(AccentWarningBrush));
            }
        }

        public Brush ExerciseCharactersBrush
        { 
            get => _exerciseCharactersBrush; 
            set
            {
                _exerciseCharactersBrush = value;
                OnPropertyChanged(nameof(ExerciseCharactersBrush));
            } 
        }

        public Brush RaidedExerciseCharactersBrush
        {
            get => _raidedExersiseCharactersBrush;
            set
            {
                _raidedExersiseCharactersBrush = value;
                OnPropertyChanged(nameof(RaidedExerciseCharactersBrush));
            }
        }
        #endregion

        #region Keyboard Brushes
        public Brush KeyboardBackgroundBrush 
        {
            get => _keyboardBackgroundBrush;
            set
            {
                _keyboardBackgroundBrush = value;
                OnPropertyChanged(nameof(KeyboardBackgroundBrush));
            }
        }
        public Brush KeyboardBorderBrush 
        {
            get => _keyboardBorderBrush;
            set
            {
                _keyboardBorderBrush = value;
                OnPropertyChanged(nameof(KeyboardBorderBrush));
            }
        }
        public Brush KeyboardFontBrush 
        {
            get => _keyboardFontBrush;
            set
            {
                _keyboardFontBrush = value;
                OnPropertyChanged(nameof(KeyboardFontBrush));
            }
        }
        public Brush KeyboardHighlightBrush 
        {
            get => _keyboardHighlightBrush;
            set
            {
                _keyboardHighlightBrush = value;
                OnPropertyChanged(nameof(KeyboardHighlightBrush));
            }
        }
        public Brush KeyboardHighlightBorderBrush 
        {
            get => _keyboardHighlightBorderBrush;
            set
            {
                _keyboardHighlightBorderBrush = value;
                OnPropertyChanged(nameof(KeyboardHighlightBorderBrush));
            }
        }
        public Brush KeyboardMistakeBrush 
        {
            get => _keyboardMistakeBrush;
            set
            {
                _keyboardMistakeBrush = value;
                OnPropertyChanged(nameof(KeyboardMistakeBrush));
            }
        }
        public double KeyBorderThickness 
        {
            get => _keyBorderThickness;
            set
            {
                _keyBorderThickness = value;
                OnPropertyChanged(nameof(KeyBorderThickness));
            }
        }
        public double KeyCornerRadius 
        {
            get => _keyCornerRadius;
            set
            {
                _keyCornerRadius = value;
                OnPropertyChanged(nameof(KeyCornerRadius));
            }
        }
        #endregion

        #region Background Additionals
        private bool _enableParallaxEffect;
        private Effect _additionalBackgroundWallpaperEffect;
        private KernelType _blurType;

        public Brush AdditionalWallpaperBrush
        {
            get => _additionalWallpaperBrush;
            set
            {
                _additionalWallpaperBrush = value;
                OnPropertyChanged(nameof(AdditionalWallpaperBrush));
            }
        }

        public bool EnableParallaxEffect
        {
            get => _enableParallaxEffect;
            set
            {
                _enableParallaxEffect = value;
                ParallaxEnabledChanged?.Invoke(value);
            }
        }

        public Effect AdditionalBackgroundWallpaperEffect
        {
            get => _additionalBackgroundWallpaperEffect;
            set
            {
                _additionalBackgroundWallpaperEffect = value;
                OnPropertyChanged(nameof(AdditionalBackgroundWallpaperEffect));
            }
        }

        public double BlurRadius
        {
            get => _blurRadius;
            set
            {
                _blurRadius = value;
                OnPropertyChanged(nameof(BlurRadius));
            }
        }

        public KernelType BlurType
        {
            get => _blurType;
            set
            {
                _blurType = value;
                OnPropertyChanged(nameof(BlurType));
            }
        }

        public KeyHighlight KeyboardKeyHighlight { get; set; }
        #endregion

        #region Sounds
        public string TypingSucceedSoundPath { get; set; }
        public double TypingSucceedSoundVolume
        {
            get => _typingSucceedSoundVolume;
            set
            {
                _typingSucceedSoundVolume = value;
                OnPropertyChanged(nameof(TypingSucceedSoundVolume));
            }
        }

        public string TypingMistakeSoundPath { get; set; }
        public double TypingMistakeSoundVolume 
        {
            get => _typingMistakeSoundVolume;
            set
            {
                _typingMistakeSoundVolume = value;
                OnPropertyChanged(nameof(TypingMistakeSoundVolume));
            }
        }

        public string MouseClickSoundPath { get; set; }
        public double MouseClickSoundVolume 
        {
            get => _mouseClickSoundVolume;
            set
            {
                _mouseClickSoundVolume = value;
                OnPropertyChanged(nameof(MouseClickSoundVolume));
            }
        }

        public string ExerciseBeginsSoundPath { get; set; }
        public double ExerciseBeginsSoundVolume 
        {
            get => _exerciseBeginsSoundVolume;
            set
            {
                _exerciseBeginsSoundVolume = value;
                OnPropertyChanged(nameof(ExerciseBeginsSoundVolume));
            }
        }

        public string ExerciseEndSoundPath { get; set; }
        public double ExerciseEndSoundVolume 
        {
            get => _exerciseEndSoundVolume;
            set
            {
                _exerciseEndSoundVolume = value;
                OnPropertyChanged(nameof(ExerciseEndSoundVolume));
            }
        }
        #endregion
    }
}
