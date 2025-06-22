using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Assignment_server
{
    class Server
    {
        static void Main()
        {
            // Server configuration
            IPAddress Address = IPAddress.Parse("127.0.0.1");
            IPEndPoint LocalEp = new IPEndPoint(Address, 5259);
            TcpListener listener = new TcpListener(LocalEp);
            Console.WriteLine("Server is ready to listen for requests");
            listener.Start();

            Socket socket = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);
            sw.AutoFlush = true;

            Random random = new Random();
            int score = 0;
            char[] operators = { '+', '-', '*', '/' };

            for (int round = 1; round <= 5; round++)
            {
                // Generate random numbers and operator
                int num1 = random.Next(1, 11);
                int num2 = random.Next(1, 11);
                char op = operators[random.Next(0, 4)];

                // For division, ensure it's a valid operation
                if (op == '/' && num2 == 0) num2 = 1;

                // Send question to client
                string question = $"{num1},{num2},{op}";
                sw.WriteLine(question);

                // Get client's answer
                string clientAnswer = sr.ReadLine();
                if (int.TryParse(clientAnswer, out int answer))
                {
                    // Calculate correct answer
                    int correctAnswer = Calculate(num1, num2, op);
                    bool isCorrect = (answer == correctAnswer);

                    // Update score and send feedback
                    if (isCorrect) score++;
                    sw.WriteLine(isCorrect ? "Correct!" : $"Incorrect! The correct answer was {correctAnswer}");
                }
                else
                {
                    sw.WriteLine("Invalid input! Please enter a number.");
                }
            }

            // Send final score
            sw.WriteLine($"Your score: {score}/5");
            Console.WriteLine($"Game completed. Final score sent: {score}/5");

            listener.Stop();
            Console.ReadKey();
        }

        static int Calculate(int num1, int num2, char op)
        {
            switch (op)
            {
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                case '*':
                    return num1 * num2;
                case '/':
                    return num1 / num2;
                default:
                    return 0;
            }
        }
    }
}