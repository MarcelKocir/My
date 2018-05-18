using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormBrowser form = new FormBrowser();
            form.WindowState = FormWindowState.Maximized;

            //Thread t = new Thread(() => 
            //{
            //    Application.Run(form);
            //});
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();

            Application.Run(form);
        }
    }
}
