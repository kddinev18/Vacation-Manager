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


    }
}
