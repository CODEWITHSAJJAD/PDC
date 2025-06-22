using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_2_2
{
    class MainClass
    {

        

        public static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(8, 5);

            for (int x = 1; x <= 600; x++)
                ThreadPool.QueueUserWorkItem(
                       WorkB,
                        ("Habib",23,3.4)
                    );

            //for (int x = 1; x <= 600; x++)
            //    ThreadPool.QueueUserWorkItem(
            //            new WaitCallback(WorkB),
            //            ("Habib", 23, 3.4)
            //        );




            //Two types of thread
            //1 Foregroud thread
            //2 Background thread
            //var t1 = new Thread(WorkA);
            //t1.IsBackground = true;
            //t1.Start();

            //Thread.Sleep(1600);
            Console.WriteLine("Main Done");
           Console.ReadKey();
        }

        public static void WorkB(object input)
        {
            if(input==null)
            {
                Console.WriteLine("parameter is null");
            }
            var (name, age, cgpa) = ((string,int,double))input;
            Console.WriteLine($"Input data: " +
                $"Name:{name},Age:{age}" +
                $",CGPA:{cgpa}");

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            int maxWorkerThreads, maxCompletionPortThreads;
            ThreadPool.GetMaxThreads(
                out maxWorkerThreads,
                out maxCompletionPortThreads
                );
            Console.WriteLine($"Maximum worker threads: " +
                $"{maxWorkerThreads}, " +
                $"Completion port threads: " +
                $"{maxCompletionPortThreads}");

            int minWorkerThreads, minCompletionPortThreads;
            ThreadPool.GetMinThreads(
                out minWorkerThreads,
                out minCompletionPortThreads);
            Console.WriteLine($"Minimum worker threads: " +
                $"{minWorkerThreads}, " +
                $"Completion port threads: {minCompletionPortThreads}");

            //var (a,b,c) = ((int,string, float)) input;
            //Console.WriteLine($"{a},{b},{c}");
            Thread.Sleep(6000);
        }

        public static void WorkA()
        {
            Console.WriteLine($"Started:{DateTime.Now}");
            for (int x = 1; x <=20; x++)
            {
                Console.WriteLine($"WorkA-{x}");
                Console.WriteLine($"working:{DateTime.Now}");
                Thread.Sleep(new TimeSpan(0,0,0,0,200));
            }

            Console.WriteLine($"Completed:{DateTime.Now}");
        }
    }
}
