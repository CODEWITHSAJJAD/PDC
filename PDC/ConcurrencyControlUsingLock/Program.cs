using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyControlUsingLock
{
    class MainClass
    {
        static int v1 = 0;
        static readonly object lockObj = new object();
        static Random random = new Random();

        public static void Main(string[] args)
        {
            Task task1 = Task.Run(() => ProcessTask("Task 1", 1, 100, 1000, value => value * 2, 10, 2000));
            Task task2 = Task.Run(() => ProcessTask("Task 2", 50, 200, 500, value => value + 100, 10, 2000));
            Task task3 = Task.Run(() => ProcessTask("Task 3", 100, 300, 1500, value => value * 2, 10, 2000));

            Task.WaitAll(task1, task2, task3);
            Console.WriteLine("All tasks completed. Press any key to exit.");
            Console.ReadKey();
        }

        static void ProcessTask(string taskName, int minValue, int maxValue, int operationDelayMs,
                              Func<int, int> operation, int repeatCount, int intervalDelayMs)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                lock (lockObj)
                {
                    // Generate and store random number
                    v1 = random.Next(minValue, maxValue + 1);
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {taskName} acquired lock. v1 = {v1}");

                    // Perform operation after delay
                    Thread.Sleep(operationDelayMs);
                    int oldValue = v1;
                    v1 = operation(v1);
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {taskName} updated value: {oldValue} => {v1}");

                    // Unlock happens automatically when leaving the lock block
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {taskName} released lock.");
                }
                Thread.Sleep(intervalDelayMs); // Interval between task operations

            }
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {taskName} completed all iterations.");
        }
    }
}
