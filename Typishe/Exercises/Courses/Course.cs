using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Typishe.Service;
using Typishe.Setup;

namespace Typishe.Exercises
{
    public class Course
    {
        public ExerciseView ExerciseView { get; set; }
        private CourseData _courseData;

        private int _currentSection, _currentExercise;
        private readonly Random _random;

        private bool _isLoadingSuccessful = false;
        private string _sourceFolder;
        public string SourceFolder => _sourceFolder;

        public void Load(string folderPath)
        {
            _isLoadingSuccessful = false;
            _currentSection = _currentExercise = 0;

            if (!Directory.Exists(folderPath))
            {
                Logger.Add($"Course loading: folder searching \"{folderPath}\"", "FAILED");

                _isLoadingSuccessful = false;
                return;
            }

            var jsonFilename = Path.Combine(folderPath, "CourseData.json");
            if (!File.Exists(jsonFilename))
            {
                Logger.Add($"Course loading: file searching \"{jsonFilename}\"", "FAILED");

                _isLoadingSuccessful = false;
                return;
            }

            var code = File.ReadAllText(jsonFilename).Replace("@folder", folderPath);
            try
            {
                _courseData = JsonConvert.DeserializeObject<CourseData>(code, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                Logger.Add("Course loading", "SUCCESS");
            }

            catch (Exception ex)
            {
                Logger.Add($"JSON parcing \"{jsonFilename}\", {ex.Message}", "FAILED");
                return;
            }

            _isLoadingSuccessful = true;
            ReloadExercise();
        }

        public void ReloadExercise()
        {
            if (!_isLoadingSuccessful)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error");
                return;
            }

            ExerciseView.Exercise.Load(_courseData.Sections[_currentSection].Exercises[_currentExercise]);
        }

        public void LoadPreviousExercsise()
        {
            if (!_isLoadingSuccessful)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error");
                return;
            }


            int previousExercise = _currentExercise - 1;
            int previousSection = _currentSection;

            if (previousExercise < 0)
            {
                previousSection -= 1;
            }

            if (previousSection < 0)
            {
                //TODO: show popup message
                return;
            }

            previousExercise = _courseData.Sections[previousSection].Exercises.Count - 1;
            LoadAt(previousSection, previousExercise);
        }

        public void LoadNextExercsise()
        {
            if (!_isLoadingSuccessful)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error");
                return;
            }

            int nextExercise = _currentExercise + 1;
            int nextSection = _currentSection;

            if (nextExercise >= _courseData.Sections[_currentSection].Exercises.Count)
            {
                nextSection += 1;
                nextExercise = 0;
            }

            LoadAt(nextSection, nextExercise);
        }

        public void LoadAt(int sectionIndex, int exerciseIndex)
        {
            if (!_isLoadingSuccessful)
            {
                //TODO: show popup message
                return;
            }

            if (sectionIndex < 0 || sectionIndex >= _courseData.Sections.Count)
            {
                //TODO: show popup message
                return;
            }

            if (exerciseIndex < 0 || exerciseIndex >= _courseData.Sections[sectionIndex].Exercises.Count)
            {
                //TODO: show popup message
                return;
            }

            _currentSection = sectionIndex;
            _currentExercise = exerciseIndex;

            ReloadExercise();
        }

        public void LoadRandomExercise()
        {
            if (!_isLoadingSuccessful)
            {
                //TODO: show popup message
                return;
            }

            int section = _random.Next(0, _courseData.Sections.Count);
            int exercise = _random.Next(0, _courseData.Sections[section].Exercises.Count);

            LoadAt(section, exercise);
        }

        public Course(ExerciseView exerciseView)
        {
            ExerciseView = exerciseView;
        }
    }
}
