using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Multiuser_Client
{
    internal class MultiClient_CLIENTSIDE
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();
           
            Console.Write("Enter your ARID number: ");
            string aridNo = Console.ReadLine();
            byte[] aridData = Encoding.UTF8.GetBytes(aridNo);
            await _stream.WriteAsync(aridData, 0, aridData.Length);

            byte[] response = new byte[1024];
            int bytesRead = await _stream.ReadAsync(response, 0, response.Length);
            string serverResponse = Encoding.UTF8.GetString(response, 0, bytesRead);

            if (serverResponse == "NOT_FOUND")
            {
                Console.WriteLine("ARID number not found. Disconnecting...");
                _client.Close();
                return; 
            }


            Console.WriteLine($"Welcome {serverResponse}! You can now chat with the server (type 'exit' to quit):");

            _ = Task.Run(ReceiveMessagesAsync);

            while (true)
            {
                string message = Console.ReadLine();
                if (message.ToLower() == "exit") break;

                byte[] data = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(data, 0, data.Length);
            }

            _client.Close();
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (_client.Connected)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }
            }
            catch
            {
                Console.WriteLine("Disconnected from server.");
            }
        }
    }

    internal class Program
    {
        static async Task Main()
        {
            MultiClient_CLIENTSIDE client = new MultiClient_CLIENTSIDE();
            await client.ConnectAsync("127.0.0.1", 5256);
        }
    }
}