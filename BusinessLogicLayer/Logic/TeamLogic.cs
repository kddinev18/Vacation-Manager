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
        public static void AddTeam(string teamName, ICollection<User> users, VacationManagerContext dbContext)
        {
            Team newTeam = new Team()
            {
                Name = teamName
            };
            dbContext.Teams.Add(newTeam);
            dbContext.SaveChanges();
            foreach (User user in users)
            {
                UserTeamLogic.AddUserTeam(user, newTeam, dbContext);
            }
            dbContext.SaveChanges();
        }
    }
}
