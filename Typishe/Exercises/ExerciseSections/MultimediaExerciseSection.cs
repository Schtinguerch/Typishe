using System;
using System.Collections.Generic;
using System.Linq;

using Typishe.Setup;

namespace Typishe.Exercises
{
    public class MultimediaExerciseSection : ExerciseSection
    {
        public delegate void SectionFinishedHandler();
        public event SectionFinishedHandler SectionFinished;

        public Uri MultimediaUri
        {
            get
            {
                var found = MultimediaFilenames.TryGetValue(ApplicationSetup.FunctionalSetup.ApplicationLanguageCode, out string localizedFilename);

                if (!found && MultimediaFilenames.Count != 0)
                {
                    localizedFilename = MultimediaFilenames.First().Value;
                }

                return new Uri(localizedFilename);
            }
        }

        public Dictionary<string, string> MultimediaFilenames { get; set; }

        public void FinishSection()
        {
            SectionFinished?.Invoke();
        }
    }
}
