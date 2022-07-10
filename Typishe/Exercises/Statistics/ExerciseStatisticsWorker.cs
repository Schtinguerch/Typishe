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

        private int _currentProgress = 0;
        private int _ticksCount = 0;

        private int _previousCharCount = 0;
        private TimeSpan _previousTime = TimeSpan.Zero;
        
        public int TotalProgressWeight { get; set; }
        public ExerciseStatistics ExerciseStatistics { get; set; }

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
            ExerciseStatistics = new ExerciseStatistics() { PassStartDateTime = DateTime.Now };

            _currentProgress = 0;
            _ticksCount = 0;

            _previousCharCount = 0;
            _previousTime = TimeSpan.Zero;

            TypingTimeTextBlock.Text = "00:00.000";
            TypingSpeedTextBlock.Text = $"{_converter.Convert(0)} {TypingSpeedConvert[ApplicationSetup.FunctionalSetup.TypingSpeedMeasure].Value}";
            PassPercentageTextBlock.Text = $"{_converter.Convert(0)} %";
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
            _ticksCount += 1;
            var currentTextSection = CurrentSection as TextExerciseSection;

            if (currentTextSection == null)
            {
                TypingTimeTextBlock.Text = timeElapsed.ToString("mm\\:ss\\.fff");
                TypingSpeedTextBlock.Text = string.Empty;
                return;
            }

            var timeToPass = currentTextSection.TimeToPass;
            var charactersPerMinute = InputStatus.CharartersEntered / timeElapsed.TotalMinutes;

            TypingTimeTextBlock.Text = $"{timeElapsed:mm\\:ss\\.fff} / {timeToPass:mm\\:ss\\.fff}";
            var measure = TypingSpeedConvert[ApplicationSetup.FunctionalSetup.TypingSpeedMeasure];

            var measuredSpeed = charactersPerMinute * measure.Key;
            TypingSpeedTextBlock.Text = $"{_converter.Convert(measuredSpeed)} {measure.Value}";

            if (_ticksCount % 20 == 0) //Invoke that code every second
            {
                var charEntered = InputStatus.CharartersEntered - _previousCharCount;
                var secondElapsed = (timeElapsed - _previousTime).TotalSeconds;

                _previousCharCount = InputStatus.CharartersEntered;
                _previousTime = timeElapsed;

                var secondInstantCpm = charEntered / secondElapsed * 60d;
                var inputStatistics = ExerciseStatistics.SectionStatistics.Last() as TextInputSectionStatistics;

                inputStatistics.SecondaryAverageCpm.Add(charactersPerMinute);
                inputStatistics.SecondaryInstCpm.Add(secondInstantCpm);
            }
        }

        public void CollectLeftStatistics()
        {
            if (CurrentSection is TextExerciseSection)
            {
                var charactersPerMinute = InputStatus.CharartersEntered / _timer.Elapsed.TotalMinutes;
                var inputStatistics = ExerciseStatistics.SectionStatistics.Last() as TextInputSectionStatistics;

                inputStatistics.AverageTypingSpeedCpm = charactersPerMinute;
                inputStatistics.PassTime = _timer.Elapsed;
            }
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
