using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CHAT_SERVER
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting chat server...");

            // Create a TCP listener on port 13000
            TcpListener server = new TcpListener(IPAddress.Parse("192.188.9.161"), 8888);
            server.Start();

            Console.WriteLine("Waiting for client connection...");

            // Accept a client connection
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            // Wait for client to send their name
            byte[] nameBuffer = new byte[1024];
            int nameBytes = stream.Read(nameBuffer, 0, nameBuffer.Length);
            string clientName = Encoding.ASCII.GetString(nameBuffer, 0, nameBytes);

            Console.WriteLine($"{clientName} has connected!");

            // Send welcome message
            string welcomeMessage = $"Welcome {clientName}! You can start chatting now.";
            byte[] welcomeData = Encoding.ASCII.GetBytes(welcomeMessage);
            stream.Write(welcomeData, 0, welcomeData.Length);

            // Start chat loop
            bool isChatting = true;
            while (isChatting)
            {
                // Receive message from client
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"{clientName}: {receivedMessage}");

                if (receivedMessage.ToLower() == "bye")
                {
                    isChatting = false;
                    Console.WriteLine($"{clientName} has left the chat.");
                    break;
                }

                // Send message to client
                Console.Write("You: ");
                string messageToSend = Console.ReadLine();
                byte[] data = Encoding.ASCII.GetBytes(messageToSend);
                stream.Write(data, 0, data.Length);

                if (messageToSend.ToLower() == "bye")
                {
                    isChatting = false;
                    Console.WriteLine("Ending chat session...");
                }
            }

            // Clean up
            stream.Close();
            client.Close();
            server.Stop();
        }
    }
}
