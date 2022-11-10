using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public static class ProjectLogic
    {
        // Adding a new project
        public static void AddProject(string name, string description, VacationManagerContext dbContext)
        {
            // Add the new project to the current context
            dbContext.Projects.Add(new Project() 
            { 
                Description = description, Name = name 
            });
            // Save the changes from the current context into the database
            dbContext.SaveChanges();
        }

        // Get projects
        public static IEnumerable<Project> GetProjects(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            // Checks if the user is admin
            if(UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If the user is admin
                // Return all the prjects skipping the already taken and taking only the amount equal to the paging size
                return dbContext.Projects.Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                // If the user isn't admin
                // Get all the teams' ids that the current user belongs in
                IEnumerable<int> teamsId = dbContext.UsersTeams
                    // Getting the rows where the user id is the same with the current user
                    .Where(userTeam => userTeam.UserId == userId)
                    // Selectiong only the team id
                    .Select(userTeam => userTeam.TeamId);
                // Get the projects only that the current user is working on skipping the already taken and taking only the amount equal to the paging size
                return dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Select(team=>team.Project).Skip(skipAmount).Take(pagingSize);
            }
        }

        // Gets the count of all projects in the database
        public static int GetProjectsCount(int userId, VacationManagerContext dbContext)
        {
            // Checks if the user is admin
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If the user is admin
                // Return the count of all projects
                return dbContext.Projects.Count();
            }
            else
            {
                // If the user isn't admin
                // Get all the teams' ids that the current user belongs in
                IEnumerable<int> teamsId = dbContext.UsersTeams
                    // Getting the rows where the user id is the same with the current user
                    .Where(userTeam => userTeam.UserId == userId)
                    // Selectiong only the team id
                    .Select(userTeam => userTeam.TeamId);
                // Get the count of projects only that the current user is working on skipping the already taken and taking only the amount equal to the paging size
                return dbContext.Teams.Where(team => teamsId.Contains(team.TeamId)).Select(team => team.Project).Count();
            }
        }

        // Edit a project
        public static void EditProject(int projectId, string name, string description, VacationManagerContext dbContext)
        {
            // Retrieve the desired project
            Project project = dbContext.Projects
                // Getting the rows where the project id matches with that we want to edit
                .Where(project=>project.ProjectId == projectId)
                // Taking the first element
                .First();
            // Setting the new name
            project.Name = name;
            // Setting the new description
            project.Description = description;
            // Save the changes from the current context into the database
            dbContext.SaveChanges();
        }

        // Remove a project
        public static void RemoveProject(int projectId, VacationManagerContext dbContext)
        {
            // Remove the first project that has equal id with the project we want to remove
            dbContext.Projects.Remove(dbContext.Projects.Where(project => project.ProjectId == projectId).First());
            // Save the changes from the current context into the database
            dbContext.SaveChanges();
        }
    }
}
