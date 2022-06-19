using System.Collections.Generic;

namespace TypisheApi
{
    public interface IClassHeader
    {
        List<IGuiControl> GuiControls { get; set; }
        List<IStartable> StartableClasses { get; set; }
    }
}
