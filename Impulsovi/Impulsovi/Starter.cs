using ImpulsoviRunner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using WakeUpSleepScheduler;

namespace Impulsovi
{
    public class Starter
    {
        //private Timer _Timer;

        private int _Counter = 0;

        private Func<string, string, Tuple<string, string>> _NavigateMethod;

        private Func<string> _CaptureImageMethod;

        private System.Windows.Forms.WebBrowser _SyncControl;

        private string _CurrentDirectory;

        private List<Repeater> _RepeaterList = new List<Repeater>();

        public void Start(Func<string, string, Tuple<string, string>> p_NavigateMethod, Func<string> p_CaptureImageMethod,
            System.Windows.Forms.WebBrowser p_SynControl)
        {
            _NavigateMethod = p_NavigateMethod;
            _CaptureImageMethod = p_CaptureImageMethod;
            _SyncControl = p_SynControl;
            _CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Start();
        }

        //private void Start()
        //{
        //    //5 MINUT (MINIMUM PRO VKLADANI STEJNE REGISTRACE)
        //    int wait = (5 * 60 * 1000);
        //    _Timer = new Timer(wait);
        //    _Timer.SynchronizingObject = _SyncControl as ISynchronizeInvoke;
        //    _Timer.Elapsed += Timer_Elapsed;

        //    InitRepeaters();

        //    RunTimerRepeaters();

        //    _Timer.Start();
        //}

        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    _Timer.Stop();
        //    RunTimerRepeaters();            
        //    _Timer.Start();
        //}

        private void Start()
        {
            InitWakUpSleep();
            InitRepeaters();
            RunRunnerForever();                     
        }

        private void RunRunnerForever()
        {
            try
            {
                RunRunner();
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder(ex.Message);
                SaveToFile(sb, 1, true);
                RunRunnerForever();
            }
        }


        private void InitRepeaters()
        {
            for (int i = 1; i <= RunnerInitializer.GetFromConfig("NumberOfTasks", 1); i++)
            {
                _Counter = i;
                _RepeaterList.Add(GetRepeaterByCounter());
            }
        }

        public async Task RunRunner()
        {
            await Task.Run(() =>
            {
                var inicializer = new RunnerInitializer();

                inicializer.Run((runner) =>
                {
                    //LogToFile($"Runner:{runner.Identification} ; Elapsed time: {runner.TimeInterval} ; Actual time: {DateTime.Now}");
                    RunRepeater(runner.Identification);
                });
            });
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

        //private void RunTimerRepeaters()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int errCounter = 0;
        //    foreach (Repeater repeater in _RepeaterList)
        //    {
        //        bool registred = false;
        //        do
        //        {
        //            registred = repeater.DoRegistration(sb, ref errCounter); ;
        //        }
        //        while (!registred);
        //    }
        //    SaveToFile(sb, errCounter);
        //}

        private void RunRepeater(int identification)
        {
            StringBuilder sb = new StringBuilder();
            int errCounter = 0;
            var repeater = _RepeaterList[identification];
            bool registred = false;
            do
            {
                registred = repeater.DoRegistration(sb, ref errCounter); ;
            }
            while (!registred);
            SaveToFile(sb, errCounter);
        }

        private Repeater GetRepeaterByCounter()
        {
            return new Repeater(ParamPatternsBI.FullParamsPatternList[_Counter - 1], _NavigateMethod, _CaptureImageMethod, _SyncControl);
        }

        private void SaveToFile(StringBuilder p_StringBuilder, int p_ErrCounter, bool p_Eexception = false)
        {
            string exceptionPrefix =  p_Eexception ? "EXCEPTION - " : string.Empty;
            string fileName = $"{exceptionPrefix}{DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss")}.log";
            if(p_ErrCounter > 0)
            {
                fileName.Insert(0, $"{p_ErrCounter} ERRORS - ");
            }
            using (System.IO.TextWriter w = new System.IO.StreamWriter(@Path.Combine(_CurrentDirectory, Directory.CreateDirectory("Log").Name, fileName)))
            {
                w.Write(p_StringBuilder.ToString());
            }
        }

        /// <summary>
        /// Jen pro testovani
        /// </summary>
        /// <param name="text"></param>
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
