using System;
using System.Threading.Tasks;
using System.Threading;

namespace Lab_3_2
{


    class MainClass
    {

        public static int GetOne()
        {
            return 1;
        }

       
        public static async Task Main(string[] args)
        {
            var t3 = Task.Factory.StartNew(()=> {
                Task.Delay(5000).Wait();
                return 44;
            });
            Console.WriteLine(t3.Result);
            Console.ReadKey();

           var t4= Task.Factory.StartNew(SomeWorkA1, 44);

           var t1= Task.Run(SomeWorkB);
           var t = Task.Run(SomeWorkC);
           
           var t2= Task.Run(SomeWorkB);


            Console.WriteLine(t1.Result+t.Result+t2.Result);
            

            //var c = SomeWorkC(); OR this both same

            //Task<Task<int>> ts0 = Task.WhenAny(Task.Run(SomeWorkC), Task.Run(SomeWorkC));
            //var ans = ts0.Result;

            //var ts01 = Task.WhenAll(Task.Run(GetOne), Task.Run(GetOne));
            //var ans01 = ts01.Result;



            //SomeWorkD();
            //SomeWorkB();


            //// await SomeWorkD();

            //var t12 = SomeWorkE();

            //t12.Wait(1000);

            // Console.WriteLine("done:"+ c.Result);


            //Console.WriteLine(DateTime.Now.ToLongTimeString());
            ////var t = Task.Run(SomeWorkA);//Blocking statement.
            ////Console.WriteLine("Main Done");
            ////var t1 = Task.Run(SomeWorkA);

            ////await Task.WhenAny(t, t1);

            //Console.WriteLine(DateTime.Now.ToLongTimeString());

            //var tsk =  Task.Run(SomeWorkC);
            //await tsk;
            //Console.WriteLine("tsk:"+tsk.Result);
            //var tsk1 = SomeWorkC();
            //var ans1 = tsk1.Result;

            //for (int i = 0; i < 30; i++)
            //{
            //    Console.WriteLine($"Hello - {i}");
            //}

            ////var ans = await tsk;
            //         //OR
            ////var ans = tsk.Result;

            ////Console.WriteLine(ans);


            //Console.WriteLine(ans1);



            ////var t3 = new Task(SomeWorkF, 90);
            ////t3.Start();
            ////var t = SomeWorkE();
            ////var ans = SomeWorkC();
            ////var t1 = Task.Run(SomeWorkA);

            ////var t2 = Task.Run(SomeWorkB);

            ////Console.WriteLine(t2.Result);


            ////Task.WhenAll(t1, t2);
            ////Task.WhenAny(t1, t2);

            ////Console.WriteLine("Now Done");
            ////Console.WriteLine(ans.Result);
            Console.ReadKey();

        }

        public static void SomeWorkA()
        {
            Console.WriteLine("Start work-A");
            Task.Delay(3000).Wait();
            Console.WriteLine("End work-A");
        }
        public static void SomeWorkA1(Object a)
        {
            Console.WriteLine("Start work-A");
            Task.Delay(3000).Wait();
            Console.WriteLine("End work-A");
        }
        public static int SomeWorkB()
        {
            Console.WriteLine("Start work-B");
            Task.Delay(3000).Wait();
            Console.WriteLine("End work-B");
            return 30;
        }

        public static async Task<int> SomeWorkC()
        {
            Console.WriteLine("Start work-C");
            await Task.Delay(3000);
            Console.WriteLine("End work-C");
            return 30;
        }

        public static async void SomeWorkD()
        {
            Console.WriteLine("Start work-D");
            await Task.Delay(4000);
            Console.WriteLine("End work-D");

        }

        public static async Task SomeWorkE()
        {
            Console.WriteLine("Start work-E");
            await Task.Delay(8000);
            Console.WriteLine("End work-E");

        }
        //this is not allowed.
        //public static async int SomeWorkG()
        //{
        //    Console.WriteLine("Start work-E");
        //    await Task.Delay(4000);
        //    Console.WriteLine("End work-E");
        //    return 111;

        //}
        public static void SomeWorkF(object input)
        {
            Console.WriteLine("Hello F");
            Task.Delay(4000).Wait();
            //Thread.Sleep(5000);//OR this, both same
            Console.WriteLine("Hello F end");
            Console.WriteLine(input);
        }

    }
}
