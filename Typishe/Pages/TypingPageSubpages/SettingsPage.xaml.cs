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

namespace Typishe.Pages.TypingPageSubpages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>

    public partial class SettingsPage : Page, IRequestable
    {
        private IRequestable[] _requestablePages = new IRequestable[3];
        private int _selectedIndex = 0;

        public SettingsPage(SubPage selectedSubPage = SubPage.VisualSetupSettings)
        {
            InitializeComponent();
            OpenPage(selectedSubPage);
        }

        public void OpenPage(SubPage selectedSubPage)
        {
            switch (selectedSubPage)
            {
                case SubPage.VisualSetupSettings:
                    _selectedIndex = 0;
                    if (_requestablePages[0] == null)
                    {
                        _requestablePages[0] = new VisualSetupPage();
                    }

                    SubpageFrame.Navigate(_requestablePages[0]);
                    break;

                case SubPage.FunctionalSetupSettings:
                    _selectedIndex = 1;
                    //TODO: create the functional setup page
                    break;

                case SubPage.ExerciseSetupSettings:
                    _selectedIndex = 2;
                    //TODO: create the exercise setup page
                    break;
            }
        }

        public void ApplySettings()
        {
            _requestablePages[_selectedIndex]?.ApplySettings();
        }

        public void DiscardSettings()
        {
            _requestablePages[_selectedIndex]?.DiscardSettings();
        }

        public void NewFile()
        {
            _requestablePages[_selectedIndex]?.NewFile();
        }

        public void OpenFile()
        {
            _requestablePages[_selectedIndex]?.OpenFile();
        }

        public void SaveFile()
        {
            _requestablePages[_selectedIndex]?.SaveFile();
        }

        public void SaveFileAs()
        {
            _requestablePages[_selectedIndex]?.SaveFileAs();
        }

        private void OpenVisualSetupButton_Click(object sender, RoutedEventArgs e) => OpenPage(SubPage.VisualSetupSettings);
        private void OpenFunctionalSetupButton_Click(object sender, RoutedEventArgs e) => OpenPage(SubPage.FunctionalSetupSettings);
        private void OpenExerciseSetupButton_Click(object sender, RoutedEventArgs e) => OpenPage(SubPage.ExerciseSetupSettings);

        private void NewFileButton_Click(object sender, RoutedEventArgs e) => NewFile();
        private void OpenFileButton_Click(object sender, RoutedEventArgs e) => OpenFile();
        private void SaveFileButton_Click(object sender, RoutedEventArgs e) => SaveFile();
        private void SaveAsFileButton_Click(object sender, RoutedEventArgs e) => SaveFileAs();

        private void ApplyButton_Click(object sender, RoutedEventArgs e) => ApplySettings();
        private void DiscardButton_Click(object sender, RoutedEventArgs e) => DiscardSettings();
    }
}
