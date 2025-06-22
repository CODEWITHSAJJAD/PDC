using System;
using System.Threading;

namespace THREADTASK
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread p1 = new Thread(ProcessA);
            p1.Start();

            Console.WriteLine("Enter the lower bound:");
            int lowerBound = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the upper bound:");
            int upperBound = int.Parse(Console.ReadLine());

            int[] arr = { lowerBound, upperBound };
            Thread p2 = new Thread(PrintEvenNumbers);
            p2.Start(arr);
            int sum = 0;
            Thread t3 = new Thread(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    sum += i;
                    Thread.Sleep(100);
                }
            });
            t3.Start();
            Console.WriteLine(sum);
        }



        public static void ProcessA()
        {
            for (int x = 1; x <= 200; x += 2)
            {
                Console.WriteLine(x);
                Thread.Sleep(100);
            }
        }

        public static void PrintEvenNumbers(object bounds)
        {
            int[] boundsArray = (int[])bounds; 
            int lowerBound1 = boundsArray[0];
            int upperBound1 = boundsArray[1];
            for (int i = lowerBound1; i <= upperBound1; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
                if ((i - lowerBound1 + 1) % 5 == 0)
                {
                    Thread.Sleep(1000);
                }
            }

        }
    }
}
