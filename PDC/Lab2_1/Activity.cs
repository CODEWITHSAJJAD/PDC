using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab2_1
{
    public class Activity
    {
        //Thread safe code.
        public void Sort(Object input)
        {
            ///
            Console.WriteLine("Logic here");
        }

        public  void DoWorkA()
        {
            for (int i = 1; i <= 30; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }

        public  void DoWorkB(object args)
        {
            var start = Convert.ToInt32(args);
            for (int i = start; i <= 100; i++)
            {
                Console.WriteLine($"{i}");
                Thread.Sleep(500);
            }
        }

        public  void DoWorkC(object args)
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
