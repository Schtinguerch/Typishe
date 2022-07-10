using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Exercises
{
    public class SectionStatistics
    {
        public TimeSpan PassTime { get; set; }
    }

    public abstract class ScoredSectionStatistics : SectionStatistics
    {
        public double EarnedPoints { get; set; }
    }

    public class QuestionSectionStatistics : ScoredSectionStatistics
    {
        public string SelectedAnswer { get; set; }
        public string RightAnswer { get; set; }
    }

    public class TextInputSectionStatistics : ScoredSectionStatistics
    {
        public double AverageTypingSpeedCpm { get; set; }
        public Dictionary<int, string> MistakenSections { get; set; } = new Dictionary<int, string>();

        public List<double> SecondaryAverageCpm { get; set; } = new List<double>();
        public List<double> SecondaryInstCpm { get; set; } = new List<double>();
    }
}
