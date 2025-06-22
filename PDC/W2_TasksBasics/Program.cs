using System;
using System.Threading.Tasks;

namespace W2_TasksBasics
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            Variant8();

            Console.ReadKey();
        }

        public static void Variant1()
        {
            // Running an anonymous function using Task.Run
            Task.Run(() =>
            {
                Console.WriteLine("Anonymous function running asynchronously.");
            }).Wait(); // Wait for the task to complete

            Console.WriteLine("Main method continues...");
        }

        public static void Variant2()
        {
            // Running a named function using Task.Run
            Task.Run(NamedMethod).Wait(); // Wait for the task to complete

            Console.WriteLine("Main method continues...");
        }

        static void NamedMethod()
        {
            Console.WriteLine("Named method running asynchronously.");
        }


        public static void Variant3()
        {
            //Anonymous with return type
            Task<int> task = Task.Run(() =>
            {
                Console.WriteLine("Task is calculating...");
                return 42; // Return a value
            });

            Console.WriteLine($"Task result: {task.Result}"); // Access the result
        }

        public static async void Variant3_1()
        {
            //Anonymous with return type using wait.
            int ans = await Task.Run(() =>
            {
                Console.WriteLine("Task is calculating...");
                return 42; // Return a value
            });

            Console.WriteLine($"Task result: {ans}"); // Access the result
        }

        public static async void Variant3_2()
        {
            //Anonymous with return type using wait.
            var task = Task.Run(() =>
            {
                Console.WriteLine("Task is calculating...");
                return 42; // Return a value
            });

            var ans = await task;//OR task.Result

            Console.WriteLine($"Task result: {ans}"); // Access the result
        }

        public static void Variant3_3()
        {
            //Anonymous with return type using wait.
            var task = Task.Run(() =>
            {
                Console.WriteLine("Task is calculating...");
                return 42; // Return a value
            });

            task.Wait();//can mention time in milliseconds, OR TimeSpan

            Console.WriteLine($"Task result: {task.Result}"); // Access the result
        }


        public static void Variant4()
        {
            // Creating a TaskFactory
            TaskFactory taskFactory = new TaskFactory();
            
            // Running an anonymous function using TaskFactory
            Task task = taskFactory.StartNew(() =>
            {
                Console.WriteLine("Anonymous function running asynchronously using TaskFactory.");
            });

            task.Wait(); // Wait for the task to complete

            Console.WriteLine("Main method continues...");
        }

        public static void Variant5()
        {
            // Creating a TaskFactory
            TaskFactory taskFactory = new TaskFactory();

            // Running a named function using TaskFactory
            Task task = taskFactory.StartNew(() => NamedMethod());

            task.Wait(); // Wait for the task to complete

            Console.WriteLine("Main method continues...");
        }

        public static async void Variant6()
        {
            Console.WriteLine("Starting delay...");
            await Task.Delay(2000); // Delay for 2 seconds
            Console.WriteLine("Delay completed.");
        }

        public static async void Variant7()
        {//completed one
            Task task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:"+t.Id); });
            Task task2 = Task.Delay(5000).ContinueWith((t) => Console.WriteLine("Task 2:"+t.Id));
            Task task3 = Task.Delay(2000).ContinueWith(_ => Console.WriteLine("Task 3:" + _.Id));

           // Console.WriteLine($"{task1.Id},{task2.Id},{task3.Id}");
            Task completedTask = await Task.WhenAny(task1, task2, task3);
            Console.WriteLine($"First completed task: {completedTask.Id}");
           
        }
        public static async void Variant7_1()
        {//completed with return
            Task<int> task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id);return 1; });
            Task<int> task2 = Task.Delay(200).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 2; });
            Task<int> task3 = Task.Delay(1000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 3; });

            Task<int> completedTask = await Task.WhenAny(task1, task2, task3);
            Console.WriteLine($"First completed task: {completedTask.Result}");

        }

        public static async void Variant8()
        {
            Task task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); });
            Task task2 = Task.Delay(5000).ContinueWith((t) => Console.WriteLine("Task 2:" + t.Id));
            Task task3 = Task.Delay(2000).ContinueWith(_ => Console.WriteLine("Task 3:"+_.Id));
            
            Console.WriteLine($"{task1.Id},{task2.Id},{task3.Id}");
            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"All Tasks completed.");

        }
        public static  void Variant8_1()
        {
            Task<int> task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 10; });
            Task<int> task2 = Task.Delay(2000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 20; });
            Task<int> task3 = Task.Delay(1000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 30; });

            Task<int[]> completedTask =  Task.WhenAll(task1, task2, task3);
            foreach (var item in completedTask.Result)
            {
                Console.WriteLine($"Value: {item}");
            }
            
        }
        public static async void Variant8_2()
        {
            Task<int> task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 11; });
            Task<int> task2 = Task.Delay(2000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 22; });
            Task<int> task3 = Task.Delay(1000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 33; });

            int[] items = await Task.WhenAll(task1, task2, task3);
            foreach(var item in items)
                Console.WriteLine($"Value: {item}");
        }

        public static  void Variant9()
        {
            Task task1 = Task.Delay(3000).ContinueWith((t) => { for (int i = 0; i <= 20; i++) { Console.WriteLine("Task 1:" + t.Id);Task.Delay(1000).Wait(); } });
            Task task2 = Task.Delay(5000).ContinueWith((t) => { for (int i = 0; i <= 10; i++) { Console.WriteLine("Task 2:" + t.Id); Task.Delay(1000).Wait(); } });
            Task task3 = Task.Delay(2000).ContinueWith(_ => { for (int i = 0; i <= 30; i++) { Console.WriteLine("Task 3:" + _.Id); Task.Delay(1000).Wait(); } });
            

            //Console.WriteLine($"{task1.Id},{task2.Id},{task3.Id}");
            int id = Task.WaitAny(new Task[] { task3, task2, task1 });
            Console.WriteLine($"Index:{id}");

        }

        public static  void Variant9_1()
        {
            Task task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); });
            Task task2 = Task.Delay(5000).ContinueWith((t) => Console.WriteLine("Task 2:" + t.Id));
            Task task3 = Task.Delay(2000).ContinueWith(_ => Console.WriteLine("Task 3:"));
            

            Console.WriteLine($"{task1.Id},{task2.Id},{task3.Id}");
            Task.WaitAll(new Task[] { task3, task2, task1 });
            Console.WriteLine($"All Done");

        }

        public static void Variant9_2()
        {
            Task<int> task1 = Task.Delay(3000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 110; });
            Task<int> task2 = Task.Delay(2000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 220; });
            Task<int> task3 = Task.Delay(1000).ContinueWith((t) => { Console.WriteLine("Task 1:" + t.Id); return 330; });

            Task<int>[] tasks = { task1, task2, task3 };
            Task.WaitAll(tasks);
            foreach (var t in tasks)
                Console.WriteLine($"Value: {t.Result}");
        }
    }
}
