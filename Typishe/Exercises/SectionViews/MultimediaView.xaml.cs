using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Typishe.Exercises.SectionViews
{
    /// <summary>
    /// Логика взаимодействия для MultimediaView.xaml
    /// </summary>
    public partial class MultimediaView : Page
    {
        private MultimediaExerciseSection _section;

        public MultimediaView(MultimediaExerciseSection section)
        {
            InitializeComponent();
            _section = section;

            AudioVideoView.Source = section.MultimediaUri;
            AudioVideoView.Play();

            AudioVideoView.MediaEnded += (s, e) => _section.FinishSection();
        }

        private void SkipButtonClick(object sender, RoutedEventArgs e)
        {
            AudioVideoView.Stop();
            _section.FinishSection();
        }
    }
}
