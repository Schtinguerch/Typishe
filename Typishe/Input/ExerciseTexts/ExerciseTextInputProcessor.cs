using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Typishe.Exercises;

namespace Typishe.Input
{
    public class ExerciseTextInputProcessor
    {
        private ExerciseView _exerciseView;
        public ExerciseView ExerciseView
        {
            get => _exerciseView;
            set
            {
                if (_exerciseView != null)
                {
                    TextInputStatus.TextEntered -= _exerciseView.ExerciseControls.TextView.UpdateTextOutput;
                    TextInputStatus.MistakeEntered -= _exerciseView.ExerciseControls.TextView.UpdateTextOutput;

                    _exerciseView.HiddenTextBlock.PreviewKeyDown -= HiddenTextBlockPreviewKeyDown;
                    _exerciseView.HiddenTextBlock.PreviewKeyUp -= HiddenTextBlockPreviewKeyUp;
                }

                _exerciseView = value;
                _exerciseView.HiddenTextBlock.Focus();

                TextInputStatus.TextEntered += _exerciseView.ExerciseControls.TextView.UpdateTextOutput;
                TextInputStatus.MistakeEntered += _exerciseView.ExerciseControls.TextView.UpdateTextOutput;

                _exerciseView.HiddenTextBlock.PreviewKeyDown += HiddenTextBlockPreviewKeyDown;
                _exerciseView.HiddenTextBlock.PreviewKeyUp += HiddenTextBlockPreviewKeyUp;
            }
        }

        private int _modifierIndex = 0;
        private void HiddenTextBlockPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.RightAlt)
            {
                _modifierIndex = 0;
            }
        }

        private void HiddenTextBlockPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _modifierIndex = 1;
                return;
            }

            if (e.Key == Key.RightAlt)
            {
                _modifierIndex = 2;
                return;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.RightAlt)
            {
                _modifierIndex = 3;
                return;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Windows)
            {
                return;
            }

            if (e.Key == Key.Back)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    TextInputStatus.DeleteLastMistakenWord();
                }

                else
                {
                    TextInputStatus.CorrectOneMistakeCharacter();
                }

                return;
            }


            var inputCharacter = KeyboardLayoutConverter.EmulatedInputtedString(e.Key, _modifierIndex);
            var correctTyping = TextInputStatus.TryToEnterNext(inputCharacter);

            if (correctTyping)
            {
                if (string.IsNullOrEmpty(TextInputStatus.LeftText))
                {
                    _exerciseView.ExerciseControls.Keyboard?.HighlightKeyAt((-1, -1));
                    return;
                }

                _exerciseView.ExerciseControls.Keyboard?.HighlightKey(TextInputStatus.LeftText[0].ToString());
            }

            else
            {
                //_exerciseView.Keyboard?.HighlightKey(inputCharacter, true);
            }
        }

        public TextInputStatus TextInputStatus { get; set; }

        public ExerciseTextInputProcessor(ExerciseView exerciseView)
        {
            TextInputStatus = new TextInputStatus();
            ExerciseView = exerciseView;
        }
    }
}
