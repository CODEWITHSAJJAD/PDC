
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace TcpChatClientE
{
    class TcpChatClient
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    public TcpChatClient(string server, int port)
    {
        client = new TcpClient();
        client.Connect(server, port);
        stream = client.GetStream();
        Console.WriteLine("Connected to server...");

        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();
    }

    public void SendData(string data)
    {
        byte[] dataBytes = Encoding.ASCII.GetBytes(data);
        stream.Write(dataBytes, 0, dataBytes.Length);
    }

    private void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received from server: " + message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void Close()
    {
        stream.Close();
        client.Close();
    }

   
}
}