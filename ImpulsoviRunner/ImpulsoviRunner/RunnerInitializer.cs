using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WakeUpSleepScheduler;

namespace ImpulsoviRunner
{
    /// <summary>
    /// The runner.
    /// </summary>
    public class RunnerInitializer
    {
        private List<Runner> _Runners;

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="methodtoRun">
        /// The methodto run.
        /// </param>
        public void Run(Action<Runner> methodtoRun)
        {
            RunRunners(methodtoRun);
        }

        private void RunRunners(Action<Runner> methodtoRun)
        {
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                InitRunners();
                stopwatch.Reset();
                for (int i = 0; i < _Runners.Count; i++)
                {
                    Wait(_Runners[i], stopwatch);
                    if (RunnerEnabled())
                    {
                        methodtoRun(_Runners[i]);
                    }
                }
            }
        }

        private void InitRunners()
        {
            int numOfTasks = GetFromConfig("NumberOfTasks", 1);

            var runners = new List<Runner>(numOfTasks);

            for (int i = 0; i < numOfTasks; i++)
            {
                var runner = new Runner()
                {
                    Identification = i,
                    MinSeconds = GetMinSeconds(),
                    MaxSeconds = GetMaxSeconds()
                };
                runners.Add(runner);
            }
            _Runners = runners.OrderBy(item => item.TimeInterval).ToList();
        }

        public void Wait(Runner runner, Stopwatch stopwatch)
        {
            stopwatch.Start();
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds >= runner.TimeInterval.TotalMilliseconds)
                {
                    stopwatch.Stop();
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        private int GetMinSeconds()
        {
            return GetFromConfig("MinSeconds", 300);
        }

        private int GetMaxSeconds()
        {
            return GetFromConfig("MaxSeconds", 600);
        }

        /// <summary>
        /// The get from config.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetFromConfig(string key, int defaultValue)
        {
            int result = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings.Get(key), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        private bool RunnerEnabled()
        {
            bool result = true;
            var applyTimeToAction = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ApplyTimeToAction") ?? "false");            
            if (applyTimeToAction)
            {
                var now = DateTime.Now;
                if (!CheckTimeBetweenStartEnd(WakeUp.WakeUpTime.TimeOfDay, Sleeper.SleepTime.TimeOfDay, now.TimeOfDay))
                {
                    result = false;
                }
            }

            return result;
        }

        private bool CheckTimeBetweenStartEnd(TimeSpan start, TimeSpan end, TimeSpan checking)
        {
            // see if start comes before end
            if (start < end)
                return start <= checking && checking <= end;
            // start is after end, so do the inverse comparison
            return !(end < checking && checking < start);
        }
    }
}
