using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TypisheApi.Setup
{
    public partial class FunctionalSetup
    {
        public string ApplicationLanguageCode { get; set; }

        public Dictionary<List<Key>, string> ShortCuts { get; set; }
        public Dictionary<string, string> ExerciseTextReplacements { get; set; }
        public List<string> ActiveKeyboardLayouts { get; set; }
        public List<string> OpenedKeyboardLayoutsHistory { get; set; }

        public bool ShowHands { get; set; }
        public bool ShowKeyboard { get; set; }
        public bool RequireCorrectInputMistakes { get; set; }

        public Keyboards SelectedKeyboardType { get; set; }
        public HandsUsing HandsUsing { get; set; }

        public bool EnableLayoutEmulation { get; set; }
        public KeyValuePair<string, string> EmulationPair { get; set; }

        public TypingSpeedMeasure TypingSpeedMeasure { get; set; }
        public NumberDisplay NumberDisplay { get; set; }
    }
}
