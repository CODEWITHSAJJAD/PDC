using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace TCPServerWithCRUD
{
    public class Student
    {
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Program { get; set; }
        public int SemesterNo { get; set; }
        public double CGPA { get; set; }
    }

    public class TcpServer
    {
        private static readonly ConcurrentDictionary<string, Student> Students = new ConcurrentDictionary<string, Student>();
        private static readonly HashSet<string> AuthorizedAuthCodes = new HashSet<string> { "AUTH123", "AUTH456"};
        // Can Add valid AuthCodes in above list.

        public static void Main(string[] args)
        {
            const int port = 5000;
            TcpListener server = new TcpListener(IPAddress.Any, port);

            server.Start();
            Console.WriteLine($"Server started on port {port}...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");
                NetworkStream stream = client.GetStream();

                try
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                    {
                        //receive client json data object.
                        string request = reader.ReadLine();
                        Console.WriteLine($"Received: {request}");

                        string response = ProcessRequest(request);
                        writer.WriteLine(response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    client.Close();
                }
            }
        }

        private static string ProcessRequest(string requestJson)
        {
            try
            {
                // Deserialize the client request
                var request = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestJson);
                string authCode = request["AuthCode"]?.ToString();
                string operation = request["Operation"]?.ToString();
                var payload = request["Payload"];
                
                if (string.IsNullOrEmpty(authCode) || AuthorizedAuthCodes.Contains(authCode)==false)
                {
                    return "Error: Unauthorized access.";
                }

                switch (operation)
                {
                    case "Add":
                        var studentToAdd = JsonConvert.DeserializeObject<Student>(payload.ToString());
                        return AddStudent(studentToAdd);

                    case "Update":
                        var studentToUpdate = JsonConvert.DeserializeObject<Student>(payload.ToString());
                        return UpdateStudent(studentToUpdate);

                    case "Delete":
                        string regNoToDelete = payload.ToString();
                        return DeleteStudent(regNoToDelete);

                    case "Retrieve":
                        string regNoToRetrieve = payload.ToString();
                        return RetrieveStudent(regNoToRetrieve);

                    default:
                        return "Error: Invalid operation.";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private static string AddStudent(Student student)
        {
            if (Students.TryAdd(student.RegNo, student))
            {
                return "Student added successfully.";
            }
            return "Error: Student with the same RegNo already exists.";
        }

        private static string UpdateStudent(Student student)
        {
            if (Students.ContainsKey(student.RegNo))
            {
                Students[student.RegNo] = student;
                return "Student updated successfully.";
            }
            return "Error: Student with the given RegNo does not exist.";
        }

        private static string DeleteStudent(string regNo)
        {
            if (Students.TryRemove(regNo, out _))
            {
                return "Student deleted successfully.";
            }
            return "Error: Student with the given RegNo does not exist.";
        }

        private static string RetrieveStudent(string regNo)
        {
            if (Students.TryGetValue(regNo, out var student))
            {
                return JsonConvert.SerializeObject(student);
            }
            return "Error: Student with the given RegNo does not exist.";
        }
    }
}
