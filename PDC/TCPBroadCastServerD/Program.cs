using System;

namespace TCPBroadCastServerD
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            TcpServer server = new TcpServer(8888);
            server.Start();
        }
    }
}




