using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

using Typishe.Service;
using Typishe.Setup;

namespace Typishe.Exercises
{
    public class Exercise
    {
        public delegate void ExerciseFinishedHandler();
        public event ExerciseFinishedHandler ExerciseFinished;

        public delegate void SectionStartedHandler(ExerciseSection section);
        public event SectionStartedHandler SectionStarted;

        public ExerciseView ExerciseView { get; set; }
        private ExerciseData _exerciseData;

        private bool _isLoadingSuccessful = false;
        private string _sourceFolder;
        public string SourceFolder => _sourceFolder;

        private int _selectedSectionIndex = 0;
        

        public void Load(string folderPath)
        {
            _isLoadingSuccessful = false;

            if (!Directory.Exists(folderPath))
            {
                Logger.Add($"Exercise loading: folder searching \"{folderPath}\"", "FAILED");

                _isLoadingSuccessful = false;
                return;
            }

            var jsonFilename = Path.Combine(folderPath, "ExerciseData.json");
            if (!File.Exists(jsonFilename))
            {
                Logger.Add($"Exercise loading: file searching \"{jsonFilename}\"", "FAILED");

                _isLoadingSuccessful = false;
                return;
            }

            var code = File.ReadAllText(jsonFilename).Replace("@folder", folderPath);
            try
            {
                _exerciseData = JsonConvert.DeserializeObject<ExerciseData>(code, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                Logger.Add("Exercise loading", "SUCCESS");
            }

            catch (Exception ex)
            {
                Logger.Add($"JSON parcing \"{jsonFilename}\", {ex.Message}", "FAILED");
                return;
            }

            _isLoadingSuccessful = true;
            Reload();
        }

        public void Reload()
        {
            if (!_isLoadingSuccessful)
            {
                CenterNode.AppWindow.PopupMessageWindow.ShowMessage("Error");
                return;
            }

            ExerciseView.StatisticsWorker.ResetOutput();
            _selectedSectionIndex = -1;

            ExerciseView.ExerciseControls.ExerciseNameTextBlock.Text = _exerciseData.Name;
            ExerciseView.LoadPreviewPage();
            ExerciseView.StatisticsWorker.TotalProgressWeight = _exerciseData.Sections.Sum(s => s.ProgressWeight);
        }

        public void LoadNextSection()
        {
            _selectedSectionIndex += 1;

            if (_selectedSectionIndex > _exerciseData.Sections.Count)
            {
                return;
            }
            
            if (_selectedSectionIndex == _exerciseData.Sections.Count)
            {
                ExerciseFinished?.Invoke();
                return;
            }

            var section = _exerciseData.Sections[_selectedSectionIndex];

            ExerciseView.OpenSection(section);
            SectionStarted?.Invoke(section);
        }

        public Exercise(ExerciseView exerciseView)
        {
            ExerciseView = exerciseView;
        }
    }
}
