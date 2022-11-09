using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public class TeamInformation
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Members { get; set; }
        public string ProjectName { get; set; }
    }
    public static class TeamLogic
    {
        public static void AddTeam(string teamName, string[] users, string projectName, VacationManagerContext dbContext)
        {
            Project project = dbContext.Projects.Where(project => project.Name == projectName).FirstOrDefault();
            if (project == null)
            {
                project = new Project();
                project.Name = projectName;
                project.Description = "Empty";
                dbContext.Projects.Add(project);
                dbContext.SaveChanges();
            }
            Team newTeam = new Team()
            {
                Name = teamName,
                Project = project,
            };
            dbContext.Teams.Add(newTeam);
            dbContext.SaveChanges();
            foreach (string userName in users)
            {
                UserTeamLogic.AddUserTeam(dbContext.Users.Where(user => user.UserName == userName).First(), newTeam, dbContext);
            }
            dbContext.SaveChanges();
        }

        public static IEnumerable<TeamInformation> GetTeams(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            VacationManagerContext nestedDbContext = new VacationManagerContext();
            IEnumerable<Team> teams;
            ICollection<TeamInformation> teamsInformation = new List<TeamInformation>();
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                teams = dbContext.Teams.Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                IEnumerable<int> teamsId = dbContext.UsersTeams.Where(userTeam => userTeam.UserId == userId).Select(userTeam => userTeam.TeamId);
                teams = dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Skip(skipAmount).Take(pagingSize);
            }
            foreach (Team team in teams)
            {
                teamsInformation.Add(new TeamInformation()
                {
                    TeamId = team.TeamId,
                    Name = team.Name,
                    ProjectName = nestedDbContext.Projects.Where(project => project.ProjectId == team.ProjectId).First().Name,
                    Members = String.Join(';', nestedDbContext.UsersTeams.Where(userTeam => userTeam.TeamId == team.TeamId).Select(userTeam => userTeam.User.UserName)),
                });
            }
            return teamsInformation;
        }

        public static int GetTeamsCount(int userId, VacationManagerContext dbContext)
        {
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                return dbContext.Teams.Count();
            }
            else
            {
                IEnumerable<int> teamsId = dbContext.UsersTeams.Where(userTeam => userTeam.UserId == userId).Select(userTeam => userTeam.TeamId);
                return dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Count();
            }
        }

        public static void EditTeam(int teamId, string teamName, string[] users, VacationManagerContext dbContext)
        {
            VacationManagerContext nesteDbContext = new VacationManagerContext();
            Team team = dbContext.Teams.Where(team => team.TeamId == teamId).First();
            team.Name = teamName;
            foreach (string userName in users)
            {
                int userId = nesteDbContext.Users.Where(user=>user.UserName == userName).First().UserId;
                if (nesteDbContext.UsersTeams.Where(userTeam => userTeam.UserId == userId && userTeam.TeamId == teamId).FirstOrDefault() == null)
                {
                    UserTeamLogic.AddUserTeam(nesteDbContext.Users.Where(user => user.UserName == userName).First(), team, nesteDbContext);
                }
            }
            nesteDbContext.SaveChanges();
            dbContext.SaveChanges();
        }

        public static void RemoveTeam(int teamId, VacationManagerContext dbContext)
        {
            VacationManagerContext nesteDbContext = new VacationManagerContext();
            UserTeamLogic.RemoveTeam(teamId, nesteDbContext);
            dbContext.Teams.Remove(dbContext.Teams.Where(team => team.TeamId == teamId).First());
            nesteDbContext.SaveChanges();
            dbContext.SaveChanges();
        }
    }
}
