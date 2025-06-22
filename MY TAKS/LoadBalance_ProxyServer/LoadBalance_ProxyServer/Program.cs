
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LoadBalance_ProxyServer
{
    public class ProxyServer
    {
        private RoundRobinBasedServerPool serverPool;
        
        public ProxyServer(RoundRobinBasedServerPool pool)
        {
            this.serverPool = pool;
        }

        public void Start(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine($"Proxy server listening at {port}");

            try
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(client);
                }
            }
            finally
            {
                listener.Stop();
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            IPEndPoint serverEndpoint = serverPool.GetNextServer();

            using (TcpClient server = new TcpClient())
            {
                server.Connect(serverEndpoint);

                using (NetworkStream clientStream = client.GetStream())
                using (NetworkStream serverStream = server.GetStream())
                {
                    clientStream.CopyTo(serverStream);
                }
            }
            
            client.Close();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RandomServerPool pool3 = new RandomServerPool();
            // Add your backend server IP addresses and ports here
            pool3.AddServer("127.0.0.1", 5001);
            pool3.AddServer("127.0.0.1", 5002);
            pool3.AddServer("127.0.0.1", 5003);
            pool3.AddServer("127.0.0.1", 5004);
            pool3.AddServer("127.0.0.1", 5005);

            //pool3.GetRandomServer();

            WeightedRandomServerPool pool4 = new WeightedRandomServerPool();
            pool4.AddServer("127.0.0.1", 5001, 10); // Example servers with weights
            pool4.AddServer("127.0.0.1", 5002, 5);
            pool4.AddServer("127.0.0.1", 5003, 1);

            //pool4.GetRandomServer();


            RoundRobinBasedServerPool pool1 = new RoundRobinBasedServerPool();
            // Add your backend server IP addresses and ports here
            pool1.AddServer("127.0.0.1", 5001); 
            pool1.AddServer("127.0.0.1", 5002);
            pool1.AddServer("127.0.0.1", 5003);

            //pool1.GetNextServer();


            WeightedRoundRobinServerPool pool2 = new WeightedRoundRobinServerPool();
            pool2.AddServer("127.0.0.1", 5001, 5); // Example servers with weights
            pool2.AddServer("127.0.0.1", 5002, 4);
            pool2.AddServer("127.0.0.1", 5003, 2);


            //for(int x=0;x<10;x++)
            //{
            //    Console.WriteLine(pool2.GetNextServer());
            //}



            ProxyServer proxy = new ProxyServer(pool1);
            // Port for the proxy server to listen on
            proxy.Start(8080); 
        }
    }

}
