using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Exercises
{
    public class ExerciseStatistics
    {
        public DateTime PassStartDateTime { get; set; }
        public DateTime PassEndDateTime { get; set; }

        public List<SectionStatistics> SectionStatistics { get; set; } = new List<SectionStatistics>();
    }
}
