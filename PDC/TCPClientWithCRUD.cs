using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace TCPClientWithCRUD
{
    class Program
    {
        private const string ServerAddress = "127.0.0.1";
        private const int ServerPort = 5000;

        static void Main(string[] args)
        {
            Console.Write("Enter AuthCode: ");
            string authCode = Console.ReadLine();

            if (string.IsNullOrEmpty(authCode))
            {
                Console.WriteLine("AuthCode is required. Exiting...");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n--- Menu ---");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Update Student");
                Console.WriteLine("3. Delete Student");
                Console.WriteLine("4. Retrieve Student");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                if (choice == "5")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                string request = BuildRequest(authCode, choice);
                if (string.IsNullOrEmpty(request)) continue;

                string response = SendRequestToServer(request);
                Console.WriteLine($"\nServer Response: {response}");
            }
        }

        private static string BuildRequest(string authCode, string choice)
        {
            string operation = choice switch
            {
                "1" => "Add",
                "2" => "Update",
                "3" => "Delete",
                "4" => "Retrieve",
                _ => null
            };

            if (operation == null)
            {
                Console.WriteLine("Invalid choice. Try again.");
                return null;
            }

            object payload = operation switch
            {
                "Add" => GetStudentDetails(),
                "Update" => GetStudentDetails(),
                "Delete" => GetRegNo(),
                "Retrieve" => GetRegNo(),
                _ => null
            };

            if (payload == null)
            {
                Console.WriteLine("Failed to create payload. Try again.");
                return null;
            }

            var request = new 
            {
                AuthCode = authCode,
                Operation = operation,
                Payload = payload
            };

            return JsonConvert.SerializeObject(request);
        }

        private static string GetRegNo()
        {
            Console.Write("Enter Registration Number: ");
            return Console.ReadLine();
        }

        private static object GetStudentDetails()
        {
            Console.Write("Enter Registration Number: ");
            string regNo = Console.ReadLine();
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Gender: ");
            string gender = Console.ReadLine();
            Console.Write("Enter Age: ");
            int age = int.TryParse(Console.ReadLine(), out int a) ? a : 0;
            Console.Write("Enter Program: ");
            string program = Console.ReadLine();
            Console.Write("Enter Semester Number: ");
            int semesterNo = int.TryParse(Console.ReadLine(), out int s) ? s : 0;
            Console.Write("Enter CGPA: ");
            double cgpa = double.TryParse(Console.ReadLine(), out double c) ? c : 0.0;

            return new
            {
                RegNo = regNo,
                Name = name,
                Gender = gender,
                Age = age,
                Program = program,
                SemesterNo = semesterNo,
                CGPA = cgpa
            };
        }

        private static string SendRequestToServer(string request)
        {
            try
            {
                using (TcpClient client = new TcpClient(ServerAddress, ServerPort))
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    writer.WriteLine(request);
                    return reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
