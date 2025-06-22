
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SynchUsingTaskFactory
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var option = 1;
            Console.WriteLine("Enter 1 to wait for all tasks, otherwise enter 0 ");
            option = Convert.ToInt32(Console.ReadLine());

            var dm = new DataMonitor();
            var tasks = new List<Task<List<int>>>();
            var factory = new TaskFactory();


            tasks.Add(factory.StartNew(dm.SourceA));
            tasks.Add(factory.StartNew(dm.SourceB));
            tasks.Add(factory.StartNew(dm.SourceC));

            Task<double> finalTask = null;
            if (option == 1)
                finalTask = factory.ContinueWhenAll(tasks.ToArray(), processAllData);
            else
                finalTask = factory.ContinueWhenAny(tasks.ToArray(), processAnyData);

            Console.WriteLine($"Answer:{finalTask.Result}");
            Console.ReadKey();
        }

        private static double processAnyData(Task<List<int>> task)
        {
            var average = 0.0;
            var sum = 0;
            var total = 0;
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Final Round Started");
            stopwatch.Start();

            foreach (var temp in task.Result)
            {
                total++;
                sum += temp;
            }

            average = (sum * 1.0) / total;
            stopwatch.Stop();
            Console.WriteLine($"Final Round Completed in Total Time(secs):{stopwatch.Elapsed.Seconds}");
            return average;
        }

        private static double processAllData(Task<List<int>>[] tasks)
        {
            var average = 0.0;
            var sum = 0;
            var total = 0;
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Final Round Started");
            stopwatch.Start();
            foreach (var task in tasks)
            {
                Thread.Sleep(500);
                foreach (var temp in task.Result)
                {
                    total++;
                    sum += temp;
                }
            }
            average = (sum * 1.0) / total;
            stopwatch.Stop();
            Console.WriteLine($"Final Round Completed in Total Time(secs):{stopwatch.Elapsed.Seconds}");
            return average;
        }
    }


    class DataMonitor
    {
        Random sensorReader = new Random();

        public List<int> SourceA()
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("SourceA Started");
            stopwatch.Start();
            var data = new List<int>();
            while (data.Count != 100)
            {
                var interval = new TimeSpan(0,0,0,0,500);
                Thread.Sleep(interval);
                lock (sensorReader)
                {
                    data.Add(sensorReader.Next(5, 50));
                }
            }
            //stopwatch..Stop();
            Console.WriteLine($"SourceA Completed in Total Time(secs):{stopwatch.Elapsed.Seconds}");
            return data;//array
        }

        public List<int> SourceB()
        {
            var data = new List<int>();
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("SourceB Started");
            stopwatch.Start();
            while (data.Count != 200)
            {
                Thread.Sleep(100);
                lock (sensorReader)
                {
                    data.Add(sensorReader.Next(5, 50));
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"SourceB Completed in Total Time(secs):{stopwatch.Elapsed.Seconds}");
            return data;
        }

        public List<int> SourceC()
        {
            var data = new List<int>();
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("SourceC Started");
            stopwatch.Start();
            while (data.Count != 50)
            {
                Thread.Sleep(100);
                lock (sensorReader)
                {
                    data.Add(sensorReader.Next(5, 50));
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"SourceC Completed in Total Time(secs):{stopwatch.Elapsed.Seconds}");
            return data;
        }
    }
}
