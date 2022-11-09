using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public static class UserTeamLogic
    {
        public static void AddUserTeam(User user, Team team, VacationManagerContext dbContext)
        {
            dbContext.UsersTeams.Add(
                new UsersTeam() 
                { 
                    TeamId = team.TeamId, 
                    UserId = user.UserId
                }
            );
        }
    }
}
