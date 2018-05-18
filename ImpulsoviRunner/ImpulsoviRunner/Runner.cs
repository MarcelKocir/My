using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpulsoviRunner
{
    public class Runner
    {
        public int Identification { get; set; }

        public int MinSeconds { get; set; }

        public int MaxSeconds { get; set; }

        private TimeSpan? _TimeInterval;
        public TimeSpan TimeInterval
        {
            get
            {
                if (!_TimeInterval.HasValue)
                {
                    _TimeInterval = GenerateTimeInterval();
                }

                return _TimeInterval.Value;
            }
        }

        private TimeSpan GenerateTimeInterval()
        {
            return new TimeSpan(
                0,
                0,
                0,
                new Random(Guid.NewGuid().GetHashCode()).Next(MinSeconds, MaxSeconds));
        }
    }
}
