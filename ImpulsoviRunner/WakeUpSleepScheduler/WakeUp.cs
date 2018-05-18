using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeUpSleepScheduler
{
    public class WakeUp
    {
        public static DateTime WakeUpTime
        {
            get
            {
                return DateTime.ParseExact(ConfigurationManager.AppSettings.Get("WakeUpTime"), "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public static IntPtr SetWakeAtByConfig(Imports.WakeUpTimerCompleteDelegate wakeUpTimerComplete)
        {            
            return SetWakeAt(WakeUpAt, wakeUpTimerComplete);
        }

        public static DateTime WakeUpAt
        {
            get
            {
                var result = WakeUpTime;
                if (result < DateTime.Now)
                {
                    result = result.AddDays(1);
                }

                return result;
            }
        }

        private static IntPtr SetWakeAt(DateTime dt, Imports.WakeUpTimerCompleteDelegate wakeUpTimerComplete)
        {            
            long interval = dt.ToFileTime();

            return SetWakeAt(interval, wakeUpTimerComplete);
        }
       
        private static IntPtr SetWakeAt(long interval, Imports.WakeUpTimerCompleteDelegate wakeUpTimerComplete)
        {
            IntPtr handle = Imports.CreateWaitableTimer(IntPtr.Zero, true, "WaitableTimer");
            Imports.SetWaitableTimer(handle, ref interval, 0, wakeUpTimerComplete, IntPtr.Zero, true);

            return handle;
        }
    }
}
