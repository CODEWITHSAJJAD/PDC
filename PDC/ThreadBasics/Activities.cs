using System;
using System.Threading;
using System.Collections.Generic;

namespace ThreadBasicsEx
{
    public class Activities
    {
        public Activities()
        {
        }

        public static void TaskA()
        {
            for (int x = 1; x <= 10; x++)
            {
                Console.WriteLine(x);
                Thread.Sleep(1000);//1 sec delay
            }
        }

        public  void TaskC(object a)
        {
            var data = new List<int>();
            if (a is null)
                Console.WriteLine($"missing paramerter");
            else
            {
                data = a as List<int>;
            }

            foreach (var d in data)
            {
                Console.WriteLine(d);
                Thread.Sleep(1000);//1 sec delay
            }
        }

        public static void TaskB(object v)
        {
            int a = Convert.ToInt32(v);
            for (int x = a; x <= a + 10; x++)
            {
                Console.WriteLine(x);
                Thread.Sleep(1000);//1 sec delay
            }
        }
    }
}
