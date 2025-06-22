using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TcpClientWithMultipleServers
{
    class MainClass
    {
        public static async Task Main()
        {
            string server1 = "127.0.0.1";
            int port1 = 8888;

            string server2 = "127.0.0.1";
            int port2 = 8889;

            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int mid = array.Length / 2;
            int[] part1 = new int[mid];
            int[] part2 = new int[array.Length - mid];
            Array.Copy(array, 0, part1, 0, mid);
            Array.Copy(array, mid, part2, 0, array.Length - mid);

            ServerHandler handler1 = new ServerHandler(server1, port1);
            ServerHandler handler2 = new ServerHandler(server2, port2);

            var part0 = new List<int> {1,2,3,4 };

            string data1 = string.Join(",", part0);
            string data2 = string.Join(",", part2);

            Task sendTask1 = handler1.SendDataAsync(data1);
            Task sendTask2 = handler2.SendDataAsync(data2);

            await Task.WhenAll(sendTask1, sendTask2);

            Task<string> receiveTask1 = handler1.ReceiveDataAsync();
            Task<string> receiveTask2 = handler2.ReceiveDataAsync();

            string[] results = await Task.WhenAll(receiveTask1, receiveTask2);

            string result1 = results[0];
            string result2 = results[1];

            int finalAns = Convert.ToInt32(result1) + Convert.ToInt32(result2);

            handler1.Close();
            handler2.Close();

            Console.WriteLine("Received from server 1: " + result1);
            Console.WriteLine("Received from server 2: " + result2);
            Console.WriteLine("Over All Answer: " + finalAns);

            //// Assuming the results are numbers and need to be combined
            ///
            //int[] resultArray1 = Array.ConvertAll(result1.Split(','), int.Parse);
            //int[] resultArray2 = Array.ConvertAll(result2.Split(','), int.Parse);

            //int[] combinedResult = new int[resultArray1.Length + resultArray2.Length];
            //Array.Copy(resultArray1, combinedResult, resultArray1.Length);
            //Array.Copy(resultArray2, 0, combinedResult, resultArray1.Length, resultArray2.Length);

            //Console.WriteLine("Combined result: " + string.Join(",", combinedResult));
        }
    }
}
