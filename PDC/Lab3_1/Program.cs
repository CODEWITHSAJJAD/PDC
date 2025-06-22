using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_1
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            
            //ThreadPool
            //delegate - is a function pointer.
            // ThreadPool.QueueUserWorkItem(WorkA);

            //var t = new Thread(WorkB);
            //t.Start();



            for (int i = 1; i <=20; i++)
            {
                ThreadPool.QueueUserWorkItem(WorkB,i);
                //new Thread(WorkB).Start();
            }


            //Thread.Sleep(5000);
           // Console.WriteLine("Main Done");
            Console.ReadKey();
        }
        public static void Work() { }
        public static void WorkA(object args)
        {
            //while (true)
            //{
            //    //get from Monday.com
            //    //check and insert into localdb.

            //    Thread.Sleep(new TimeSpan(0, 5, 0));
            //}




            for (int i = 1; i <=30; i++)
            {
                Console.WriteLine($"Work-A - {i}/30");
                Thread.Sleep(new TimeSpan(0,0,0,0,500));
            }
        }

        public static void WorkB(object args)
        {
            if(args!=null)
            Console.WriteLine($"Value-{args}");
            else
                Console.WriteLine("Null args");
             Console.WriteLine($"{Thread.CurrentThread.IsThreadPoolThread}," +
                 $" {Thread.CurrentThread.ManagedThreadId}");
            
        }
    }
}
