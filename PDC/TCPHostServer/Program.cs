using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPHostServerJ
{
    private static TcpListener _server;
    private static Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();
    private static object _lock = new object();

    static void Main()
    {
        StartServer();
    }

    static void StartServer()
    {
        int port = 5000;
        _server = new TcpListener(IPAddress.Any, port);
        _server.Start();
        Console.WriteLine($"Server started on port {port}");

        while (true)
        {
            var client = _server.AcceptTcpClient();
            Console.WriteLine("Client connected!");
            ThreadPool.QueueUserWorkItem(HandleClient, client);
        }
    }

    static void HandleClient(object obj)
    {
        var client = (TcpClient)obj;
        var stream = client.GetStream();
        string clientName = null;

        try
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                // First message from the client is their name   
                clientName = reader.ReadLine();
                lock (_lock)
                {
                    _clients[clientName] = client;
                }
                Console.WriteLine($"Client '{clientName}' joined.");

                // Broadcast to all other clients that a new user has joined
                Broadcast($"{clientName} has joined the chat!", clientName);

                // Listen for messages from this client
                string message;
                while ((message = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        Console.WriteLine($"{clientName} left the chat.");
                        break;
                    }

                    Console.WriteLine($"Message from {clientName}: {message}");

                    // Parse message to determine the recipient
                    var parts = message.Split(new char[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        var recipient = parts[0];
                        var msg = parts[1];
                        SendToClient(recipient, $"{clientName}: {msg}");
                    }
                    else
                    {
                        writer.WriteLine("Invalid message format. Use: recipient:message");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client {clientName}: {ex.Message}");
        }
        finally
        {
            if (clientName != null)
            {
                lock (_lock)
                {
                    _clients.Remove(clientName);
                }
                Console.WriteLine($"Client '{clientName}' disconnected.");
                Broadcast($"{clientName} has left the chat.", clientName);
            }

            client.Close();
        }
    }

    static void Broadcast(string message, string sender)
    {
        lock (_lock)
        {
            foreach (var kvp in _clients)
            {
                if (kvp.Key != sender)
                {
                    SendMessage(kvp.Value, message);
                }
            }
        }
    }

    static void SendToClient(string recipient, string message)
    {
        lock (_lock)
        {
            if (_clients.TryGetValue(recipient, out var client))
            {
                SendMessage(client, message);
            }
            else
            {
                Console.WriteLine($"Client '{recipient}' not found.");
            }
        }
    }

    static void SendMessage(TcpClient client, string message)
    {
        try
        {
            var writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
            writer.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }
}
