using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPSERVER_multioneclinet
{

    class ComputationServer
    {
        private TcpListener listener;

        public ComputationServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Server started on port {((IPEndPoint)listener.LocalEndpoint).Port}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected...");
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + message);

                    // Process the message (sum the array elements)
                    int[] numbers = Array.ConvertAll(message.Split(','), int.Parse);
                    int evensum = 0;
                    int oddsum = 0;
                    foreach (int number in numbers)
                    {
                        if (number % 2 == 0)
                            evensum += number;

                        else
                            oddsum += number;
                    }
                   
                    string result = $"{evensum},{oddsum}";
                    byte[] resultBytes = Encoding.ASCII.GetBytes(result);
                    stream.Write(resultBytes, 0, resultBytes.Length);
                    Console.WriteLine($"Sent: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Client disconnected...");
            }
        }
    }
}
