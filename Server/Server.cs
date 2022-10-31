using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private Master masterClass;

        private static TcpListener _tcpListener;
        private static List<TcpClient> _clients = new List<TcpClient>();
        // Buffer
        private static byte[] _data = new byte[16777216];
        private int _port;
        private static int _success = 0;
        private static int _error = 1;

        public Server(int port)
        {
            _port = port;
            masterClass = new Master();
        }

        public void ServerSertUp()
        {
            masterClass.OpenConnection();

            _tcpListener = new TcpListener(IPAddress.Any, _port);
            // Starts the server
            _tcpListener.Start();
            // Starts accepting clients
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public void ServerShutDown()
        {
            masterClass.CloseConnection();

            // Stops the server
            _tcpListener.Stop();
            _tcpListener = null;
        }

        public static void AcceptClients(IAsyncResult asyncResult)
        {
            // Newly connection client
            TcpClient client;
            try
            {
                // Connect the client
                client = _tcpListener.EndAcceptTcpClient(asyncResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Add the client newly connect vlient into the _clients list
            _clients.Add(client);
            // Begin recieving bytes from the client
            client.Client.BeginReceive(_data, 0, _data.Length, SocketFlags.None, new AsyncCallback(ReciveUserInput), client);
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public static void ReciveUserInput(IAsyncResult asyncResult)
        {
            TcpClient client = asyncResult.AsyncState as TcpClient;
            int reciever;
            try
            {
                // How many bytes has the user sent
                reciever = client.Client.EndReceive(asyncResult);
                // If the bytes are - disconnect the client
                if (reciever == 0)
                {
                    DisconnectClient(client);
                    return;
                }
                // Get the data
                string data = Encoding.ASCII.GetString(_data).Replace("\0", String.Empty);
                Console.WriteLine("data:" + data);
            }
            catch (Exception ex)
            {
                string response = $"{_error}|{ex.Message}";
                // send data to the client
                client.Client.Send(Encoding.ASCII.GetBytes(response));
            }
            finally
            {
                FlushBuffer();
            }
            client.Client.BeginReceive(_data, 0, _data.Length, SocketFlags.None, new AsyncCallback(ReciveUserInput), client);
        }

        // Clear the buffer
        public static void FlushBuffer()
        {
            Array.Clear(_data, 0, _data.Length);
        }

        public static void DisconnectClient(TcpClient client)
        {
            client.Client.Shutdown(SocketShutdown.Both);
            client.Client.Close();
            _clients.Remove(client);
        }

        public static void SendCorrenspodingResponse(TcpClient client, int operationNumber, List<string> args)
        {
            UserOperation operation = (UserOperation)operationNumber;
            string response = String.Empty;
            switch (operation)
            {
                case UserOperation.Register:
                    int userId = Operations.Register(args[0], args[1], args[2]);
                    response = $"{_success}|{userId}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.LogIn:
                    response = $"{_success}|{Operations.LogIn(args[0], args[1])}";
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.LogInWithCookies:
                    response = $"{_success}|{Operations.LogInWithCookies(args[0], args[1])}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                default:
                    break;
            }
        }
    }
}
