using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ParkinReservation_client
{
    internal class ParkingClient
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private string _clientId;
        private bool _isProcessingCommand = false;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();

            Console.Write("Enter your client ID: ");
            _clientId = Console.ReadLine();
            byte[] idData = Encoding.UTF8.GetBytes(_clientId);
            await _stream.WriteAsync(idData, 0, idData.Length);

            // Start notification listener on separate thread
            _ = Task.Run(ReceiveNotificationsAsync);

            while (true)
            {
                DisplayMenu();
                string input = Console.ReadLine();

                if (input.ToLower() == "6" || input.ToLower() == "exit")
                {
                    _client.Close();
                    return;
                }

                string command = ConvertMenuInputToCommand(input);
                if (command == null) continue;

                _isProcessingCommand = true;
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(command);
                    await _stream.WriteAsync(data, 0, data.Length);

                    // Read response
                    byte[] response = new byte[1024];
                    int bytesRead = await _stream.ReadAsync(response, 0, response.Length);
                    string serverResponse = Encoding.UTF8.GetString(response, 0, bytesRead);

                    Console.WriteLine(serverResponse);
                }
                finally
                {
                    _isProcessingCommand = false;
                }
            }
        }

        private string ConvertMenuInputToCommand(string input)
        {
            switch (input)
            {
                case "1":
                    return "AVAILABLE";
                case "2":
                    Console.Write("Enter spot number and hours (e.g., 5 2): ");
                    string bookingInput = Console.ReadLine();
                    return $"BOOK {bookingInput}";
                case "3":
                    return "MYSPOTS";
                case "4":
                    Console.Write("Enter spot number to release: ");
                    string spotToRelease = Console.ReadLine();
                    return $"RELEASE {spotToRelease}";
                case "5":
                    Console.Write("Enter spot number and extra hours (e.g., 5 1): ");
                    string extendInput = Console.ReadLine();
                    return $"EXTEND {extendInput}";
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    return null;
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nParking Management System");
            Console.WriteLine("1. Check available spots");
            Console.WriteLine("2. Book a spot");
            Console.WriteLine("3. View my spots");
            Console.WriteLine("4. Release a spot");
            Console.WriteLine("5. Extend booking");
            Console.WriteLine("6. Exit");
            Console.Write("Enter option (1-6): ");
        }

        private async Task ReceiveNotificationsAsync()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (_client.Connected)
                {
                    // Skip reading if we're in the middle of processing a command
                    if (_isProcessingCommand)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    if (_stream.DataAvailable)
                    {
                        int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (message.StartsWith("NOTIFICATION:"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n{message}");
                            Console.ResetColor();
                            DisplayMenu(); // Redisplay menu after notification
                        }
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification error: {ex.Message}");
            }
        }
    }

    internal class Program
    {
        static async Task Main()
        {
            ParkingClient client = new ParkingClient();
            await client.ConnectAsync("127.0.0.1", 5256);
        }
    }
}