using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WakeUpSleepScheduler
{
    public class Sleeper
    {
        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public static bool Hibernate()
        {
            return SetSuspendState(true, false, false);
        }

        public static bool Sleep()
        {
            return SetSuspendState(false, false, false);
        }


        public static DateTime SleepTime
        {
            get
            {
                return DateTime.ParseExact(ConfigurationManager.AppSettings.Get("SleepTime"), "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public static void WaitForSleepByConfig()
        {           
            WaitForSleep(SleepAt.Subtract(DateTime.Now));            
        }

        public static DateTime SleepAt
        {
            get
            {
                var result = SleepTime;
                if (result < DateTime.Now)
                {
                    result = result.AddDays(1);
                }

                return result;
            }
        }

        public static async Task WaitForSleep(TimeSpan delay)
        {
            await Task.Delay(delay);

            Sleep();
        }
    }
}
