using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ServiceLayer
{
    public class Services
    {
        private static byte[] _data = new byte[16777216];
        private static TcpClient _tcpClient;
        private readonly static string _userCredentialsPath = @$"{Directory.GetCurrentDirectory()}/DiabetesTrackerCredentials.txt";

        // Connect to the server
        public static void SetUpConnection()
        {
            if (_tcpClient == null)
                _tcpClient = new TcpClient("127.0.0.1", 5400);
        }

        // Diconenct from the servet
        public static void RemoveConnection()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Client.Shutdown(SocketShutdown.Both);
                _tcpClient.Close();
                _tcpClient = null;
            }
        }

        // Convert the bytes into a string
        public static string FormatData()
        {
            return Encoding.ASCII.GetString(_data).Replace("\0", String.Empty);
        }

        // Clear the data buffer
        public static void FlushBuffer()
        {
            Array.Clear(_data, 0, _data.Length);
        }

        // Communication with the server
        private static string ClientToServerComunication(string message)
        {
            // Clear the data buffer
            FlushBuffer();

            // Send message to the server
            _tcpClient.Client.Send(Encoding.UTF8.GetBytes(message));
            // Wait until a response is recieved
            _tcpClient.Client.Receive(_data);

            // Format tha data
            string serialisedData = FormatData();
            // If the first argument is '0' throw exception
            if (serialisedData.Split('|')[0] == "1")
                throw new Exception(serialisedData.Split('|')[1]);

            // Else return the data
            return serialisedData;
        }
    }
}
