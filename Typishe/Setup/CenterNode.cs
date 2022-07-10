using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Typishe.Pages;
using TypisheApi;

namespace Typishe.Setup
{
    public static class CenterNode
    {
        private static MainWindow _appWindow;
        public static MainWindow AppWindow
        {
            get => _appWindow;
            set
            {
                _appWindow = value;
                var header = Plugins.GetItems(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "EnabledPlugins"));

                foreach (var guiControl in header.GuiControls)
                {
                    if (guiControl.GetControlPlace() == GuiControlPlace.NewWindow)
                    {
                        var window = new TestingWindow();
                        window.Show();

                        window.Frame.Navigate(guiControl.GetUserControl());
                    }
                }
            }
        }

        public static TypingPage TypingPage { get; set; }
        public static StartPage StartPage { get; set; }
    }
}
