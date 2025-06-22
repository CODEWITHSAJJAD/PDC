using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkinReservation_server
{
    public class ParkingSpot
    {
        public int SpotNumber { get; set; }
        public bool IsOccupied { get; set; }
        public string ClientId { get; set; }
        public DateTime ReservationTime { get; set; }
        public DateTime ExpirationTime { get; set; }
    }

    internal class ParkingServer
    {
        private TcpListener _server;
        private ConcurrentDictionary<string, TcpClient> _connectedClients = new ConcurrentDictionary<string, TcpClient>();
        private ConcurrentDictionary<int, ParkingSpot> _parkingSpots = new ConcurrentDictionary<int, ParkingSpot>();
        private Timer _monitoringTimer;

        public async Task StartAsync(int port)
        {
            InitializeParkingSpots();
            StartMonitoringParkingSpots();

            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _server.Start();
            Console.WriteLine("Parking Server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private void InitializeParkingSpots()
        {
            for (int i = 1; i <= 100; i++)
            {
                _parkingSpots.TryAdd(i, new ParkingSpot
                {
                    SpotNumber = i,
                    IsOccupied = false,
                    ClientId = null,
                    ReservationTime = DateTime.MinValue,
                    ExpirationTime = DateTime.MinValue
                });
            }
            Console.WriteLine("Initialized 100 parking spots.");
        }

        private void StartMonitoringParkingSpots()
        {
            _monitoringTimer = new Timer(CheckParkingExpirations, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void CheckParkingExpirations(object state)
        {
            foreach (var spot in _parkingSpots.Values.Where(s => s.IsOccupied))
            {
                TimeSpan remainingTime = spot.ExpirationTime - DateTime.Now;

                // Send notification if expiration is within 10 minutes
                if (remainingTime <= TimeSpan.FromMinutes(10) && remainingTime > TimeSpan.FromMinutes(9))
                {
                    if (_connectedClients.TryGetValue(spot.ClientId, out TcpClient client))
                    {
                        try
                        {
                            string notification = $"NOTIFICATION: Your parking spot {spot.SpotNumber} will expire in 10 minutes.";
                            byte[] data = Encoding.UTF8.GetBytes(notification);
                            NetworkStream stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                        }
                        catch
                        {
                            // Client may have disconnected
                        }
                    }
                }
                // Free the spot if expired
                else if (remainingTime <= TimeSpan.Zero)
                {
                    spot.IsOccupied = false;
                    spot.ClientId = null;
                    spot.ReservationTime = DateTime.MinValue;
                    spot.ExpirationTime = DateTime.MinValue;
                    Console.WriteLine($"Spot {spot.SpotNumber} has been freed due to expiration.");
                }
            }
        }

        // ... (keep all the existing server code, but modify the HandleClientAsync method as follows)

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            string clientId = null;

            try
            {
                // Get client ID
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                clientId = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _connectedClients.TryAdd(clientId, client);

                Console.WriteLine($"Client {clientId} connected.");
                await SendMessageAsync(stream, $"Welcome to Parking Management System, Client {clientId}");

                while (true)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received from {clientId}: {message}");

                    string response = ProcessClientRequest(clientId, message);
                    await SendMessageAsync(stream, response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {clientId}: {ex.Message}");
            }
            finally
            {
                // Cleanup
                if (clientId != null)
                {
                    _connectedClients.TryRemove(clientId, out _);
                    ReleaseAllClientSpots(clientId);
                }
                client.Dispose();
                Console.WriteLine($"Client {clientId} disconnected.");
            }
        }

        private async Task SendMessageAsync(NetworkStream stream, string message)
        {
            byte[] responseData = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(responseData, 0, responseData.Length);
        }

        private string ProcessClientRequest(string clientId, string request)
        {
            string[] parts = request.Split(' ');
            string command = parts[0].ToUpper();

            switch (command)
            {
                case "AVAILABLE":
                    return GetAvailableSpots();

                case "BOOK":
                    if (parts.Length == 3 && int.TryParse(parts[1], out int spot) && int.TryParse(parts[2], out int hours))
                    {
                        return BookSpot(clientId, spot, hours);
                    }
                    return "ERROR: Invalid booking format. Use: BOOK [spot] [hours]";

                case "MYSPOTS":
                    return GetClientSpots(clientId);

                case "RELEASE":
                    if (parts.Length == 2 && int.TryParse(parts[1], out int spotToRelease))
                    {
                        return ReleaseSpot(clientId, spotToRelease);
                    }
                    return "ERROR: Invalid release format. Use: RELEASE [spot]";

                case "EXTEND":
                    if (parts.Length == 3 && int.TryParse(parts[1], out int spotToExtend) && int.TryParse(parts[2], out int extraHours))
                    {
                        return ExtendBooking(clientId, spotToExtend, extraHours);
                    }
                    return "ERROR: Invalid extend format. Use: EXTEND [spot] [hours]";

                default:
                    return "ERROR: Unknown command. Available commands: AVAILABLE, BOOK [spot] [hours], MYSPOTS, RELEASE [spot], EXTEND [spot] [hours]";
            }
        }

        private string GetAvailableSpots()
        {
            var availableSpots = _parkingSpots.Values.Where(s => !s.IsOccupied).Select(s => s.SpotNumber);
            return $"AVAILABLE_SPOTS: {string.Join(", ", availableSpots)}";
        }

        private string BookSpot(string clientId, int spotNumber, int hours)
        {
            if (hours < 1 || hours > 5)
            {
                return "ERROR: Parking duration must be between 1 and 5 hours.";
            }

            if (_parkingSpots.TryGetValue(spotNumber, out ParkingSpot spot))
            {
                if (!spot.IsOccupied)
                {
                    spot.IsOccupied = true;
                    spot.ClientId = clientId;
                    spot.ReservationTime = DateTime.Now;
                    spot.ExpirationTime = DateTime.Now.AddHours(hours);

                    return $"SUCCESS: Spot {spotNumber} booked for {hours} hours. Expires at {spot.ExpirationTime}.";
                }
                return $"ERROR: Spot {spotNumber} is already occupied.";
            }
            return $"ERROR: Spot {spotNumber} does not exist.";
        }

        private string GetClientSpots(string clientId)
        {
            var clientSpots = _parkingSpots.Values
                .Where(s => s.IsOccupied && s.ClientId == clientId)
                .Select(s => $"Spot {s.SpotNumber} (Expires: {s.ExpirationTime})");

            return clientSpots.Any()
                ? $"YOUR_SPOTS: {string.Join(", ", clientSpots)}"
                : "You have no active parking spots.";
        }

        private string ReleaseSpot(string clientId, int spotNumber)
        {
            if (_parkingSpots.TryGetValue(spotNumber, out ParkingSpot spot))
            {
                if (spot.IsOccupied && spot.ClientId == clientId)
                {
                    spot.IsOccupied = false;
                    spot.ClientId = null;
                    spot.ReservationTime = DateTime.MinValue;
                    spot.ExpirationTime = DateTime.MinValue;
                    return $"SUCCESS: Spot {spotNumber} has been released.";
                }
                return $"ERROR: You don't own spot {spotNumber}.";
            }
            return $"ERROR: Spot {spotNumber} does not exist.";
        }

        private string ExtendBooking(string clientId, int spotNumber, int extraHours)
        {
            if (extraHours < 1 || extraHours > 5)
            {
                return "ERROR: Extension must be between 1 and 5 hours.";
            }

            if (_parkingSpots.TryGetValue(spotNumber, out ParkingSpot spot))
            {
                if (spot.IsOccupied && spot.ClientId == clientId)
                {
                    spot.ExpirationTime = spot.ExpirationTime.AddHours(extraHours);
                    return $"SUCCESS: Spot {spotNumber} extended by {extraHours} hours. New expiration: {spot.ExpirationTime}.";
                }
                return $"ERROR: You don't own spot {spotNumber}.";
            }
            return $"ERROR: Spot {spotNumber} does not exist.";
        }

        private void ReleaseAllClientSpots(string clientId)
        {
            foreach (var spot in _parkingSpots.Values.Where(s => s.IsOccupied && s.ClientId == clientId))
            {
                spot.IsOccupied = false;
                spot.ClientId = null;
                spot.ReservationTime = DateTime.MinValue;
                spot.ExpirationTime = DateTime.MinValue;
                Console.WriteLine($"Released spot {spot.SpotNumber} from disconnected client {clientId}");
            }
        }
    }

    internal class Program
    {
        static async Task Main()
        {
            ParkingServer server = new ParkingServer();
            await server.StartAsync(5256);
        }
    }
}