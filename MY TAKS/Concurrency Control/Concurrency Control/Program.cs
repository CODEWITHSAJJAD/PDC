using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class Concurrency_control
{

    static int sharedNumber = 10;

    static readonly object lockObject = new object();

    static void Main(string[] args)
    {

        Thread thread1 = new Thread(TaskAdd);
        Thread thread2 = new Thread(TaskSubtract);
        Thread thread3 = new Thread(TaskMultiply);


        thread1.Start();
        thread2.Start();
        thread3.Start();


        thread1.Join();
        thread2.Join();
        thread3.Join();

        Console.WriteLine($"Final value of sharedNumber: {sharedNumber}");
        Console.ReadLine();
    }


    static void TaskAdd()
    {
        lock (lockObject)
        {
            Console.WriteLine("TaskAdd started.");
            int before = sharedNumber;
            sharedNumber += 5;
            Console.WriteLine($"TaskAdd: {before} + 5 = {sharedNumber}");
            Thread.Sleep(500);
        }
    }


    static void TaskSubtract()
    {
        lock (lockObject)
        {
            Console.WriteLine("TaskSubtract started.");
            int before = sharedNumber;
            sharedNumber -= 3;
            Console.WriteLine($"TaskSubtract: {before} - 3 = {sharedNumber}");
            Thread.Sleep(500);
        }
    }


    static void TaskMultiply()
    {
        lock (lockObject)
        {
            Console.WriteLine("TaskMultiply started.");
            int before = sharedNumber;
            sharedNumber *= 2;
            Console.WriteLine($"TaskMultiply: {before} * 2 = {sharedNumber}");
            Thread.Sleep(500);
        }
    }
}