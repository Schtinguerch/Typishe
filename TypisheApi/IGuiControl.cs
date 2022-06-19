using System.Windows;

namespace TypisheApi
{
    public enum GuiControlPlace
    {
        StartMenu,
        Settings,
        WidgetPanel,
        TextView,
        NewWindow,
    }

    public interface IGuiControl
    {
        UIElement GetUserControl();
        public GuiControlPlace GetControlPlace();
    }
}
