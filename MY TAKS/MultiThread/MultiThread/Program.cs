using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            Thread p1 = new Thread(ProcessA);
            p1.Start();         
            Console.WriteLine("process A done");
            Thread p2 = new Thread(ProcessB);
            p2.Start();
            Console.WriteLine("process B done");


        }
        public static void ProcessA()
        {
            for (int x = 2; x <= 100; x += 2)
            {
                Console.WriteLine(x);
                Thread.Sleep(1000);
            }
        }
        public static void ProcessB()
        {
            for (int x = 1; x <= 100; x += 2)
            {
                Console.WriteLine(x);
                Thread.Sleep(1000);
            }
        }
    }
}