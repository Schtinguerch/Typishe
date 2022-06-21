using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using System.Windows.Media.Animation;

using Typishe.Input;
using Typishe.Visuality;
using Typishe.Exercises;

using Typishe.Input;
using Typishe.Service;
using Commands = TypisheApi.Localizations.Commands;

using Typishe.Input.ExerciseTextViews;
using Typishe.Pages.TypingPageSubpages;

namespace Typishe.Pages
{
    /// <summary>
    /// Логика взаимодействия для TypingPage.xaml
    /// </summary>
    public partial class TypingPage : Page
    {
        private bool _openLeftMenu = false;

        public TypingPage()
        {
            InitializeComponent();
            /* var exData = new ExerciseData()
            {
                Name = "Combined sections",
                Description = "Texts and multimedia",
                Author = "Schtinguerch",

                Sections = new List<ExerciseSection>()
                {
                    new TextExerciseSection()
                    {
                        TextToInput = "Let's gO to the river and to the battle for Britain.",
                        TimeToPass = new TimeSpan(0, 0, 36),
                        ProgressWeight = 100000,
                    },
                    new TextExerciseSection()
                    {
                        TextToInput = "Oh yeah, no timers, no speed",
                        TimeToPass = new TimeSpan(0),
                        ProgressWeight = 100000,
                    },
                    new MultimediaExerciseSection()
                    {
                        MultimediaFilenames = new Dictionary<string, string>()
                        {
                            { "en-US", @"C:\Users\2sust\Downloads\Ummy Downloads\1.mp4" }
                        }
                    },
                }
            };

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var json = JsonConvert.SerializeObject(exData, typeof(ExerciseData), Formatting.Indented, settings);
            File.WriteAllText(@"D:\Cases\ForMe\TestExercise\ExerciseData.json", json); */

            var qwertyLayout = JsonConvert.DeserializeObject<KeyboardLayout>(File.ReadAllText(@"C:\Users\2sust\source\repos\KeyboardTrainer\WPFMeteroWindow\bin\Debug\KeyboardLayouts\JSON LAYOUTS\QWERTY US.json"));
            var schtDvorakLayout = JsonConvert.DeserializeObject<KeyboardLayout>(File.ReadAllText(@"C:\Users\2sust\source\repos\KeyboardTrainer\WPFMeteroWindow\bin\Debug\KeyboardLayouts\JSON LAYOUTS\SchtDvorak.json"));

            var parallaxEffect = new ParallaxEffect(BackgroundGrid);
            var layout = new StandardKeyboard(KeyboardGrid, schtDvorakLayout);
            var textView = new SingleLineTextView();

            KeyboardLayoutConverter.InstalledLayout = qwertyLayout;
            KeyboardLayoutConverter.EmulatedLayout = schtDvorakLayout;

            var controls = new ExerciseControls(ExerciseNameTextBlock, TypingSpeedTextBlock, TypingTimeTextBlock, PassPercentageTextBlock, layout, textView);
            var exerciseView = new ExerciseView(controls);
            var keyPressProcessor = new KeyPressProcessor(this);

            ExerciseAreaGrid.Children.Add(exerciseView);
            ApplicationCommander.ExerciseView = exerciseView;

            exerciseView.Exercise.Load(@"D:\Cases\ForMe\TestExercise");
        }

        private void ReloadExerciseButton_Click(object sender, RoutedEventArgs e) => ApplicationCommander.ExecuteCommand(Commands.ReloadExercise);
        private void PreviousExerciseButton_Click(object sender, RoutedEventArgs e) => ApplicationCommander.ExecuteCommand(Commands.LoadPreviousExercise);
        private void NextExerciseButton_Click(object sender, RoutedEventArgs e) => ApplicationCommander.ExecuteCommand(Commands.LoadNextExercise);
        private void RandomExerciseButton_Click(object sender, RoutedEventArgs e) => ApplicationCommander.ExecuteCommand(Commands.LoadRandomExercise);

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _openLeftMenu = !_openLeftMenu;

            if (_openLeftMenu)
            {
                OpenSettingsPage();
            }

            else
            {
                CloseSettingsPage();
            }
        }

        public void OpenSettingsPage()
        {
            var page = new SettingsPage();
            LeftMenuFrame.Navigate(page);

            var storyboard = FindResource("ShowLeftFrameStoryboard") as Storyboard;
            storyboard.Begin();
        }

        public void CloseSettingsPage()
        {
            LeftMenuFrame.Content = null;
            while (LeftMenuFrame.NavigationService.RemoveBackEntry() != null) ;

            var storyboard = FindResource("HideLeftFrameStoryboard") as Storyboard;
            storyboard.Begin();
        }
    }
}
