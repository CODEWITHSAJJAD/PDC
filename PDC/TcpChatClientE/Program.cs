using System;

namespace TcpChatClientE
{
    class MainClass
    {
        public static void Main()
        {
            TcpChatClient chatClient = new TcpChatClient("127.0.0.1", 8888);

            // Continuously read messages from the console and send them to the server
            string input;
            while ((input = Console.ReadLine()) != null)
            {
                chatClient.SendData(input);
            }

            chatClient.Close();
        }
    }
}
