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

namespace Typishe.Exercises.SectionViews
{
    /// <summary>
    /// Логика взаимодействия для ExercisePreView.xaml
    /// </summary>
    public partial class ExercisePreView : Page
    {
        public delegate void ExerciseStartedHandler();
        public event ExerciseStartedHandler ExerciseStarted;

        public ExercisePreView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExerciseStarted?.Invoke();
        }
    }
}
