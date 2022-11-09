using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public static class TeamLogic
    {
        public static void AddTeam(string teamName, string[] users, string projectName, VacationManagerContext dbContext)
        {
            VacationManagerContext nestedDbContext = new VacationManagerContext();
            Project project = nestedDbContext.Projects.Where(project => project.Name == projectName).FirstOrDefault();
            if(project == null)
            {
                project = new Project();
                project.Name = projectName;
                project.Description = "Empty";
                nestedDbContext.Projects.Add(project);
                nestedDbContext.SaveChanges();
            }
            Team newTeam = new Team()
            {
                Name = teamName,
                Project = project,
            };
            nestedDbContext.Teams.Add(newTeam);
            nestedDbContext.SaveChanges();
            foreach (string userName in users)
            {
                UserTeamLogic.AddUserTeam(dbContext.Users.Where(user=>user.UserName == userName).First(), newTeam, nestedDbContext);
            }
            nestedDbContext.SaveChanges();
        }
    }
}
