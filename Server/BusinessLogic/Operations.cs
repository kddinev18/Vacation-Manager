﻿using BusinessLogicLayer;
using BusinessLogicLayer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using DataAccessLayer;

namespace Server
{
    public static class Operations
    {
        public static int Register(string userName, string email, string password, VacationManagerContext dbContext)
        {
            // Register the user and get the id of the newly registered user
            int userId = UserLogic.Register(userName, email, password, dbContext);

            // Log the operation
            Logger.WriteData(2, "Information", $"Register - UserId: {userId}, UserName: {userName}");

            // Return the user id
            return userId;
        }

        public static string LogIn(string userName, string password, VacationManagerContext dbContext)
        {
            // Log in and get sereialisd UserCredentials
            string serializedResponse = JsonSerializer.Serialize(UserLogic.LogIn(userName, password, dbContext));
            // Log the operation
            Logger.WriteData(2, "Information", "LogIn");

            // Return the serialized data
            return serializedResponse;
        }

        public static int LogInWithCookies(string userName, string password, VacationManagerContext dbContext)
        {
            // Log in the user and get the id of the user
            int userId = UserLogic.LogInWithPreHashedPassword(userName, password, dbContext);
            // Log the operation
            Logger.WriteData(2, "Information", "LogIn");

            // Return the user id
            return userId;
        }

        public static void RegisterMember(string userName, string email, string password, string roleIdentificator, VacationManagerContext dbContext)
        {
            UserLogic.RegisterMember(userName, email, password, roleIdentificator, dbContext);
        }
    }
}
