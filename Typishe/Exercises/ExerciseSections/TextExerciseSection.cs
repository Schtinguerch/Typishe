using System;
using Typishe.Setup;

namespace Typishe.Exercises
{
    public class TextExerciseSection : ExerciseSection
    {
        public delegate void SectionFinishedHandler();
        public event SectionFinishedHandler SectionFinished;

        public TimeSpan TimeToPass { get; set; }

        private string _textToInput;
        public string TextToInput
        {
            get => _textToInput;
            set
            {
                var text = value;
                var replacements = ApplicationSetup.FunctionalSetup.ExerciseTextReplacements;

                if (replacements != null)
                {
                    foreach (var replacement in replacements)
                    {
                        text = text.Replace(replacement.Key, replacement.Value);
                    }
                }

                _textToInput = text;
            }
        }
    }
}
