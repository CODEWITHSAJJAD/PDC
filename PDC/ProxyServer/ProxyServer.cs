using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace ProxyServer
{

    class ProxyServer
    {
        private static string primaryServer = "127.0.0.1";
        private static int primaryPort = 5000;
        private static string secondaryServer = "127.0.0.1";
        private static int secondaryPort = 6000;

        static void Main(string[] args)
        {
            TcpListener proxyListener = new TcpListener(IPAddress.Any, 7000);
            proxyListener.Start();
            Console.WriteLine("Proxy server started on port 7000.");

            while (true)
            {
                try
                {
                    TcpClient client = proxyListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(HandleClient, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            try
            {
                ForwardRequest(client, primaryServer, primaryPort);
            }
            catch (Exception)
            {
                Console.WriteLine("Primary server failed. Switching to secondary server...");
                try
                {
                    ForwardRequest(client, secondaryServer, secondaryPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Both servers failed: " + ex.Message);
                }
            }
            finally
            {
                client.Close();
            }
        }

        private static void ForwardRequest(TcpClient client, string server, int port)
        {
            using (TcpClient serverClient = new TcpClient(server, port))
            {
                NetworkStream clientStream = client.GetStream();
                NetworkStream serverStream = serverClient.GetStream();

                // Forward request from client to server
                byte[] buffer = new byte[256];
                int bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                serverStream.Write(buffer, 0, bytesRead);

                // Get response from server and send back to client
                bytesRead = serverStream.Read(buffer, 0, buffer.Length);
                clientStream.Write(buffer, 0, bytesRead);
            }
        }
    }

}