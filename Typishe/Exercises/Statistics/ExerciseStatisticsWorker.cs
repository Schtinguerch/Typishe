using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Typishe.Setup;
using TypisheApi.Setup;
using Typishe.Service;
using Typishe.Input;
using Typishe.Exercises;
using Localization = TypisheApi.Localizations.Resources;

namespace Typishe.Exercises
{
    public class ExerciseStatisticsWorker
    {
        private NumberDisplayConverter _converter;

        public int _currentProgress = 0;
        public int TotalProgressWeight { get; set; }

        public TextBlock TypingSpeedTextBlock { get; set; }
        public TextBlock TypingTimeTextBlock { get; set; }
        public TextBlock PassPercentageTextBlock { get; set; }
        public TextInputStatus InputStatus { get; set; }

        private ExerciseSection _currentSection;
        public ExerciseSection CurrentSection
        {
            get => _currentSection;
            set
            {
                _currentSection = value;
                _timer.Restart();
            }
        }

        private ExerciseTimer _timer;

        public ExerciseStatisticsWorker(TextInputStatus inputStatus, TextBlock typingSpeedTextBlock, TextBlock typingTimeTextBlock, TextBlock passPercentageTextBlock)
        {
            _converter = new NumberDisplayConverter();
            InputStatus = inputStatus;

            TypingSpeedTextBlock = typingSpeedTextBlock;
            TypingTimeTextBlock = typingTimeTextBlock;
            PassPercentageTextBlock = passPercentageTextBlock;

            _timer = new ExerciseTimer();
            _timer.Tick += ExerciseTimerTick;

            ResetOutput();
        }

        public void ResetOutput()
        {
            _timer.StopAndReset();
            _currentProgress = 0;

            TypingTimeTextBlock.Text = "00:00.000";
            TypingSpeedTextBlock.Text = $"{_converter.Convert(0)} {TypingSpeedConvert[ApplicationSetup.FunctionalSetup.TypingSpeedMeasure].Value}";
        }

        public void UpdatePassPercentage(double addPercentage)
        {
            if (_currentSection == null)
            {
                return;
            }

            _currentProgress += (int)(_currentSection.ProgressWeight * addPercentage);
            var percentage = 100d * _currentProgress / TotalProgressWeight;

            PassPercentageTextBlock.Text = $"{_converter.Convert(percentage)} %";
        }


        private void ExerciseTimerTick(TimeSpan timeElapsed)
        {
            if (CurrentSection is not TextExerciseSection)
            {
                TypingTimeTextBlock.Text = timeElapsed.ToString("mm\\:ss\\.fff");
                TypingSpeedTextBlock.Text = string.Empty;
                return;
            }

            var timeToPass = (CurrentSection as TextExerciseSection).TimeToPass;
            var charactersPerMinute = InputStatus.CharartersEntered / timeElapsed.TotalMinutes;

            TypingTimeTextBlock.Text = $"{timeElapsed:mm\\:ss\\.fff} / {timeToPass:mm\\:ss\\.fff}";
            var measure = TypingSpeedConvert[ApplicationSetup.FunctionalSetup.TypingSpeedMeasure];

            var measuredSpeed = charactersPerMinute * measure.Key;
            TypingSpeedTextBlock.Text = $"{_converter.Convert(measuredSpeed)} {measure.Value}";
        }
        
        private Dictionary<TypingSpeedMeasure, KeyValuePair<double, string>> TypingSpeedConvert = new Dictionary<TypingSpeedMeasure, KeyValuePair<double, string>>()
        {
            { TypingSpeedMeasure.CharactersPerMinute, new KeyValuePair<double, string>(1, Localization.CharactersPerMinute) },
            { TypingSpeedMeasure.CharactersPerSecond, new KeyValuePair<double, string>(1 / 60d, Localization.CharactersPerSecond) },
            { TypingSpeedMeasure.WordsPerMinute, new KeyValuePair<double, string>(1 / 5d, Localization.WordsPerMinute) },
            { TypingSpeedMeasure.OsuBumpsPerMinute, new KeyValuePair<double, string>(1 / 4d, Localization.BumpsPerMinute) },
        };
    }
}
