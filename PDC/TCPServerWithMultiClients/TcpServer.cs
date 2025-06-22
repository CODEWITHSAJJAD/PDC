using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServerWithMultiClients
{
    public class TcpServer
    {
        TcpListener listener;
        bool isRunning;
        public TcpServer(string ip,int port)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            listener.Start();
            isRunning = true;

            Console.WriteLine("Server is Ready...");


            Task.Run(()=> {
                while (isRunning)
                {
                    try
                    {
                        Console.WriteLine("Waiting for client...");
                        var socket = listener.AcceptSocket();
                        Console.WriteLine("Client is connected...");

                        Task.Run(async () => await HandleClient(socket));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }

        private async Task HandleClient(Socket socket)
        {
            var stream = new NetworkStream(socket);
            var buffer = new byte[1024];
            using (stream)
            {
                while (isRunning) {
                    int bytesCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesCount == 0)
                    {
                        break;
                    }
                    string receivedData = System.Text.Encoding.UTF8.GetString(buffer,0,bytesCount);
                    Console.WriteLine($"Received:{receivedData}");

                    byte[] response = System.Text.Encoding.UTF8.GetBytes($"Echo:" + receivedData);
                    await stream.WriteAsync(response, 0, response.Length);
                  }

                Console.WriteLine("Client disconnected");
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener.Stop();
            Console.WriteLine("Server stopped");
        }
    }
}
