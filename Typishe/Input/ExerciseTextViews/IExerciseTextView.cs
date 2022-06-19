using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Input
{
    public interface IExerciseTextView
    {
        void UpdateTextOutput(string enteredText, string leftText);
        void UpdateTextOutput(string mistakenText);
    }
}
