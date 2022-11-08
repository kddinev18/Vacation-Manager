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
        public static void AddTeam(string teamName, string[] users, VacationManagerContext dbContext)
        {
            Team newTeam = new Team()
            {
                Name = teamName
            };
            dbContext.Teams.Add(newTeam);
            dbContext.SaveChanges();
            foreach (string userName in users)
            {
                UserTeamLogic.AddUserTeam(dbContext.Users.Where(user=>user.UserName == userName).First(), newTeam, dbContext);
            }
            dbContext.SaveChanges();
        }
    }
}
