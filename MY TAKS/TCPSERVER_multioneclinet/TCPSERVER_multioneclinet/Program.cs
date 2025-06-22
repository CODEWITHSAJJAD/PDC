using System;

namespace TCPSERVER_multioneclinet
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ComputationServer <port>");
                return;
            }

            int port = int.Parse(args[0]);
            ComputationServer server = new ComputationServer(port);
            server.Start();
        }
    }
}
