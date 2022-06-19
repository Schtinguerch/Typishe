using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypisheApi;

namespace PluginDemo
{
    public class ClassHeader : IClassHeader
    {
        public List<IGuiControl> GuiControls { get; set; } = new List<IGuiControl>() { new TestPage() };

        public List<IStartable> StartableClasses { get; set; } = new List<IStartable>();
    }
}
