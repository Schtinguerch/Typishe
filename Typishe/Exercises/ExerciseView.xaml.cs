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
using Typishe.Exercises.SectionViews;
using Typishe.Input;
using Typishe.Input.ExerciseTextViews;
using Keyboard = Typishe.Input.Keyboard;

namespace Typishe.Exercises
{
    /// <summary>
    /// Логика взаимодействия для ExerciseView.xaml
    /// </summary>
    public partial class ExerciseView : UserControl
    {
        public Course Course { get; set; }
        public Exercise Exercise { get; set; }

        public ExerciseStatisticsWorker StatisticsWorker { get; set; }
        public ExerciseControls ExerciseControls { get; set; }
        public ExerciseTextInputProcessor Processor { get; set; }

        private Func<ExerciseSection, bool>[] _conditions;
        private Action<ExerciseSection>[] _loadSectionActions;

        private ExerciseSection _currentSection;
        private CountDownView _lastCountDownView;

        public ExerciseView(ExerciseControls controls)
        {
            InitializeComponent();
            ExerciseControls = controls;

            Processor = new ExerciseTextInputProcessor(this);
            StatisticsWorker = new ExerciseStatisticsWorker(
                Processor.TextInputStatus, 
                ExerciseControls.TypingSpeedTextBlock, 
                ExerciseControls.TypingTimeTextBlock, 
                ExerciseControls.PassPercentageTextBlock);

            Course = new Course(this);
            Exercise = new Exercise(this);
            Exercise.SectionStarted += section => StatisticsWorker.CurrentSection = section;

            Exercise.ExerciseFinished += () =>
            {
                StatisticsWorker.ResetOutput();
                MessageBox.Show("The end");
            };

            _conditions = new Func<ExerciseSection, bool>[]
            {
                s => s is TextExerciseSection,
                s => s is MultimediaExerciseSection,
            };

            _loadSectionActions = new Action<ExerciseSection>[]
            {
                LoadTextInputSection,
                LoadMultimediaSection,
            };

            MouseDown += (s, e) => HiddenTextBlock.Focus();
            Processor.TextInputStatus.TextEnterFinished += Exercise.LoadNextSection;
            Processor.TextInputStatus.TextEntered += TextInputStatus_TextEntered;
        }

        private void TextInputStatus_TextEntered(string enteredText, string leftText)
        {
            if (Processor.TextInputStatus.CharartersEntered == 0)
            {
                return;
            }

            var addPoints = 1d / Processor.TextInputStatus.ExerciseText.Length;
            StatisticsWorker.UpdatePassPercentage(addPoints);
        }

        public void OpenSection(ExerciseSection section)
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i](section))
                {
                    _loadSectionActions[i](section);
                    return;
                }
            }
        }

        public void LoadTextInputSection(ExerciseSection section)
        {
            var textExerciseSection = section as TextExerciseSection;
            StatisticsWorker.ExerciseStatistics.SectionStatistics.Add(new TextInputSectionStatistics());

            ExerciseComponentViewFrame.Navigate(ExerciseControls.TextView as Page);
            Processor.TextInputStatus.SetNewText(textExerciseSection.TextToInput);

            ExerciseControls.Keyboard.HighlightKey(textExerciseSection.TextToInput[0].ToString());

            HiddenTextBlock.IsEnabled = true;
            HiddenTextBlock.Focus();
        }

        public void LoadMultimediaSection(ExerciseSection section)
        {
            var multimediaSection = section as MultimediaExerciseSection;
            var multimediaView = new MultimediaView(multimediaSection);

            ExerciseComponentViewFrame.Navigate(multimediaView);
            multimediaSection.SectionFinished += () =>
            {
                StatisticsWorker.UpdatePassPercentage(1);
                Exercise.LoadNextSection();
            };

            HiddenTextBlock.IsEnabled = false;
        }

        public void StartCountDown()
        {
            _lastCountDownView = new CountDownView();
            ExerciseComponentViewFrame.Navigate(_lastCountDownView);

            _lastCountDownView.CountdownFinished += Exercise.LoadNextSection;
        }

        public void LoadPreviewPage()
        {
            if (_lastCountDownView != null)
            {
                _lastCountDownView.CountdownFinished -= Exercise.LoadNextSection;
                _lastCountDownView = null;

                GC.Collect();
            }

            var previewPage = new ExercisePreView();
            ExerciseComponentViewFrame.Navigate(previewPage);

            previewPage.ExerciseStarted += () => StartCountDown();
        }
    }
}
