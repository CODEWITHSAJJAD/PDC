using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace TaskCancellationSimple
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Level1();
        }
        public static void Level1()
        {
            var source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var t1 = Task.Run(() => { DoWorkA(token, source); },token);
            var t2 = Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
            Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
            Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
            Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
            Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
            Task.Factory.StartNew(() => { DoWorkA(token, source); }, token);
           // var t3 = Task.Factory.StartNew(() => { DoWorkB(token, source); });
            Task.WaitAll(t1,t2);
            Console.ReadKey();
        }
        public static void DoWorkA(CancellationToken token,CancellationTokenSource source)
        {
            var r = new Random();
            while (true)
            {
                //like a signal checker, if signal is true then means stop.
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation Request Found");
                    break;
                }
                var temp = r.Next(0, 100);

                Console.Write($",{temp}");

                if (temp >= 20 && temp <=30)
                    source.Cancel();
                Task.Delay(1000*r.Next(1,4)).Wait();
            }
        }
        public static async void DoWorkB(CancellationToken t, CancellationTokenSource s)
        {
            var r = new Random();
            while (true)
            {
                if (t.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation Request Found from even"); break;
                }
                var temp = r.Next(0, 100);
                Console.Write($",{temp}");
                if (temp % 2 == 0)
                {
                    Console.WriteLine("Going to cancell all from even");
                    s.Cancel();
                }
                await Task.Delay(1000);
            }
        }
        public static void Level2() { 
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            
            var tasks1 = new List<Task>();
            var endings = new int[] {5,10,12,14,15 };
            for (int k = 1; k <= 5; k++)
            {
                tasks1.Add(Task.Factory.StartNew(async
                    () =>
                {
                    for (int i = 0; i <=endings[i]; i++)
                    {
                        if (token.IsCancellationRequested)
                            break;
                        Console.WriteLine($"{i}/{endings[i]}");
                        await Task.Delay(1000);
                    }
                    Console.Write("finished");
                }, token));
            }

            Task.WaitAny(tasks1.ToArray());

            Console.WriteLine("one of them completed.");
            source.Cancel();

            Console.ReadKey();
            var factory = new TaskFactory(token);
            var tasks = new List<Task>();

               tasks.Add(factory.StartNew(() =>
                {
                        int x = 1;
                        while (!token.IsCancellationRequested)
                        {
                            Console.WriteLine($"X:{x}");
                            x++;
                            Thread.Sleep(1000);
                        }
                   

                }, token));

            tasks.Add(factory.StartNew(() =>
            {
                    int y = 100;
                    while (!token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Y:{y}");
                        y++;
                    if (y == 103)
                    {
                        source.Cancel();
                    }
                    Thread.Sleep(1000);
                    
                }
                

            }, token));

            Console.WriteLine(Task.WaitAny(tasks.ToArray()));
            Console.WriteLine("All Stopped");
            Console.ReadKey();
        }
    }
}
