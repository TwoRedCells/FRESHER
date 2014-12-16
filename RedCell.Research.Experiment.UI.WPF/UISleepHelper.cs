using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace RedCell.Research.Experiment.UI
{
    internal static class UISleepHelper
    {
        private static readonly List<DispatcherTimer> Timers = new List<DispatcherTimer>();

        public static void Do(Action action)
        {
            // If there is a timer running, add this task to its completion queue.
            if (Timers.Count != 0 && Timers.First().IsEnabled)
            {
                var timer = Timers.Last();
                var events = timer.Tag as List<Action>;
                events.Add(action);
            }

            // Otherwise just run it.
            else
            {
                action();
            }
        }

        public static void Wait(double seconds)
        {
            // Replace the existing timer with a new one.
            var timer = new DispatcherTimer {Tag = new List<Action>()};
            timer.Interval = TimeSpan.FromSeconds(seconds);

            // Once this timer elapses, fire all of its events and get rid of it.
            timer.Tick += (sender, e) =>
            {
                var u = sender as DispatcherTimer;
                u.Stop();
                var events = u.Tag as List<Action>;
                foreach (Action a in events)
                    a();
                Timers.Remove(u);
            };
            // But don't start it until the old one is done.
            Do(timer.Start);
            Timers.Add(timer);
        }
    }
}
