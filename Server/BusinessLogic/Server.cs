﻿using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Server
{
    public class Server
    {
        private static TcpListener _tcpListener;
        private static List<TcpClient> _clients = new List<TcpClient>();
        //private static Dictionary<TcpClient, VacationManagerContext> _dbContexts = new Dictionary<TcpClient, VacationManagerContext>();
        // Buffer
        private static byte[] _data = new byte[16777216];
        private int _port;
        private static int _success = 0;
        private static int _error = 1;

        public Server(int port)
        {
            _port = port;
        }

        public void ServerSertUp()
        {
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            // Starts the server
            _tcpListener.Start();
            // Starts accepting clients
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public void ServerShutDown()
        {
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

                //_dbContexts.Add(client, new VacationManagerContext());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Add the client newly connect client into the _clients list
            _clients.Add(client);
            // Begin recieving bytes from the client
            client.Client.BeginReceive(_data, 0, _data.Length, SocketFlags.None, new AsyncCallback(ReciveUserInput), client);
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public static void ReciveUserInput(IAsyncResult asyncResult)
        {
            TcpClient client = asyncResult.AsyncState as TcpClient;
            int reciever;
            List<string> args;

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
                try
                {
                    args = data.Split('|')[1].Split(", ").ToList();
                }
                catch (Exception)
                {
                    args = null;
                }
                SendCorrenspodingResponse(client, int.Parse(data.Split('|')[0]), args);
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
            //_dbContexts.Remove(client);
            _clients.Remove(client);
        }

        public static void SendCorrenspodingResponse(TcpClient client, int operationNumber, List<string> args)
        {
            VacationManagerContext vacationManagerContext = new VacationManagerContext();
            UserOperation operation = (UserOperation)operationNumber;
            string response = String.Empty;
            switch (operation)
            {
                case UserOperation.Register:
                    int userId = Operations.Register(args[0], args[1], args[2], vacationManagerContext);
                    // Generate response
                    response = $"{_success}|{userId}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.LogIn:
                    // Generate response
                    response = $"{_success}|{Operations.LogIn(args[0], args[1], vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.LogInWithCookies:
                    // Generate response
                    response = $"{_success}|{Operations.LogInWithCookies(args[0], args[1], vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.RegisterMember:
                    Operations.RegisterMember(args[0], args[1], args[2], args[3], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetUsers:
                    // Generate response
                    response = $"{_success}|{Operations.GetUsers(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetUserCount:
                    // Generate response
                    response = $"{_success}|{Operations.GetUserCount(vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.RemoveUser:
                    Operations.RemoveUser(int.Parse(args[0]), vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.EditUser:
                    Operations.EditUser(int.Parse(args[0]), args[1], args[2], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.CheckAuthentication:
                    // Generate response
                    response = $"{_success}|{Operations.CheckAuthentication(int.Parse(args[0]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.AddTeam:
                    Operations.AddTeam(args[0], args[1].Split(';'), args[2], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.AddProject:
                    Operations.AddProject(args[0], args[1], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetProjects:
                    // Generate response
                    response = $"{_success}|{Operations.GetProjects(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetProjectCount:
                    // Generate response
                    response = $"{_success}|{Operations.GetProjectCount(int.Parse(args[0]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.EditProject:
                    Operations.EditProject(int.Parse(args[0]), args[1], args[2], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.RemoveProject:
                    Operations.RemoveProject(int.Parse(args[0]), vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetTeams:
                    // Generate response
                    response = $"{_success}|{Operations.GetTeams(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetTeamCount:
                    // Generate response
                    response = $"{_success}|{Operations.GetTeamsCount(int.Parse(args[0]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.EditTeam:
                    Operations.EditTeam(int.Parse(args[0]), args[1], args[2].Split(';'), vacationManagerContext);;
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.RemoveTeam:
                    Operations.RemoveTeam(int.Parse(args[0]), vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.AddVacation:
                    Operations.AddVacation(int.Parse(args[0]), DateTime.Parse(args[1]), DateTime.Parse(args[2]), args[3], vacationManagerContext);
                    // Generate response
                    response = $"{_success}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetVacations:
                    // Generate response
                    response = $"{_success}|{Operations.GetVacations(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetVacationsCount:
                    // Generate response
                    response = $"{_success}|{Operations.GetVacationsCount(int.Parse(args[0]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.ApprooveVacation:
                    Operations.ApprooveVacation(int.Parse(args[0]), vacationManagerContext);
                    // Generate response
                    response = $"{_success}|";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetUserByName:
                    // Generate response
                    response = $"{_success}|{Operations.GetUserByName(args[0], vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetProjectByName:
                    // Generate response
                    response = $"{_success}|{Operations.GetProjectByName(args[0], vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetTeamByName:
                    // Generate response
                    response = $"{_success}|{Operations.GetTeamByName(args[0], vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                case UserOperation.GetCurrentUserInformation:
                    // Generate response
                    response = $"{_success}|{Operations.GetCurrentUserInformation(int.Parse(args[0]), vacationManagerContext)}";
                    // send data to the client
                    client.Client.Send(Encoding.UTF8.GetBytes(response));
                    break;
                default:
                    // If the operation is invalid throw an exception
                    throw new InvalidOperationException("There is no operation called: " + operation.ToString());
            }
        }
    }
}
