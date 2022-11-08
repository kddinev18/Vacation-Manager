using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
namespace ServiceLayer
{
    public class Services
    {
        private static byte[] _data = new byte[16777216];
        private static TcpClient _tcpClient;
        private readonly static string _userCredentialsPath = @$"{Directory.GetCurrentDirectory()}/VacationManagerCredentials.txt";

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

        public static int Register(string userName, string email, string password)
        {
            string serialisedData = ClientToServerComunication($"{(int)UserOperation.Register}|{userName}, {email}, {password}");

            return int.Parse(serialisedData.Split('|')[1]);
        }
        public static void RegisterMember(string userName, string email, string password, string roleIdentificator)
        {
            ClientToServerComunication($"{(int)UserOperation.RegisterMember}|{userName}, {email}, {password}, {roleIdentificator}");
        }
        public static string GetUsers(int userId, int pagingSize, int skipAmount)
        {
            return ClientToServerComunication($"{(int)UserOperation.GetUsers}|{userId}, {pagingSize}, {skipAmount}").Split('|')[1];
        }
        public static int GetUserCount()
        {
            string serialisedData = ClientToServerComunication($"{(int)UserOperation.GetUserCount}|");

            return int.Parse(serialisedData.Split('|')[1]);
        }
        public static void RemoveUser(int userId)
        {
            ClientToServerComunication($"{(int)UserOperation.RemoveUser}|{userId}");
        }
        public static void EditUser(int userId, string email, string role)
        {
            ClientToServerComunication($"{(int)UserOperation.EditUser}|{userId}, {email}, {role}");
        }
        public static int LogIn(string userName, string password, bool doRememberMe)
        {
            string serialisedData = ClientToServerComunication($"{(int)UserOperation.LogIn}|{userName}, {password}");

            // Deserialize the data
            UserCredentials userCredentials = JsonSerializer.Deserialize<UserCredentials>(serialisedData.Split('|')[1]);

            // If the remember me checkbox is checked add user credentials txt file
            if (doRememberMe == true)
                AddCookies(userCredentials);
            else
                RemoveCookies();

            // Return the user id
            return userCredentials.Id;
        }

        public static int? CheckCookies()
        {
            // If there isn't a text user credential file return null
            if (!File.Exists(_userCredentialsPath))
                return null;

            // Log with the credentials from the file otherwise
            string credentials = File.ReadAllText(_userCredentialsPath);
            UserCredentials userCredentials = JsonSerializer.Deserialize<UserCredentials>(credentials);
            string serialisedData = ClientToServerComunication($"{(int)UserOperation.LogInWithCookies}|{userCredentials.UserName}, {userCredentials.HashedPassword}");
            // If the ids doesn't match throw an exception
            if (int.Parse(serialisedData.Split('|')[1]) != userCredentials.Id)
                throw new Exception("Fatal error");

            // Returns the user id
            return userCredentials.Id;
        }
        public static void RemoveCookies()
        {
            // Checks if the file is already deleted
            if (!File.Exists(_userCredentialsPath))
                return;

            // Delete the file otherwise
            File.Delete(_userCredentialsPath);
        }
        public static int? LogInWithCookies()
        {
            // If there isn't a text user credential file return null
            int? userId = CheckCookies();
            if (userId is null)
            {
                return null;
            }

            // Return the user id of the logged user
            return userId.Value;
        }
        public static void AddCookies(UserCredentials userCredentials)
        {
            // Cerates a file and writes the user credential in it
            File.WriteAllText(_userCredentialsPath, JsonSerializer.Serialize(new UserCredentials() { Id = userCredentials.Id, UserName = userCredentials.UserName, HashedPassword = userCredentials.HashedPassword }));
        }
    }
}
