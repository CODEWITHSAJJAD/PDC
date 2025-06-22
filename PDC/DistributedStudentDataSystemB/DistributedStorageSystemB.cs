using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DistributedStudentDataSystemB
{
    public class Server
    {
        public string Name { get; private set; }
        private Dictionary<string, Student> storage = new Dictionary<string, Student>();

        public Server(string name)
        {
            Name = name;
        }

        public void StoreStudent(string regNo, Student student)
        {
            storage[regNo] = student;
            Console.WriteLine($"Student with RegNo '{regNo}' stored on server '{Name}'.");
        }

        public Student RetrieveStudent(string regNo)
        {
            return storage.ContainsKey(regNo) ? storage[regNo] : null;
        }

        public bool HasStudent(string regNo)
        {
            return storage.ContainsKey(regNo);
        }
    }

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

    public class DistributedStudentSystem
    {
        private List<Server> servers = new List<Server>();
        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        // Add a new server
        public void AddServer(Server server)
        {
            lockSlim.EnterWriteLock();
            try
            {
                servers.Add(server);
                Console.WriteLine($"Server '{server.Name}' added.");
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        // Remove (or mark as down) a server
        public void RemoveServer(string serverName)
        {
            lockSlim.EnterWriteLock();
            try
            {
                var server = servers.FirstOrDefault(s => s.Name == serverName);
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
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        // Store a student
        public void StoreStudent(string regNo, Student student)
        {
            lockSlim.EnterWriteLock();
            try
            {
                if (servers.Count == 0)
                {
                    Console.WriteLine("No servers available to store the student.");
                    return;
                }

                int serverIndex = Math.Abs(regNo.GetHashCode()) % servers.Count;
                servers[serverIndex].StoreStudent(regNo, student);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        // Retrieve a student
        public Student RetrieveStudent(string regNo)
        {
            lockSlim.EnterReadLock();
            try
            {
                foreach (var server in servers)
                {
                    if (server.HasStudent(regNo))
                    {
                        Console.WriteLine($"Student with RegNo '{regNo}' found on server '{server.Name}'.");
                        return server.RetrieveStudent(regNo);
                    }
                }

                Console.WriteLine($"Student with RegNo '{regNo}' not found.");
                return null;
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DistributedStudentSystem dss = new DistributedStudentSystem();

            // Create threads for concurrent operations
            Thread addServerThread = new Thread(() =>
            {
                dss.AddServer(new Server("Server1"));
                Thread.Sleep(1000);
                dss.AddServer(new Server("Server2"));
            });

            Thread removeServerThread = new Thread(() =>
            {
                Thread.Sleep(500); // Simulate a delay before removing a server
                dss.RemoveServer("Server1");
            });

            Thread searchStudentThread = new Thread(() =>
            {
                Thread.Sleep(700); // Simulate a delay before searching for a student
                dss.StoreStudent("202301", new Student("Abdul", "Mateen", 90));
                var student = dss.RetrieveStudent("202301");
                if (student != null)
                {
                    Console.WriteLine($"Retrieved Student: {student}");
                }
            });

            // Start threads
            addServerThread.Start();
            removeServerThread.Start();
            searchStudentThread.Start();

            // Wait for threads to finish
            addServerThread.Join();
            removeServerThread.Join();
            searchStudentThread.Join();

            Console.WriteLine("All operations completed.");
        }
    }
}
