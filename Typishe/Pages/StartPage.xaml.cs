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
using System.Windows.Media.Animation;

using Typishe.Visuality;
using Typishe.Input.Keyboards.XamlPatterns;
using Localization = TypisheApi.Localizations.Resources;
using Typishe.Setup;

namespace Typishe.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            CenterNode.StartPage = this;
        }

        private async Task AddKeysByHelloMessage(string message)
        {
            foreach (var character in message)
            {
                var key = new KeyboardKey(character.ToString().ToUpper(), "", "", "")
                {
                    Width = 60d,
                    Height = 60d,
                    Margin = new Thickness(4d),
                    Visibility = Visibility.Hidden,
                };

                KeysStackPanel.Children.Add(key);
            }

            foreach (var item in KeysStackPanel.Children)
            {
                var key = item as KeyboardKey;
                key.Visibility = Visibility.Visible;

                key.EndHighlightAnimation();
                await Task.Delay(200);
            }

            var storyboard = FindResource("HideKeysStoryboard") as Storyboard;
            storyboard.Begin();
        }

        private void StartTypingButton_Click(object sender, RoutedEventArgs e)
        {
            if (CenterNode.TypingPage == null)
            {
                CenterNode.AppWindow.OpenPageWithAnimation(new TypingPage());
            }

            else
            {
                CenterNode.AppWindow.OpenPageWithAnimation(CenterNode.TypingPage);
            }
        }

        private void PlayGamesButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: typing games (after 1000 years, of course)
        }

        private void AboutTypisheButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: create the page "About"
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Page_Loaded;
            Loaded += Page_LoadedNew;

            AddKeysByHelloMessage(Localization.Hello);

            var storyboard = FindResource("AppStartStoryboard") as Storyboard;
            storyboard.Begin();
        }

        private void Page_LoadedNew(object sender, RoutedEventArgs e)
        {
            var storyboard = FindResource("LogoAnimationStoryboard") as Storyboard;
            storyboard.Begin();
        }
    }
}
