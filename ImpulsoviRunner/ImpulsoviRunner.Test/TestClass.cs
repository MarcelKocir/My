using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WakeUpSleepScheduler;

namespace ImpulsoviRunner.Test
{
    [TestFixture]
    public class TestClass
    {       
        [Test]
        public void RunRunnerTest()
        {
            RunRunner();

            // TODO: Add your test code here
            Assert.IsTrue(true, "error");
        }


        [Test]
        public void WakeUpSleepTest()
        {
            RunRunner();
            InitWakUpSleep();

            Thread.Sleep(Timeout.Infinite);

            // TODO: Add your test code here
            Assert.IsTrue(true, "error");
        }

        public void InitWakUpSleep()
        {
            WakeUp.SetWakeAtByConfig(
            () =>
            {
                InitWakUpSleep();
            });
            Sleeper.WaitForSleepByConfig();
        }

        public async void RunRunner()
        {
            await Task.Run(() =>
                {
                    var inicializer = new RunnerInitializer();
                    //inicializer.Run((runner) => Debug.WriteLine($"Runner:{runner.Identification} ; Elapsed time: {runner.TimeInterval} ; Actual time: {DateTime.Now}"));

                    inicializer.Run((runner) =>
                    {                        
                        LogToFile($"Runner:{runner.Identification} ; Elapsed time: {runner.TimeInterval} ; Actual time: {DateTime.Now}");
                    });
                });
        }

        private void LogToFile(string text)
        {
            string logFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log.txt");
            if (!File.Exists(logFileName))
            {
                using (File.Create(logFileName))
                {
                    
                }
            }
            using (var sw = File.AppendText(logFileName))
            {
                sw.Write(text);
                sw.Write(sw.NewLine);
            }
        }

    }
}
