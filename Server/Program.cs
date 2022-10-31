using System;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(5400);

            server.ServerSertUp();

            Console.ReadKey();

            server.ServerShutDown();
        }
    }
}
