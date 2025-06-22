using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Lab_3_1
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {

           //var tt = Task.Delay(5000).ContinueWith(t => { Console.WriteLine("started"); return 4; });

           // var task = Task.Run(
           //     ()=>
           //     {
           //         for (int i = 0; i <=3; i++)
           //         {
           //             Console.WriteLine($"Working .... {i+1}");
           //             Task.Delay(1000).Wait();
           //         }
           //     }
           //     );
            
            //Task<int[]> r= Task.WhenAll(Task.Run(() => { return 1; }), Task.Run(() => { return 2; }), Task.Run(() => { return 3; }));
            Task<Task<int>> r = Task.WhenAny(Task.Run(() => { return 1; }), Task.Run(() => { return 2; }), Task.Run(() => { return 3; }));
            var x = r.Result;
           // foreach (var item in r.Result)
           // {
           //     Console.WriteLine(item);
           // }

            //Console.WriteLine("Main method statement...");

            //await task;

            //Console.WriteLine("Task completed.");





            //Console.ReadKey();




            Func<int> a1 = GetRandomNumber;

            for (int i = 0; i < 5; i++)
            {

                Console.WriteLine(a1.Invoke());
            }

            Func<int, bool> a2 = IsNumberEven;
            for (int i = 1; i <=7; i++)
            {
                Console.WriteLine($"{i}-{a2.Invoke(i)}");
            }

            Console.WriteLine();

            Console.ReadKey();



            Action<List<float>,List<string>,float> d = WorkD;

            d.Invoke(new List<float> {30.6F,40.6F,70.2F },new List<string> {"Rizwan","Shairwani","Anna" },50.5F);


            Console.ReadKey();


          //  var ans = 4 * 5 + 3 * 6;

           // WorkC(WorkB());

            //Console.ReadKey();


            Action a = WorkA;
            Console.WriteLine("Main Statement Exected");
            //calling the function method-1
         //   a.Invoke();

            a();//method 2


            Action<int> b = WorkC;
            //b.Invoke(5);
            b(5);


            Console.ReadKey();
        }


        public static bool IsNumberEven(int number)
        {
            return number % 2 == 0;
        }


        public static int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1, 7);
        }



        public static void WorkA()
        {
            Console.WriteLine("Work-A Executed");
        }

        public static int WorkB()
        {
            return 3;
        }

        public static void WorkC(int input)
        {
            Console.WriteLine($"{input*2}");
        }

        public static void WorkD(List<float> marks,List<string> names, float threshold)
        {
            for (int i = 0; i <=names.Count; i++)
            {
                if(marks[i]>threshold)
                    Console.WriteLine($"Good-{names[i]}({marks[i]})");
                else
                    Console.WriteLine($"Poor-{names[i]}({marks[i]})");
            }
        }
    }
}
