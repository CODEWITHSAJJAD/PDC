using System;
using System.Net.Sockets;
using System.Text;

namespace TCPClientForProxy
{
    

    class Client
    {
        static void Main(string[] args)
        {
            string proxyServer = "127.0.0.1";
            int proxyPort = 7000;

            try
            {
                Console.WriteLine("Connecting to proxy server...");
                TcpClient client = new TcpClient(proxyServer, proxyPort);
                NetworkStream stream = client.GetStream();

                // Send a request to the proxy server
                byte[] buffer = Encoding.UTF8.GetBytes("Hello Proxy Server");
                stream.Write(buffer, 0, buffer.Length);

                // Read the response from the proxy server
                buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received: " + response);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to proxy server: " + ex.Message);
            }
        }
    }

}
