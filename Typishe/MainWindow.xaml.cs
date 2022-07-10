using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

using Typishe.Visuality;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Page = System.Windows.Controls.Page;

namespace Typishe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Storyboard _curtainDownStoryboard, _curtainUpStoryboard;

        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

        public MainWindow()
        {
            Setup.CenterNode.AppWindow = this;
            InitializeComponent();
            var parallaxEffect = new ParallaxEffect(BackgroundGrid);

            _curtainDownStoryboard = FindResource("CurtainDownStoryboard") as Storyboard;
            _curtainUpStoryboard = FindResource("CurtainUpStoryboard") as Storyboard;

            _curtainDownStoryboard.Completed += _curtainDownStoryboard_Completed;
            _curtainUpStoryboard.Completed += _curtainUpStoryboard_Completed;
            
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }

        private void _curtainUpStoryboard_Completed(object? sender, EventArgs e)
        {
            CurtainRectangle.Visibility = Visibility.Collapsed;
        }

        private Page _openingPage;
        private void _curtainDownStoryboard_Completed(object? sender, EventArgs e)
        {
            MainWindowFrame.Navigate(_openingPage);
            _curtainUpStoryboard.Begin();
        }

        public void OpenPageWithAnimation(Page page)
        {
            CurtainRectangle.Visibility = Visibility.Visible;

            _openingPage = page;
            _curtainDownStoryboard.Begin();
        }
    }
}
