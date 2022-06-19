using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Typishe.Exercises.SectionViews
{
    /// <summary>
    /// Логика взаимодействия для CountDownView.xaml
    /// </summary>
    public partial class CountDownView : Page
    {
        public delegate void CountdownFinishedHandler();
        public event CountdownFinishedHandler CountdownFinished;

        private Storyboard _splashStoryboard;
        private int _seconds = 3;

        public CountDownView()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += Timer_Tick;
            Loaded += (s, e) =>
            {
                timer.Start();
                SecondsTextBlock.Text = _seconds.ToString();
                _splashStoryboard.Begin();
            };

            InitializeComponent();
            _splashStoryboard = FindResource("CircleSplashStoryboard") as Storyboard;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _splashStoryboard.Begin();
            SecondsTextBlock.Text = (--_seconds).ToString();

            if (_seconds == 0)
            {
                (sender as DispatcherTimer).Stop();
                CountdownFinished?.Invoke();
            }
        }
    }
}
