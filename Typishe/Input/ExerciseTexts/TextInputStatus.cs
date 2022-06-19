using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Typishe.Setup;

namespace Typishe.Input
{
    public class TextInputStatus
    {
        public delegate void TextEnteredHandler(string enteredText, string leftText);
        public delegate void MistakeEnteredHandler(string mistakenText);
        public delegate void TextEnterFinishedHandler();

        public event TextEnteredHandler TextEntered;
        public event MistakeEnteredHandler MistakeEntered;
        public event TextEnterFinishedHandler TextEnterFinished;

        public string ExerciseText { get; private set; }
        public string MistakenText { get; private set; }
        public int CharartersEntered { get; private set; }

        public string EnteredText => ExerciseText.Substring(0, CharartersEntered);
        public string LeftText => ExerciseText.Substring(CharartersEntered, ExerciseText.Length - CharartersEntered);

        public void SetNewText(string text)
        {
            ExerciseText = text;
            ClearMistakes();

            CharartersEntered = 0;
            TextEntered?.Invoke(EnteredText, LeftText);
        }

        public bool TryToEnterNext(string character)
        {
            if (character == ExerciseText[CharartersEntered].ToString() && HasAbilityToContinueInput())
            {
                EnterNext();
                return true;
            }

            EnterMistake(character);
            return false;
        }
        
        public void EnterNext()
        {
            CharartersEntered += 1;
            
            if (MistakenText != "")
            {
                ClearMistakes();
                MistakeEntered?.Invoke(MistakenText);
            }

            TextEntered?.Invoke(EnteredText, LeftText);

            if (CharartersEntered == ExerciseText.Length)
            {
                TextEnterFinished?.Invoke();
                return;
            }
        }

        public void EnterMistake(string mistake)
        {
            MistakenText += mistake;
            MistakeEntered?.Invoke(MistakenText);
        }

        public void ClearMistakes()
        {
            MistakenText = "";
            MistakeEntered?.Invoke(MistakenText);
        }

        public void CorrectOneMistakeCharacter()
        {
            if (MistakenText.Length == 0)
            {
                return;
            }

            MistakenText = MistakenText.Substring(0, MistakenText.Length - 1);
            MistakeEntered?.Invoke(MistakenText);
        }

        public void DeleteLastMistakenWord()
        {
            int lastSpaceIndex = -1;
            var wasSpaces = false;

            for (int i = MistakenText.Length - 1; i >= 0; i--)
            {
                if (MistakenText[i] == ' ')
                {
                    if (!wasSpaces)
                    {
                        continue;
                    }

                    lastSpaceIndex = i;
                    break;
                }

                if (MistakenText[i] != ' ')
                {
                    wasSpaces = true;
                    continue;
                }
            }

            MistakenText = MistakenText.Substring(0, lastSpaceIndex + 1);
            MistakeEntered?.Invoke(MistakenText);
        }

        private bool HasAbilityToContinueInput()
        {
            if (ApplicationSetup.FunctionalSetup.RequireCorrectInputMistakes)
            {
                return MistakenText.Length == 0;
            }

            return true;
        }
    }
}
