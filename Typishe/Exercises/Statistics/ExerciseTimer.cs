using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;

namespace Typishe.Exercises
{
    public class ExerciseTimer
    {
        public delegate void TickHandler(TimeSpan timeElapsesd);
        public event TickHandler Tick;

        private readonly TimeSpan _interval;

        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;

        public ExerciseTimer()
        {
            _interval = TimeSpan.FromMilliseconds(25);
            _timer = new DispatcherTimer() { Interval = _interval };
            _timer.Tick += _timerTick;

            _stopwatch = new Stopwatch();
        }

        private void _timerTick(object? sender, EventArgs e)
        {
            Tick?.Invoke(_stopwatch.Elapsed);
        }

        public void StartOrContinue()
        {
            _timer.Start();
            _stopwatch.Start();
        }

        public void Pause()
        {
            _timer.Stop();
            _stopwatch.Stop();
        }

        public void StopAndReset()
        {
            _timer.Stop();
            _stopwatch.Reset();
        }
        
        public void Restart()
        {
            StopAndReset();
            StartOrContinue();
        }
    }
}
