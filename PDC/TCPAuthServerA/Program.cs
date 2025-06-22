using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPAuthServerA
{
    class Server
    {
        static void Main()
        {
            // Server configuration
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            // Dictionary to store client authentication codes
            var clientAuthCodes = new System.Collections.Generic.Dictionary<string, string>()
        {
            { "client1", "authcode1" },
            { "client2", "authcode2" }
        };

            // Create TCP listener
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine($"Server listening on {ipAddress}:{port}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"Accepted connection from {((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}");

                // Handle client connection
                HandleClientConnection(client, clientAuthCodes);
            }
        }

        static void HandleClientConnection(TcpClient client, System.Collections.Generic.Dictionary<string, string> clientAuthCodes)
        {
            NetworkStream stream = client.GetStream();

            // Receive authentication code from client
            byte[] authCodeBuffer = new byte[9];
            int authCodeBytes = stream.Read(authCodeBuffer, 0, authCodeBuffer.Length);
            string authCode = Encoding.ASCII.GetString(authCodeBuffer, 0, authCodeBytes);

            // Receive data from client
            byte[] dataBuffer = new byte[1024];
            int dataBytes = stream.Read(dataBuffer, 0, dataBuffer.Length);
            string data = Encoding.ASCII.GetString(dataBuffer, 0, dataBytes);

            // Check if authentication code is valid
            if (ValidateAuthCode(authCode, clientAuthCodes))
            {
                // Process the client's request
                Console.WriteLine($"Received data from client: {data}");
                // Here you can implement the logic to process the request

                // Example response
                string response = "Request processed successfully";
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);
            }
            else
            {
                Console.WriteLine("Invalid authentication code. Connection closed.");
                // Example response
                string response = "Request Rejected";
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBuffer, 0, responseBuffer.Length);
            }

            client.Close();
        }

        static bool ValidateAuthCode(string authCode, System.Collections.Generic.Dictionary<string, string> clientAuthCodes)
        {
            // Check if the received authentication code is valid
            foreach (var pair in clientAuthCodes)
            {
                if (authCode == pair.Value)
                    return true;
            }
            return false;
        }
    }

}
