using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPClientJ
{
    static void Main()
    {
        Console.Write("Enter your name: ");
        var name = Console.ReadLine();

        using (var client = new TcpClient("127.0.0.1", 5000))
        using (var stream = client.GetStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        {
            // Send the user's name to the server
            writer.WriteLine(name);

            // Start a thread to listen for messages from the server
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var serverMessage = reader.ReadLine();
                        if (!String.IsNullOrEmpty(serverMessage))
                        {
                            Console.WriteLine(serverMessage);
                        }else
                        {
                            Console.WriteLine("Disconnected from server.");
                            break;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Disconnected from server.");
                        break;
                    }
                }
            }).Start();

            // Read user input and send it to the server
            string message;
            while ((message = Console.ReadLine()) != null)
            {
                writer.WriteLine(message);
            }
        }
    }
}
