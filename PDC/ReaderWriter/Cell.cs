using System;
using System.Threading;

namespace ReaderWriter
{
    public class Cell
    {
        int data;
        bool readTurn = false;//true means can read, false means can write

        public void Read()
        {
            lock (this)
            {
                Console.WriteLine("Attempting to read");
                if (readTurn == false)
                {
                    Console.WriteLine("Not read turn, so wait");
                    Monitor.Wait(this);
                    Console.WriteLine("Got the signal, continue");
                }

                Console.WriteLine(data);
                Thread.Sleep(1000);

                readTurn = false;
                Monitor.Pulse(this);
            }
        }

        public void Write(int d)
        {
            lock (this)
            {
                Console.WriteLine("Attempting to write");
                if (readTurn == true)
                {
                    Console.WriteLine("Not write turn, so wait");
                    Monitor.Wait(this);
                    Console.WriteLine("Got the signal, continue");
                }

                data = d;
                Thread.Sleep(2000);
                readTurn = true;
                Monitor.Pulse(this);
            }
        }
    }
}
