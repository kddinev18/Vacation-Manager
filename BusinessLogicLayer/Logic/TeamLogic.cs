using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    // POCO class userd for transferring information about a team
    public class TeamInformation
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Members { get; set; }
        public string ProjectName { get; set; }
    }
    public static class TeamLogic
    {
        // Adds a team
        public static void AddTeam(string teamName, string[] users, string projectName, VacationManagerContext dbContext)
        {
            // Get the project with the same name we want the team to be working on
            Project project = dbContext.Projects
                // Gets all the projects with name maching the name we want out project to be
                .Where(project => project.Name == projectName)
                // Get the first element if there is one, otherwise return null
                .FirstOrDefault();

            // if there isn't a rpoject with that name create one
            if (project == null)
            {
                // Instantiate a new project
                project = new Project();
                // Set the name of the newly added project
                project.Name = projectName;
                // Set the description of the newly added project to "Empty"
                project.Description = "Empty";
                // Add the project to the context
                dbContext.Projects.Add(project);
                // Save the canges made to the contect into the database
                dbContext.SaveChanges();
            }
            // Instantiate a new team and assign its name and the project they are going to be working on
            Team newTeam = new Team()
            {
                Name = teamName,
                Project = project,
            };
            // Add the team to the context
            dbContext.Teams.Add(newTeam);
            // Save the canges made to the contect into the database
            dbContext.SaveChanges();

            // For each user in the team add a new row into the UsersTeam linking table
            foreach (string userName in users)
            {
                // Gets the user maching the username and the team
                UserTeamLogic.AddUserTeam(dbContext.Users.Where(user => user.UserName == userName).First(), newTeam, dbContext);
            }
            // Save the canges made to the contect into the database
            dbContext.SaveChanges();
        }

        // Gets teams
        public static IEnumerable<TeamInformation> GetTeams(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            // Instantiating a nested context so that we can use it for separate request to the database
            VacationManagerContext nestedDbContext = new VacationManagerContext();
            IEnumerable<Team> teams;
            ICollection<TeamInformation> teamsInformation = new List<TeamInformation>();
            // Check is the user is admin
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If user is admin
                // Take the teams skipping the amount of already view teams and taking the amount equal to the paging size
                teams = dbContext.Teams.Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                // If user isn't admin
                // Get the teams' ids that the current user is working in
                IEnumerable<int> teamsId = dbContext.UsersTeams
                    // Gets the userTeasm which user id is equal to the current user id
                    .Where(userTeam => userTeam.UserId == userId)
                    // Select only the team id
                    .Select(userTeam => userTeam.TeamId);
                // Getting the teams that the current user is working in skipping the amount of already view teams and taking the amount equal to the paging size
                teams = dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Skip(skipAmount).Take(pagingSize);
            }
            // Arrange the information on the POCO class
            foreach (Team team in teams)
            {
                // Add a new element of TeamInformation for every team in the collection teams
                teamsInformation.Add(new TeamInformation()
                {
                    TeamId = team.TeamId,
                    Name = team.Name,
                    // using the nested context for getting the project name
                    ProjectName = nestedDbContext.Projects
                    // Get the project that matches the project id of the project the team is working on
                    .Where(project => project.ProjectId == team.ProjectId)
                    // Get only the first element
                    .First()
                    // Get the name of the project
                    .Name,
                    // Get the members' names and join them usgin the separator ';'
                    Members = String.Join(';', 
                    nestedDbContext.UsersTeams
                    // Get the userTeam with maching team id
                    .Where(userTeam => userTeam.TeamId == team.TeamId)
                    // Select only the username
                    .Select(userTeam => userTeam.User.UserName)),
                });
            }
            // Return the teams information
            return teamsInformation;
        }

        // Gets the count of the teams in the databse
        public static int GetTeamsCount(int userId, VacationManagerContext dbContext)
        {
            // Check is the user is admin
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If user is admin
                // Return the count of all teams in the database
                return dbContext.Teams.Count();
            }
            else
            {
                // If user isn't admin
                // Ge
                // Get the teams' ids that the current user is working in
                IEnumerable<int> teamsId = dbContext.UsersTeams
                    // Gets the userTeasm which user id is equal to the current user id
                    .Where(userTeam => userTeam.UserId == userId)
                    // Select only the team id
                    .Select(userTeam => userTeam.TeamId);
                // Getting the count of teams that the current user is working in skipping the amount of already view teams and taking the amount equal to the paging size
                return dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Count();
            }
        }

        // Edits a team
        public static void EditTeam(int teamId, string teamName, string[] users, VacationManagerContext dbContext)
        {
            // Instantiating a nested context so that we can use it for separate request to the database
            VacationManagerContext nesteDbContext = new VacationManagerContext();
            // Gets the team we want ot edit
            Team team = dbContext.Teams
                // Gets the teams which id is the same with the id of the team we want to edit
                .Where(team => team.TeamId == teamId)
                // Takes the firts element
                .First();
            // Enter the new name of the team
            team.Name = teamName;
            foreach (string userName in users)
            {
                // get the id of the user with the same username that is described to beworking on the team
                int userId = nesteDbContext.Users.Where(user=>user.UserName == userName).First().UserId;
                // Check if the user is actually working in the team
                if (nesteDbContext.UsersTeams.Where(userTeam => userTeam.UserId == userId && userTeam.TeamId == teamId).FirstOrDefault() == null)
                {
                    // If not add it
                    UserTeamLogic.AddUserTeam(nesteDbContext.Users.Where(user => user.UserName == userName).First(), team, nesteDbContext);
                }
            }
            // Save the canges made to the nested contect into the database
            nesteDbContext.SaveChanges();
            // Save the canges made to the contect into the database
            dbContext.SaveChanges();
        }

        // Remove a team
        public static void RemoveTeam(int teamId, VacationManagerContext dbContext)
        {
            // Instantiating a nested context so that we can use it for separate request to the database
            VacationManagerContext nesteDbContext = new VacationManagerContext();
            // Removes all the rows in the linking table UsersTeam
            UserTeamLogic.RemoveTeam(teamId, nesteDbContext);
            // Remove the desired team
            dbContext.Teams.Remove(
                dbContext.Teams
                // Gets the teams with maching id od the team we want to delete
                .Where(team => team.TeamId == teamId)
                // Gets the first element
                .First()
            );
            // Save the canges made to the nested contect into the database
            nesteDbContext.SaveChanges();
            // Save the canges made to the contect into the database
            dbContext.SaveChanges();
        }
    }
}
