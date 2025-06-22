using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrinterSynch
{

    class Printer
    {
        object c1 = new object();
        public string Name { get; set; }

        public void PrintDocument(int noOfPages)
        {
            Console.WriteLine("Request to get Printer Access");
            lock (this)
            {
                Console.WriteLine("Printer request granted");
                for(int page = 1; page <= noOfPages; page++)
                {
                    Console.WriteLine($"{Name} - Printing " +
                        $"{page}/{noOfPages}");
                    Thread.Sleep(500);
                }
                Console.WriteLine("Job Completed.");
            }
        }
    }


    class MainClass
    {
        public static void Main(string[] args)
        {
            
                var printer1 = new Printer();
                var printer2 = new Printer();
                printer1.Name = "HP-9980";
                printer2.Name = "HP-9080";
           

            var random = new Random();
            for (int i = 1; i <=5; i++)
            {
                Task.Run(() => {
                    printer2.PrintDocument(random.Next(2,11));
                    
                });
            }

            for (int i = 1; i <= 4; i++)
            {
                Task.Run(() =>
                {
                    printer1.PrintDocument(random.Next(20, 100));

                });
            }

            //for (int i = 1; i <= 2; i++)
            //{
            //    Task.Run(() => {
            //        printer1.PrintDocument(random.Next(20, 100));

            //    });
            //}

            Console.ReadKey();

        }
    }
}
