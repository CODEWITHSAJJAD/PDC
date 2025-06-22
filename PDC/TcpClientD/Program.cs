using System;
using System.Threading;

namespace TcpClientD
{
    class MainClass
    {
        public static void Main()
        {
            TcpClientExample client = new TcpClientExample("127.0.0.1", 8888);
            client.SendData("Hello Server");

            // Wait for some time to receive the broadcasted messages
            Thread.Sleep(10000);

            client.Close();
        }
    }
}
