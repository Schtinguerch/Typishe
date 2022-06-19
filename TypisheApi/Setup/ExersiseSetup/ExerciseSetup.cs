using System.Collections.Generic;

namespace TypisheApi.Setup
{
    public class ExerciseSetup
    {
        public List<string> OpenedSingleLessonsHistory { get; set; }
        public List<string> OpenedCoursesHistory { get; set; }

        public bool IsCourseOpened { get; set; }
    }
}
