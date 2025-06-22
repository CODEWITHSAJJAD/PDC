using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPSecondaryServer
{
  

    class SecondaryServer
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 6000);
            listener.Start();
            Console.WriteLine("Server2 started on port 6000.");

            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes("Hello from Server2");
                    stream.Write(buffer, 0, buffer.Length);
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }

}
