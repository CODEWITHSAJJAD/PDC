using System;
using System.Collections.Generic;

namespace DistributedStudentDataSystemA
{
    // Simulated server node
    public class Server
    {
        public string Name { get; private set; }
        private Dictionary<string, Student> storage = new Dictionary<string, Student>();

        public Server(string name)
        {
            Name = name;
        }

        // Store a student's data
        public void StoreStudent(string regNo, Student student)
        {
            storage.Add(regNo, student);
                    //OR
            //storage[regNo] = student;
            Console.WriteLine($"Student with RegNo '{regNo}' stored on server '{Name}'.");
        }

        // Retrieve a student's data
        public Student RetrieveStudent(string regNo)
        {
            if (storage.ContainsKey(regNo))
                return storage[regNo];
            else
                return null;
            //return storage.ContainsKey(regNo) ? storage[regNo] : null;
        }

        // Check if a student's data exists
        public bool HasStudent(string regNo)
        {
            return storage.ContainsKey(regNo);
        }
    }

    // Student class
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExamScore { get; set; }

        public Student(string firstName, string lastName, int examScore)
        {
            FirstName = firstName;
            LastName = lastName;
            ExamScore = examScore;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Score: {ExamScore}";
        }
    }

    // Distributed student data system
    public class DistributedStudentSystem
    {
        private List<Server> servers = new List<Server>();        
        private int replicationFactor;

        public DistributedStudentSystem(int replicationFactor)
        {
            this.replicationFactor = replicationFactor;
        }

        // Add a new server
        public void AddServer(Server server)
        {
            servers.Add(server);
            Console.WriteLine($"Server '{server.Name}' added to the distributed system.");
        }
        public void RemoveRandomServer()
        {
            if (servers.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(servers.Count);
                string serverName = servers[randomIndex].Name;
                servers.RemoveAt(randomIndex);
                Console.WriteLine($"Server '{serverName}' removed.");
            }
            else
            {
                Console.WriteLine("No servers available to remove.");
            }
        }
        public void RemoveServer(string serverName)
        {
            if (server != null)
            {
                servers.Remove(server);
                Console.WriteLine($"Server '{serverName}' removed.");
            }
            else
            {
                Console.WriteLine($"Server '{serverName}' not found.");
            }
        }
        // Hash function to determine server assignment
        private int HashFunction(string regNo)
        {
            var index= Math.Abs(regNo.GetHashCode()) % servers.Count;
            Console.WriteLine($"PrimaryIndex::{index}, {regNo.GetHashCode()}");
            return index;
        }

        // Store student data with replication
        public void StoreStudent(string regNo, Student student)
        {
            if (servers.Count == 0)
            {
                Console.WriteLine("No servers available in the system.");
                return;
            }

            int primaryServerIndex = HashFunction(regNo);
            Console.WriteLine($"Primary server for RegNo '{regNo}': {servers[primaryServerIndex].Name}");

            // Store student data on primary server and replicate to additional servers
            for (int i = 0; i < replicationFactor; i++)
            {
                int serverIndex = (primaryServerIndex + i) % servers.Count;
                servers[serverIndex].StoreStudent(regNo, student);
            }
        }

        // Retrieve student data
        public Student RetrieveStudent(string regNo)
        {
            foreach (var server in servers)
            {
                if (server.HasStudent(regNo))
                {
                    Console.WriteLine($"Data for RegNo '{regNo}' found on server '{server.Name}'.");
                    return server.RetrieveStudent(regNo);
                }
            }
            Console.WriteLine($"Data for RegNo '{regNo}' not found.");
            return null;
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            // Create a distributed student system with a replication factor of 2
            var dss = new DistributedStudentSystem(replicationFactor: 4);

            // Add servers
            Thread addServerThread = new Thread(() =>
            {
                dss.AddServer(new Server("Server1"));
                Thread.Sleep(5000);
                dss.AddServer(new Server("Server2"));
                Thread.Sleep(5000);
                dss.AddServer(new Server("Server3"));
                Thread.Sleep(5000);
                dss.AddServer(new Server("Server4"));
                Thread.Sleep(5000);
                dss.AddServer(new Server("Server5"));
                Thread.Sleep(5000);
                dss.AddServer(new Server("Server6"));
            });
            Thread removeServerThread = new Thread(() =>
            {
                Thread.Sleep(500); // Simulate a delay before removing a server
                dss.RemoveServer("Server1");
            });
            // Store student data
            dss.StoreStudent("2024-01", new Student("Abdul", "Qadeer", 85));
            dss.StoreStudent("2024-02", new Student("Basil", "Ahmed", 92));
            dss.StoreStudent("2024-03", new Student("Qasim", "Shah", 78));

            // Retrieve student data
            var student1 = dss.RetrieveStudent("2024-01");
            Console.WriteLine(student1 != null ? $"Retrieved: {student1}" : "Student not found.");

            var student2 = dss.RetrieveStudent("202302");
            Console.WriteLine(student2 != null ? $"Retrieved: {student2}" : "Student not found.");

            var student3 = dss.RetrieveStudent("202304"); // Non-existent
            Console.WriteLine(student3 != null ? $"Retrieved: {student3}" : "Student not found.");
        }
    }
}
