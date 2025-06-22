using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4
{
    class MainClass
    {
        static ManualResetEvent mrEvent = new ManualResetEvent(false);
        static AutoResetEvent arEvent = new AutoResetEvent(false);
        static Stopwatch sw = new Stopwatch();
        public static void Main(string[] args)
        {
            sw.Start();
            Method1("A");
            Method1("B");
            while (true)
            {
                Thread.Sleep(5000);
                mrEvent.Set();
            }
            
        }

        public static async Task Method1(string name)
        {
          await new TaskFactory().StartNew(()=> {
                while (true)
                {
                    var status = mrEvent.WaitOne(2000);
                    Console.WriteLine($"{status}-Method-{name}:{sw.Elapsed.Seconds}");
                    mrEvent.Reset();
                }
            });
        }
    }
}
