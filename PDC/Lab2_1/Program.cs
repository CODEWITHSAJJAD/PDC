using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab2_1
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            var activity = new Activity();

            var input1 = new List<int> { 4, 5, 3, 7, 90, 8, 9, 12, 13 };
            var input2 = new List<int> { 14, 50, 30, 17, 190, 18, 19, 112, 13 };
            var input3 = new List<int> { 40, 105, 30, 107, 990, 98, 99, 912, 213 };


            var t11 = new Thread(activity.Sort);
            var t12 = new Thread(activity.Sort);
            var t13 = new Thread(activity.Sort);
            t11.Start(input1);
            t12.Start(input2);
            t13.Start(input3);

            Console.ReadKey();

            //Thread t1 = new Thread(activity.DoWorkA);
            //var t3 = new Thread(activity.DoWorkB);
            //var t4 = new Thread(activity.DoWorkC);
            //var t5 = new Thread(Activity.DoWorkD);
            //var t2 = new Thread(() =>
            //{
            //    for (int i = 100; i <= 130; i++)
            //    {
            //        Console.WriteLine($"{Thread.CurrentThread.Name},{i}");
            //        Thread.Sleep(500);
            //    }
            //});
            //t1.Start();
            //t2.Start();
            //t3.Start(30);
            //t4.Start(new float[] { -20, -10 });
            //t5.Start(new List<int> {-90,-80 });
        }
    }
}
