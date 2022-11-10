using BusinessLogicLayer;
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
            int userId = UserLogic.Register(userName, email, password, dbContext);

            return userId;
        }

        public static string LogIn(string userName, string password, VacationManagerContext dbContext)
        {
            string serializedResponse = JsonSerializer.Serialize(UserLogic.LogIn(userName, password, dbContext));

            return serializedResponse;
        }

        public static int LogInWithCookies(string userName, string password, VacationManagerContext dbContext)
        {
            int userId = UserLogic.LogInWithPreHashedPassword(userName, password, dbContext);

            return userId;
        }

        public static void RegisterMember(string userName, string email, string password, string roleIdentificator, VacationManagerContext dbContext)
        {
            UserLogic.RegisterMember(userName, email, password, roleIdentificator, dbContext);
        }

        public static string GetUsers(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            return JsonSerializer.Serialize(UserLogic.GetUsers(userId, pagingSize, skipAmount, dbContext));
        }

        public static int GetUserCount(VacationManagerContext dbContext)
        {
            return UserLogic.GetUserCount(dbContext);
        }

        public static void RemoveUser(int userId, VacationManagerContext dbContext)
        {
            UserLogic.RemoveUser(userId, dbContext);
        }
        public static void EditUser(int userId, string email, string role, VacationManagerContext dbContext)
        {
            UserLogic.EditUser(userId, email, role, dbContext);
        }
        public static bool CheckAuthentication(int userId, VacationManagerContext dbContext)
        {
            return UserLogic.CheckAuthorisation(userId, dbContext);
        }
        public static void AddTeam(string teamName, string[] users, string projectName, VacationManagerContext dbContext)
        {
            TeamLogic.AddTeam(teamName, users, projectName, dbContext);
        }
        public static void AddProject(string name, string description, VacationManagerContext dbContext)
        {
            ProjectLogic.AddProject(name, description, dbContext);
        }
        public static string GetProjects(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            return JsonSerializer.Serialize(ProjectLogic.GetProjects(userId, pagingSize, skipAmount, dbContext));
        }
        public static int GetProjectCount(int userId, VacationManagerContext dbContext)
        {
            return ProjectLogic.GetProjectsCount(userId, dbContext);
        }
        public static void EditProject(int projectId, string name, string description, VacationManagerContext dbContext)
        {
            ProjectLogic.EditProject(projectId, name, description, dbContext);
        }
        public static void RemoveProject(int projectId, VacationManagerContext dbContext)
        {
            ProjectLogic.RemoveProject(projectId, dbContext);
        }

        public static string GetTeams(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            return JsonSerializer.Serialize(TeamLogic.GetTeams(userId, pagingSize, skipAmount, dbContext));
        }

        public static int GetTeamsCount(int userId, VacationManagerContext dbContext)
        {
            return TeamLogic.GetTeamsCount(userId, dbContext);
        }
        public static void EditTeam(int teamid, string teamName, string[] users, VacationManagerContext dbContext)
        {
            TeamLogic.EditTeam(teamid, teamName, users, dbContext);
        }
        public static void RemoveTeam(int teamid, VacationManagerContext dbContext)
        {
            TeamLogic.RemoveTeam(teamid, dbContext);
        }

        public static void AddVacation(int userId, DateTime from, DateTime to, string image, VacationManagerContext dbContext)
        {
            string[] stringBytes = image.Split(';');
            byte[] imageBytes = new byte[stringBytes.Length];
            for (int i = 0; i < imageBytes.Length; i++)
            {
                imageBytes[i] = byte.Parse(stringBytes[i]);
            }
            VacationLogic.AddVacation(userId, from, to, imageBytes, dbContext);
        }
        public static string GetVacations(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            return JsonSerializer.Serialize(VacationLogic.GetVacations(userId, pagingSize, skipAmount, dbContext));
        }
        public static int GetVacationsCount(int userId, VacationManagerContext dbContext)
        {
            return VacationLogic.GetVacationsCount(userId, dbContext);
        }
        public static void ApprooveVacation(int cationId, VacationManagerContext dbContext)
        {
            VacationLogic.ApprooveVacation(cationId, dbContext);
        }
    }
}
