using System;
using System.Threading;
using ThreadBasicsEx;
using System.Collections.Generic;

namespace ThreadBasics
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.ReadKey();
            var act = new Activities();
            ThreadPool.QueueUserWorkItem(act.TaskC,new List<int> { 1, 2, 3, 4, 5, 6 });

            Thread.Sleep(5000);
            Console.ReadKey();

            //var t1 = new Thread(TaskA);
            ////t1.IsBackground = true;
            //t1.Start();

            //var t2 =  new Thread(() =>
            //{
            //    for (int x = 11; x <= 20; x++)
            //    {
            //        Console.WriteLine(x);
            //        Thread.Sleep(500);//0.5 sec delay
            //    }
            //});
            ////t2.IsBackground = true;
            //t2.Start();


            //var t3 = new Thread(TaskB);
            ////t3.IsBackground = true;
            //t3.Start(21);

        }


       
    }
}
