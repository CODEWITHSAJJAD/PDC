
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace TcpClientWithMultipleServers
{
    public class ServerHandler
    {

        private TcpClient client;
        private NetworkStream stream;

        public ServerHandler(string server, int port)
        {
            client = new TcpClient();
            client.Connect(server, port);
            stream = client.GetStream();
            Console.WriteLine($"Connected to server {server} on port {port}...");
        }

        public async Task SendDataAsync(string data)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
        }

        public async  Task<string> ReceiveDataAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

    }
   
}

