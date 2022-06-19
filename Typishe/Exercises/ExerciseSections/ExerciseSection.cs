using System;

namespace Typishe.Exercises
{
    public abstract class ExerciseSection
    {
        public string Name { get; set; }
        public int ProgressWeight { get; set; }
        public int EarnedScore { get; set; }
    }
}
