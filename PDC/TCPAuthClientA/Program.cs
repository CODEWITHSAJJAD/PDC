using System;
using System.Net.Sockets;
using System.Text;

namespace TCPAuthClientA
{
    

    class Client
    {
        static void Main()
        {
            // Server configuration
            string ipAddress = "127.0.0.1";
            int port = 12345;

            // Client authentication code
            string authCode = "authcode0"; // Change this based on the client's authentication code

            // Data to be sent to the server
            string data = "Hello, Mr. Server!";

            using (TcpClient client = new TcpClient(ipAddress, port))
            using (NetworkStream stream = client.GetStream())
            {
                // Send authentication code to the server
                byte[] authCodeBuffer = Encoding.ASCII.GetBytes(authCode);
                stream.Write(authCodeBuffer, 0, authCodeBuffer.Length);

                // Send data to the server
                byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
                stream.Write(dataBuffer, 0, dataBuffer.Length);

                // Receive response from the server
                byte[] responseBuffer = new byte[1024];
                int responseBytes = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, responseBytes);
                Console.WriteLine($"Response from server: {response}");
            }
            Console.ReadKey();
        }
    }

}
