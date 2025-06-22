using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadBalance_ProxyServer
{
    
public class RandomServerPool
    {
        private List<ServerInfo> servers;
        private Random random;

        public RandomServerPool()
        {
            servers = new List<ServerInfo>();
            random = new Random();
        }

        public void AddServer(string ipAddress, int port)
        {
            servers.Add(new ServerInfo(ipAddress, port,1));
        }

        public ServerInfo GetRandomServer()
        {
            if (servers.Count == 0)
                throw new InvalidOperationException("No servers available");

            int index = random.Next(0, servers.Count);
            return servers[index];
        }
    }

}
