using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Pages.TypingPageSubpages
{
    public interface IRequestable
    {
        void ApplySettings();
        void DiscardSettings();

        void NewFile();
        void OpenFile();
        void SaveFile();
        void SaveFileAs();
    }
}
