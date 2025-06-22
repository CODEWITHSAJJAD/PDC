using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UdpClientA
{
    static void Main()
    {
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

        try
        {
            while (true)
            {
                Console.Write("Enter message (or 'exit' to quit): ");
                string message = Console.ReadLine();

                if (message.ToLower() == "exit")
                    break;

                byte[] data = Encoding.ASCII.GetBytes(message);
                client.SendTo(data, serverEP);

                // Receive response
                byte[] responseData = new byte[1024];
                EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
                int bytes = client.ReceiveFrom(responseData, ref senderEP);
                string response = Encoding.ASCII.GetString(responseData, 0, bytes);
                Console.WriteLine($"Server response: {response}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            client.Close();
        }
    }
}