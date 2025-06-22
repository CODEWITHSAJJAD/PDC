using System;
using System.Collections.Generic;
using System.Net;

namespace LoadBalance_ProxyServer
{
    public class RoundRobinBasedServerPool
    {
        private List<IPEndPoint> servers;
        private int currentIndex;

        public RoundRobinBasedServerPool()
        {
            servers = new List<IPEndPoint>();
            currentIndex = 0;
        }

        public void AddServer(string ipAddress, int port)
        {
            servers.Add(new IPEndPoint(IPAddress.Parse(ipAddress), port));
        }

        public IPEndPoint GetNextServer()
        {
            if (servers.Count == 0)
                throw new InvalidOperationException("No servers available");

            lock (this)
            {
                var nextIndex = currentIndex++ % servers.Count;
                return servers[nextIndex];
            }
        }
    }
}
