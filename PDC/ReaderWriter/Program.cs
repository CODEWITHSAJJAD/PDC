using System;
using System.Threading;

namespace ReaderWriter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var cell1 = new Cell();

            var processor = new DataProcessor(cell1, 50);

            var reader = new Thread(processor.ReadAttempt);

            var writer = new Thread(processor.WriteAttempt);
                
            reader.Start();
            writer.Start();

            Console.ReadKey();
        }
    }
}
