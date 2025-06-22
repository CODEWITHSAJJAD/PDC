using System;
using System.Threading;

namespace Introduction
{
    /*
     Suppose you have 3 processes and each has to print number within ranges mentioned below:
    1. first will print values from 1 - 10, taking 1 second to print each value
    2. second will print values from 11 - 18 taking 1 sec. each
    3. 3rd will print values from 19-28 taking 1 sec. each
     */

    class MainClass
    {
        public static void ProcessA()
        {
            Console.WriteLine($"Before:{DateTime.Now.ToLongTimeString()}");
            for (int i = 2; i <= 50; i+=2)
            {
                Console.WriteLine($"Even#:{i}");
                Thread.Sleep(1000);
            }
            Console.WriteLine($"After:{DateTime.Now.ToLongTimeString()}");
        }
        //process, task or thread
        public static void Main(string[] args)
        {
            Console.WriteLine("Sensor 1:");
            var p1= new Thread(new ThreadStart(ProcessA));
            p1.Start();

            Console.WriteLine("Sensor 2:");
            var p2 = new Thread(ProcessA);
            p2.Start();
            Console.ReadKey();
        }
    }
}
