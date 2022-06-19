using System.Collections.Generic;

namespace TypisheApi
{
    public class ClassHeader : IClassHeader
    {
        public List<IGuiControl> GuiControls { get; set; } = new List<IGuiControl>();
        public List<IStartable> StartableClasses { get; set; } = new List<IStartable>();
    }
}
