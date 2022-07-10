using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

using Typishe.Exercises;
using Typishe.Pages;
using Typishe.Pages.TypingPageSubpages;
using Typishe.Setup;

using Commands = TypisheApi.Localizations.Commands;

namespace Typishe.Input
{
    public static class ApplicationCommander
    {
        private static int _initializeApi = new Func<int>(() =>
        {
            TypisheApi.Input.ApplicationCommander.OnCommandExecution += OnCommandExecution;
            return 0; 
        })();

        private static Dictionary<string, Action> _commands = new Dictionary<string, Action>() 
        { 
            { Commands.ReloadExercise, ReloadExercise },
            { Commands.LoadPreviousExercise, LoadPreviousExercise },
            { Commands.LoadNextExercise, LoadNextExercise },
            { Commands.LoadRandomExercise, LoadRandomExercise },
            { Commands.OpenSettings, OpenSettings },
            { Commands.CloseSettings, CloseSettings },
            { Commands.BackToStartPage, BackToStartPage },
        };

        public static ExerciseView ExerciseView { get; set; }
        public static TypingPage TypingPage { get; set; }
        public static SettingsPage SettingsPage { get; set; }

        public static bool ExecuteCommand(string commandName)
        {
            var success = _commands.TryGetValue(commandName, out var action);
            if (!success)
            {
                return false;
            }

            try
            {
                action();
                return true;
            }

            catch
            {
                return false;
            }
        }

        private static void OnCommandExecution(string command, ref bool executed)
        {
            executed = ExecuteCommand(command);
        }

        private static void ReloadExercise()
        {
            ExerciseView.Exercise.Reload();
        }

        private static void LoadNextExercise()
        {
            ExerciseView.Course.LoadNextExercsise();
        }

        private static void LoadPreviousExercise()
        {
            ExerciseView.Course.LoadPreviousExercsise();
        }

        private static void LoadRandomExercise()
        {
            ExerciseView.Course.LoadRandomExercise();
        }

        private static void OpenSettings()
        {
            TypingPage.OpenSettingsPage();
        }

        private static void CloseSettings()
        {
            TypingPage.CloseSettingsPage();
        }

        private static void BackToStartPage()
        {
            CenterNode.AppWindow.OpenPageWithAnimation(CenterNode.StartPage);
        }
    }
}
