using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using Typishe.Input;
using Typishe.Input.ExerciseTextViews;

namespace Typishe.Exercises
{
    public class ExerciseControls
    {
        public TextBlock ExerciseNameTextBlock { get; set; }
        public TextBlock TypingSpeedTextBlock { get; set; }
        public TextBlock TypingTimeTextBlock { get; set; }
        public TextBlock PassPercentageTextBlock { get; set; }
        public Keyboard Keyboard { get; set; }
        public IExerciseTextView TextView { get; set; }

        public ExerciseControls() { }

        public ExerciseControls(
            TextBlock exerciseNameTextBlock,
            TextBlock typingSpeedTextBlock,
            TextBlock typingTimeTextBlock,
            TextBlock passPercentageTextBlock,
            Keyboard keyboard,
            IExerciseTextView textView)
        {
            ExerciseNameTextBlock = exerciseNameTextBlock;
            TypingSpeedTextBlock = typingSpeedTextBlock;
            TypingTimeTextBlock = typingTimeTextBlock;
            PassPercentageTextBlock = passPercentageTextBlock;
            Keyboard = keyboard;
            TextView = textView;
        }
    }
}
