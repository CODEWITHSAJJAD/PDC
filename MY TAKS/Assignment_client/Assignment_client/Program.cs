using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Assignment_client
{
    class Client
    {
        static void Main(string[] args)
        {
            var remoteIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5259);
            TcpClient client = new TcpClient();
            client.Connect(remoteIp);
            Console.WriteLine("Connected to server");

            var stream = client.GetStream();
            var sr = new StreamReader(stream);
            var sw = new StreamWriter(stream);
            sw.AutoFlush = true;

            for (int round = 1; round <= 5; round++)
            {
                // Receive question from server
                string question = sr.ReadLine();
                string[] parts = question.Split(',');

                if (parts.Length == 3 &&
                    int.TryParse(parts[0], out int num1) &&
                    int.TryParse(parts[1], out int num2))
                {
                    char op = parts[2][0];
                    Console.Write($"Round {round}: What is {num1} {op} {num2}? ");

                    // Get user input and send to server
                    string userAnswer = Console.ReadLine();
                    sw.WriteLine(userAnswer);

                    // Receive and display feedback
                    string feedback = sr.ReadLine();
                    Console.WriteLine(feedback);
                }
            }

            // Receive and display final score
            string finalScore = sr.ReadLine();
            Console.WriteLine(finalScore);
            Console.WriteLine("Game over. Press any key to exit...");
            Console.ReadKey();
        }
    }
}