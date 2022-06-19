using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Exercises
{
    public class CourseData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public List<CourseSection> Sections { get; set; }
    }
}
