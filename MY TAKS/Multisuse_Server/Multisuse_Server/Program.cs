using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Multisuse_Server
{
    internal class MultiClient_SERVERSIDE
    {
        private TcpListener _server;
        private ConcurrentDictionary<string, TcpClient> _clients = new ConcurrentDictionary<string, TcpClient>();
        //Safe concurrent add, remove, check operations
        private Dictionary<string, string> _students = new Dictionary<string, string>()
        {
            {"2022-ARID-3915", "Wasay"},
            {"2022-ARID-3926", "Afnan"},
            {"2022-ARID-4176", "Sajjad"}
        };

        public async Task StartAsync(int port)
        {
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _server.Start();
            Console.WriteLine("Server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            string aridNo = "";
            string clientName = "";

            try
            {                
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                aridNo = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                if (_students.TryGetValue(aridNo, out clientName))
                {
                    _clients.TryAdd(aridNo, client);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(clientName), 0, clientName.Length);
                    Console.WriteLine($"{clientName} ({aridNo}) connected.");
                }
                else
                {
                    await stream.WriteAsync(Encoding.UTF8.GetBytes("NOT_FOUND"), 0, 10);
                    client.Dispose();
                    Environment.Exit(0);
                    return;
                }
                
                while (true)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; 

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[{clientName}]: {message}");
                    await BroadcastAsync($"[{clientName}]: {message}", clientName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with {clientName}: {ex.Message}");
            }
            finally
            {
                // Cleanup
                if (!string.IsNullOrEmpty(clientName))
                {
                    _clients.TryRemove(clientName, out _);
                    // it stores the removed value in the out parameter.
                    await BroadcastAsync($"[Server] {clientName} left the chat.", clientName);
                }
                client.Dispose();
                Console.WriteLine($"{clientName} disconnected.");
            }
        }

        private async Task BroadcastAsync(string message, string senderName)
        {
            //This method broadcasts a message to all connected clients,
            byte[] data = Encoding.UTF8.GetBytes(message);
            //UTF-8 (Unicode Transformation Format – 8-bit) is used
            //character encoding on the web and in network communications.

            foreach (var client in _clients)
            {
                if (client.Key != senderName) // Skip sender
                {
                    try
                    {
                        NetworkStream stream = client.Value.GetStream();
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                    catch
                    {
                        // Remove disconnected clients
                        _clients.TryRemove(client.Key, out _);

                    }
                }
            }
        }
    }
    internal class Program
    {
        static async Task Main()
        {
            MultiClient_SERVERSIDE server = new MultiClient_SERVERSIDE();
            await server.StartAsync(5256);
        }
    }
}

