using System;
using System.Collections.Generic;

namespace LoadBalance_ProxyServer
{
   
    public class WeightedRandomServerPool
    {
        private List<ServerInfo> servers;
        private Random random;
        private int totalWeight;

        public WeightedRandomServerPool()
        {
            servers = new List<ServerInfo>();
            random = new Random();
            totalWeight = 0;
        }

        public void AddServer(string ipAddress, int port, int weight)
        {
            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero");

            servers.Add(new ServerInfo(ipAddress, port, weight));
            totalWeight += weight;
        }

        public ServerInfo GetRandomServer()
        {
            if (servers.Count == 0)
                throw new InvalidOperationException("No servers available");

            int randomWeight = random.Next(1, totalWeight + 1);
            int cumulativeWeight = 0;

            foreach (var server in servers)
            {
                cumulativeWeight += server.Weight;
                if (randomWeight <= cumulativeWeight)
                    return server;
            }

            // Should never reach here
            throw new InvalidOperationException("Failed to select a server");
        }
    }
}
