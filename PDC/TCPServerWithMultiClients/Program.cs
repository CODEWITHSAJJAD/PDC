using System;

namespace TCPServerWithMultiClients
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            var server = new TcpServer("127.0.0.1", 5256);
            server.Start();

            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            server.Stop();
        }
    }
}
