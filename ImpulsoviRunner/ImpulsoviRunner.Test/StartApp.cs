using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpulsoviRunner.Test
{
    public class StartApp
    {
        //public static void Main()
        //{
        //    new TestClass().RunRunner();
        //    Console.ReadLine();
        //}

        public static void Main()
        {
            var testClass = new TestClass();
            testClass.RunRunner();
            testClass.InitWakUpSleep();
            Console.ReadLine();
        }
    }
}
