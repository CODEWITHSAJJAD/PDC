using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLockSlimDemo
{
    class MainClass
    {
        static void Main(string[] args)
        {
            ConcurrentListManager listManager = new ConcurrentListManager();

            var totalRead = 0; var totalAdd = 0; var totalDelete = 0; var totalSearch = 0;
            var totalUpdate = 0;

            // Create and start multiple threads to perform operations on the list concurrently
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Random rand = new Random();
                    for (int j = 0; j < 5;j++)
                    {
                        int operation = rand.Next(6); // Randomly select an operation
                    switch (operation)
                        {
                            case 0:
                                totalAdd++;
                                listManager.Add(rand.Next(100));
                                break;
                            case 1:
                                totalRead++;
                                foreach (var item in listManager.Read())
                                {
                                    Console.WriteLine($"Read: {item}");
                                }
                                break;
                            case 2:
                                totalSearch++;
                                int searchValue = rand.Next(100);
                                Console.WriteLine($"Search for {searchValue}: {listManager.Search(searchValue)}");
                                break;
                            case 3:
                                totalUpdate++;
                                int oldValue = rand.Next(100);
                                int newValue = rand.Next(100);
                                listManager.Update(oldValue, newValue);
                                break;
                            case 4:
                                listManager.AddOrUpdate(rand.Next(100));
                                break;
                            case 5:
                                totalDelete++;
                                int deleteValue = rand.Next(100);
                                listManager.Delete(deleteValue);
                                break;
                        }
                        Thread.Sleep(rand.Next(100)); // Simulate some delay between operations
                }
                }));
            }

            
            // Wait for all tasks to complete
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Read:{totalRead}, Add:{totalAdd}, Search:{totalSearch}, Update:{totalUpdate}, Delete:{totalDelete}");

        }
    }
}
