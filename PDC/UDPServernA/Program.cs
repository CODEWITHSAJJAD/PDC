using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UdpServerA
{
    static void Main()
    {
        UdpClient server = new UdpClient(11000);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            Console.WriteLine("UDP Server started. Listening on port 11000...");

            while (true)
            {
                byte[] data = server.Receive(ref remoteEP);
                string message = Encoding.ASCII.GetString(data);
                Console.WriteLine($"Received from {remoteEP}: {message}");

                // Send response
                string response = "Message received";
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                server.Send(responseData, responseData.Length, remoteEP);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            server.Close();
        }
    }
}