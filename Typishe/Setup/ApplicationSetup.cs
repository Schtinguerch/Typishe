using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Media;
using System.Windows;

using Typishe.Service;
using TypisheApi.Setup;
using Commands = TypisheApi.Localizations.Commands;

using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Typishe.Setup
{
    public static class ApplicationSetup
    {
        public static void BackToDefault()
        {
            VisualSetup = _standardSetup;
            FunctionalSetup = _standardFuncSetup;
            ExerciseSetup = _standardExerciseSetup;

            _openedExerciseSetup = "";
            _openedFuncSetupPath = "";
            _openedVisualSetupPath = "";
        }

        #region VisualSetup
        private static string _openedVisualSetupPath;
        private static VisualSetup _standardSetup = new VisualSetup()
        {
            SetupName = "Typishe standard",

            GeneralFontFamily = new FontFamily("Segoe UI"),
            GeneralFontSize = 16d,
            GeneralFontWeight = FontWeights.Bold,
            GeneralFontDecorations = null,

            KeyboardFontFamily = new FontFamily("Segoe UI"),
            KeyboardFontSize = 22d,
            KeyboardSmallKeysFontSize = 12d,
            KeyboardFontWeight = FontWeights.Bold,
            KeyboardFontDecorations = null,

            ExerciseFontFamily = new FontFamily("Segoe UI"),
            ExerciseFontSize = 36d,
            ExerciseFontWeight = FontWeights.Bold,

            RaidedExerciseFontFamily = new FontFamily("Segoe UI"),
            RaidedExerciseFontSize = 36d,
            RaidedExerciseFontWeight = FontWeights.Bold,

            BloomShadowBrush = Brushes.Black,
            BackgroundBrush = "#1E1E1E".ToSolidSolorBrush(),
            BackgroundFontBrush = Brushes.White,

            MenuBrush = "#121212".ToSolidSolorBrush(),
            MenuFontBrush = Brushes.White,

            ControlBorderBrush = "#0B0B0B".ToSolidSolorBrush(),
            ShadowColor = Colors.Black,

            ControlBackgroundBrush = "#FF191919".ToSolidSolorBrush(),
            ControlFontBrush = Brushes.White,

            AccentBrush = "#00BFFF".ToSolidSolorBrush(),
            AccentFontBrush = Brushes.Black,
            AccentWarningBrush = Brushes.Red,

            ExerciseCharactersBrush = Brushes.White,
            RaidedExerciseCharactersBrush = "#004884".ToSolidSolorBrush(),

            KeyboardBackgroundBrush = "#88262626".ToSolidSolorBrush(),
            KeyboardBorderBrush = "#191919".ToSolidSolorBrush(),
            KeyboardFontBrush = Brushes.White,
            KeyboardHighlightBrush = "#17C4F5".ToSolidSolorBrush(),
            KeyboardMistakeBrush = Brushes.Red,
            KeyBorderThickness = 1d,
            KeyCornerRadius = 6d,

            AdditionalWallpaperBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("https://www.99images.com/download-image/953078/2560x1600")),
                Stretch = Stretch.UniformToFill,
            },

            EnableParallaxEffect = true,
            BlurRadius = 0d,
            BlurType = System.Windows.Media.Effects.KernelType.Gaussian,
            AdditionalBackgroundWallpaperEffect = null,
            KeyboardKeyHighlight = KeyHighlight.NextKeyHint,
        };

        public static VisualSetup VisualSetup { get; set; }

        public static bool LoadVisualSetup(string folderPath, bool loadStandardIfFails = false)
        {
            if (!Directory.Exists(folderPath))
            {
                return InterruptSetup($"folder searching \"{folderPath}\"", loadStandardIfFails);
            }

            var jsonHeaderPath = Path.Combine(folderPath, "Parameters.json");
            if (!File.Exists(jsonHeaderPath))
            {
                return InterruptSetup($"parameters header searching \"{jsonHeaderPath}\"", loadStandardIfFails);
            }

            var jsonCode = File.ReadAllText(jsonHeaderPath).Replace("@folder", folderPath);

            try
            {
                var visualSetup = JsonConvert.DeserializeObject<VisualSetup>(jsonCode);

                VisualSetup = visualSetup;
                _openedVisualSetupPath = folderPath;

                Logger.Add("Visual setup loading", "SUCCESS");
                return true;
            }
            
            catch (Exception ex)
            {
                return InterruptSetup($"JSON parcing \"{jsonHeaderPath}\", {ex.Message}", loadStandardIfFails);
            }
        }

        private static bool InterruptSetup(string visualSetupStep, bool loadStandardIfFails)
        {
            var processName = $"Visual setup loading: {visualSetupStep}";
            Logger.Add(processName, "FAILED");

            if (loadStandardIfFails) VisualSetup = _standardSetup;
            return false;
        }

        public static bool SaveVisualSetup()
        {
            var jsonHeaderPath = Path.Combine(_openedVisualSetupPath, "Parameters.json");
            var jsonCode = JsonConvert.SerializeObject(VisualSetup.Clone(), Formatting.Indented);

            try
            {
                File.WriteAllText(jsonHeaderPath, jsonCode.Replace(_openedVisualSetupPath, "@folder"));

                Logger.Add("Visual setup export", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                Logger.Add($"Visual setup export: write to file \"{jsonHeaderPath}\", {ex.Message}", "FAILED");
                return false;
            }
        }

        public static bool ExportVisualSetup(string folderPath)
        {
            var imageFolderPath = Path.Combine(folderPath, "Images");
            var soundFolderPath = Path.Combine(folderPath, "Sounds");

            if (!CreateDirectoriesIfNotExists(folderPath, imageFolderPath, soundFolderPath))
            {
                return false;
            }

            var cloneSetup = VisualSetup.Clone();
            var allBrushes = new List<Brush>()
            {
                cloneSetup.BackgroundBrush,
                cloneSetup.BackgroundFontBrush,
                cloneSetup.MenuBrush,
                cloneSetup.MenuFontBrush,
                cloneSetup.ControlBorderBrush,
                cloneSetup.ControlBackgroundBrush,
                cloneSetup.ControlFontBrush,
                cloneSetup.ExerciseCharactersBrush,
                cloneSetup.RaidedExerciseCharactersBrush,
                cloneSetup.KeyboardBackgroundBrush,
                cloneSetup.KeyboardBorderBrush,
                cloneSetup.KeyboardFontBrush,
                cloneSetup.KeyboardHighlightBrush,
                cloneSetup.KeyboardMistakeBrush
            };

            int index = 0;
            foreach (var brush in allBrushes)
            {
                if (!(brush is ImageBrush))
                {
                    continue;
                }

                var imageBrush = brush as ImageBrush;
                var bitmapImage = (imageBrush.ImageSource as BitmapImage);

                if (bitmapImage == null)
                {
                    continue;
                }

                var imagePath = bitmapImage.UriSource.AbsolutePath;
                var setupImagePath = Path.Combine(imageFolderPath, $"Image {++index:D2)}.{Path.GetExtension(imagePath)}");

                CopyFile(imagePath, setupImagePath);
                bitmapImage.UriSource = new Uri(setupImagePath);
            }

            var allSoundPaths = new List<string>()
            {
                cloneSetup.TypingSucceedSoundPath,
                cloneSetup.TypingMistakeSoundPath,
                cloneSetup.MouseClickSoundPath,
                cloneSetup.ExerciseBeginsSoundPath,
                cloneSetup.ExerciseEndSoundPath,
            };

            index = 0;
            foreach (var path in allSoundPaths)
            {
                var setupSoundPath = Path.Combine(soundFolderPath, $"Sound {++index:D2)}.{Path.GetExtension(path)}");
                CopyFile(path, setupSoundPath);
            }

            var jsonHeaderPath = Path.Combine(folderPath, "Parameters.json");
            var jsonCode = JsonConvert.SerializeObject(cloneSetup.Clone(), Formatting.Indented);

            try
            {
                File.WriteAllText(jsonHeaderPath.Replace(folderPath, "@folder"), jsonCode);

                Logger.Add("Visual setup export", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                Logger.Add($"Visual setup export: write to file \"{jsonHeaderPath}\", {ex.Message}", "FAILED");
                return false;
            }
        }

        private static bool CopyFile(string from, string to)
        {
            if (from.Contains("http"))
            {
                try
                {
                    var client = new WebClient();
                    client.DownloadFile(from, to);

                    Logger.Add($"Download file \"{from}\" to \"{to}\"", "SUCCESS");
                    return true;
                }

                catch (Exception ex)
                {
                    Logger.Add($"Download file \"{from}\" to \"{to}\", {ex.Message}", "FAILED");
                    return false;
                }
            }

            else
            {
                try
                {
                    File.Copy(from, to);

                    Logger.Add($"Copy file \"{from}\" to \"{to}\"", "SUCCESS");
                    return true;
                }

                catch (Exception ex)
                {
                    Logger.Add($"Copy file \"{from}\" to \"{to}\", {ex.Message}", "FAILED");
                    return false;
                }
            } 
        }

        private static bool CreateDirectoriesIfNotExists(params string[] folderPaths)
        {
            foreach (var folderPath in folderPaths)
            {
                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        Logger.Add($"Create folder \"{folderPath}\"", "SUCCESS");
                    }

                    catch (Exception ex)
                    {
                        Logger.Add($"Create folder \"{folderPath}\", {ex.Message}", "FAILED");
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region FunctionalSetup
        private static string _openedFuncSetupPath;
        private static FunctionalSetup _standardFuncSetup = new FunctionalSetup() 
        { 
            RequireCorrectInputMistakes = true,
            ApplicationLanguageCode = "en-US",
            TypingSpeedMeasure = TypingSpeedMeasure.CharactersPerMinute,
            NumberDisplay = NumberDisplay.ArabicNumbers,
            ShortCuts = new Dictionary<List<Key>, string>()
            {
                { new List<Key>() { Key.LeftCtrl, Key.J }, Commands.ReloadExercise },
                { new List<Key>() { Key.LeftCtrl, Key.U }, Commands.LoadNextExercise },
                { new List<Key>() { Key.LeftCtrl, Key.M }, Commands.LoadPreviousExercise },
                { new List<Key>() { Key.LeftCtrl, Key.H }, Commands.LoadRandomExercise },
                { new List<Key>() { Key.Escape }, Commands.BackToStartPage },
            }
        };

        public static FunctionalSetup FunctionalSetup { get; set; }

        public static bool LoadFunctionalSetup(string filename, bool loadStandardIfFails = false)
        {
            if (!File.Exists(filename))
            {
                return InterruptFuncSetup($"filename searching \"{filename}\"", loadStandardIfFails);
            }

            var jsonCode = File.ReadAllText(filename);
            try
            {
                var functionalSetup = JsonConvert.DeserializeObject<FunctionalSetup>(jsonCode);

                _openedFuncSetupPath = filename;
                FunctionalSetup = functionalSetup;

                Logger.Add("Functional setup loading", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                return InterruptFuncSetup($"JSON parcing \"{filename}\", {ex.Message}", loadStandardIfFails);
            }
        }

        private static bool InterruptFuncSetup(string functionallSetupStep, bool loadStandardIfFails)
        {
            var processName = $"Functional setup loading: {functionallSetupStep}";
            Logger.Add(processName, "FAILED");

            if (loadStandardIfFails) FunctionalSetup = _standardFuncSetup;
            return false;
        }
        
        public static bool SaveFunctionalSetup(string filename = "")
        {
            if (string.IsNullOrWhiteSpace(filename)) filename = _openedFuncSetupPath;
            var jsonCode = JsonConvert.SerializeObject(FunctionalSetup, Formatting.Indented);

            try
            {
                File.WriteAllText(filename, jsonCode);

                Logger.Add("Functional setup export", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                Logger.Add($"Functional setup export: write to file \"{filename}\", {ex.Message}", "FAILED");
                return false;
            }
        }

        public static bool ExportFunctionalSetup(string filename)
        {
            return SaveFunctionalSetup(filename);
        }
        #endregion

        #region ExerciseSetup
        private static string _openedExerciseSetup;
        private static ExerciseSetup _standardExerciseSetup = new ExerciseSetup();

        public static ExerciseSetup ExerciseSetup { get; set; }

        public static bool LoadExerciseSetup(string filename, bool loadStandardIfFails = false)
        {
            if (!File.Exists(filename))
            {
                return InterruptExerciseSetup($"filename searching \"{filename}\"", loadStandardIfFails);
            }

            var jsonCode = File.ReadAllText(filename);
            try
            {
                var exerciseSetup = JsonConvert.DeserializeObject<ExerciseSetup>(jsonCode);

                _openedExerciseSetup = filename;
                ExerciseSetup = exerciseSetup;

                Logger.Add("Functional setup loading", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                return InterruptExerciseSetup($"JSON parcing \"{filename}\", {ex.Message}", loadStandardIfFails);
            }
        }

        private static bool InterruptExerciseSetup(string functionallSetupStep, bool loadStandardIfFails)
        {
            var processName = $"Exercise setup loading: {functionallSetupStep}";
            Logger.Add(processName, "FAILED");

            if (loadStandardIfFails) ExerciseSetup = _standardExerciseSetup;
            return false;
        }

        public static bool SaveExerciseSetup(string filename = "")
        {
            if (string.IsNullOrWhiteSpace(filename)) filename = _openedExerciseSetup;
            var jsonCode = JsonConvert.SerializeObject(ExerciseSetup, Formatting.Indented);

            try
            {
                File.WriteAllText(filename, jsonCode);

                Logger.Add("Exercise setup export", "SUCCESS");
                return true;
            }

            catch (Exception ex)
            {
                Logger.Add($"Exercise setup export: write to file \"{filename}\", {ex.Message}", "FAILED");
                return false;
            }
        }

        public static bool ExportExerciseSetup(string filename)
        {
            return SaveExerciseSetup(filename);
        }
        #endregion
    }
}
