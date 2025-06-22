using System;

namespace TcpChatServerE
{
    class MainClass
    {
        public static void Main()
        {
            TcpChatServer server = new TcpChatServer(8888);
            server.Start();
        }
    }
}
