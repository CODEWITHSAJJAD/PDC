using System;
using System.Collections.Generic;
using System.Linq;

namespace LoadBalance_ProxyServer
{

    public class WeightedRoundRobinServerPool
    {
        private List<ServerInfo> servers;
        private int currentIndex;

        public WeightedRoundRobinServerPool()
        {
            servers = new List<ServerInfo>();
            currentIndex = -1;
        }

        public void AddServer(string ipAddress, int port, int weight)
        {
            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero");

            //for compacked array
            servers.Add(new ServerInfo(ipAddress, port, weight));

            // OR
            //for expanded array i.e. copy of server based on weight.
            //for (int i = 0; i < weight; i++)
            //{
            //    servers.Add(new ServerInfo(ipAddress, port, 1));
            //}


        }

        public ServerInfo GetNextServer()
        {
            if (servers.Count == 0)
                throw new InvalidOperationException("No servers available");

            //if compacked one. Otherwise simple round robin
            currentIndex = (currentIndex + 1) % servers.Sum(s => s.Weight);

            int cumulativeWeight = 0;
            foreach (var server in servers)
            {
                cumulativeWeight += server.Weight;
                if (currentIndex < cumulativeWeight)
                    return server;
            }

            // Should never reach here
            throw new InvalidOperationException("Failed to select a server");
        }
    }
}
