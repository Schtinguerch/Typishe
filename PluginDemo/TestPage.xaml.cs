using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TypisheApi;
using TypisheApi.Input;
using Localization = TypisheApi.Localizations.Resources;

namespace PluginDemo
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page, IGuiControl
    {
        public TestPage()
        {
            InitializeComponent();
        }

        public GuiControlPlace GetControlPlace()
        {
            return GuiControlPlace.NewWindow;
        }

        public UIElement GetUserControl()
        {
            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationCommander.ExecuteCommand(Localization.ReloadExercise);
        }
    }
}
