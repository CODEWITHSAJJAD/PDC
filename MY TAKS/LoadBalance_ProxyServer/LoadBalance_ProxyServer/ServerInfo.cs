using System;
namespace LoadBalance_ProxyServer
{
    public class ServerInfo
    {
        public string IpAddress { get; }
        public int Port { get; }
        public int Weight { get; }

        public ServerInfo(string ipAddress, int port, int weight)
        {
            IpAddress = ipAddress;
            Port = port;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{IpAddress}, {Port}, {Weight}";
        }
    }
}
