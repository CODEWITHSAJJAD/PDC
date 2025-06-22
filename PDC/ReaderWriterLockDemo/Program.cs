using System;
using System.Threading;

namespace ReaderWriterLockDemo
{
    class MainClass
    {
       
        public static void Main(string[] args)
        {
            //int waitingReadCount = 0;
            //int waitingWriteCount = 0;

            int counter = 0;
            var rwLock = new ReaderWriterLock();
            
            Action workADelegate = delegate ()
            {//PRINTING THE COUNTER VARIABLE
                while (true)
                {
                    //waitingReadCount++;
                    rwLock.AcquireReaderLock(-1); //-1 for infinite wait.  OR use Timeout.Infinite
                    //waitingReadCount--;
                    Console.WriteLine("Start Reading...");
                    Console.WriteLine($"Read value:{counter}");
                    Thread.Sleep(1000);
                    Console.WriteLine("End Reading...");
                    rwLock.ReleaseReaderLock();
                }
            };

            Action workBDelegate = delegate ()
            {//MODIFY THE COUNTER-I.E. INCREMENTING.
                while (true)
                {
                    rwLock.AcquireWriterLock(Timeout.Infinite); // or use -1 for infinite
                    Console.WriteLine("Start Writing...");
                    counter++;
                    Console.WriteLine($"New value:{counter}");
                    Thread.Sleep(100);
                    Console.WriteLine("End Writing...");
                    rwLock.ReleaseWriterLock();
                }
            };

            Action workCDelegate = delegate ()
            {//MODIFY THE COUNTER-I.E. DECREMENTING.
                while (true)
                {
                    rwLock.AcquireWriterLock(Timeout.Infinite); // or use -1 for infinite
                    Console.WriteLine("Start Writing...");
                    counter--;
                    Console.WriteLine($"New value:{counter}");
                    Thread.Sleep(100);
                    Console.WriteLine("End Writing...");
                    rwLock.ReleaseWriterLock();
                }
            };

            var tr1 = new Thread(new ThreadStart(workADelegate));//Reader-1 thead
            tr1.IsBackground = true;
            tr1.Start();

            var tr2 = new Thread(new ThreadStart(workADelegate));//Reader-2
            tr2.IsBackground = true;
            tr2.Start();

            var tr3 = new Thread(new ThreadStart(workADelegate));//Reader-3

            var tw1 = new Thread(new ThreadStart(workBDelegate));//Writer-1
            var tw2 = new Thread(new ThreadStart(workBDelegate));//Writer-2
            var tw3 = new Thread(new ThreadStart(workCDelegate));//Writer-3

           
           
            tr3.IsBackground = true;
            tw1.IsBackground = true;
            tw2.IsBackground = true;
            tw3.IsBackground = true;


           
            
            tr3.Start();
            tw1.Start();
            tw2.Start();
            tw3.Start();

            Console.ReadKey();
        }
    }
}
