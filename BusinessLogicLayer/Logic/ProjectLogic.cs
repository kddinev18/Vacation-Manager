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
        public static void AddProject(string name, string description, VacationManagerContext dbContext)
        {
            dbContext.Projects.Add(new Project() 
            { 
                Description = description, Name = name 
            });
            dbContext.SaveChanges();
        }

        public static IEnumerable<Project> GetProjects(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            if(UserLogic.CheckAuthorisation(userId, dbContext))
            {
                return dbContext.Projects.Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                return null;
            }
        }

        public static int GetProjectsCount(int userId, VacationManagerContext dbContext)
        {
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                return dbContext.Projects.Count();
            }
            else
            {
                return 0;
            }
        }

        public static void EditProject(int projectId, string name, string description, VacationManagerContext dbContext)
        {
            Project project = dbContext.Projects.Where(project=>project.ProjectId == projectId).First();
            project.Name = name;
            project.Description = description;
            dbContext.SaveChanges();
        }

        public static void RemoveProject(int projectId, VacationManagerContext dbContext)
        {
            dbContext.Projects.Remove(dbContext.Projects.Where(project => project.ProjectId == projectId).First());
            dbContext.SaveChanges();
        }
    }
}
