using System;
using System.Threading;
using System.Collections.Generic;

namespace ThreadIntro
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //var a = 5;
            //var b = 5.5;
            //var c = 5.5F;
            //var d = true;
            //var f = 'c';
            //var g = "c";


            Thread t1= new Thread(DoWorkA);
            //var t3 = new Thread(DoWorkB);
            //var t4 = new Thread(DoWorkC);
            //var t5 = new Thread(DoWorkD);
            var t2 = new Thread(() =>
            {
                for (int i = 100; i <= 130; i++)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name},{i}");
                    Thread.Sleep(500);
                }
            });
            t1.Start();
            t2.Start();
            //t3.Start(30);
            //t4.Start(new float[] { -20, -10 });
            //t5.Start(new List<int> {-90,-80 });
        }

        public static void DoWorkA()
        {
            for (int i = 1; i <= 30; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }

        public static void DoWorkB(object args)
        {
            var start = Convert.ToInt32(args);
            for (int i = start; i <= 100; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }

        public static void DoWorkC(object args)
        {
            //as is used when converting to a reference type 
            //var inputParams = (int[])args;
            var inputParams = args as int[];
            for (int i = inputParams[0]; i <= inputParams[1]; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }

        public static void DoWorkD(object args)
        {
            var inputParams = args as List<int>;
            for (int i = inputParams[0]; i <= inputParams[1]; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }
    }
}
