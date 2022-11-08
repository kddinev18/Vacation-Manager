using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public static class RoleLogic
    {
        public static Role AddRole(string roleIdentificator, VacationManagerContext dbContext)
        {
            // Gets the role with the requested role identificator
            Role role = dbContext.Roles.Where(role => role.RoleIdentificator == roleIdentificator).FirstOrDefault();
            // If there isn't any role with the current role indentificator, add it
            if (role == null)
            {
                // Set the role identificator
                role = new Role() { RoleIdentificator = roleIdentificator };
                // Add the role to the current context
                dbContext.Roles.Add(role);
                // Add the role from the current context to the database
                dbContext.SaveChanges();

                // returns the role
                return role;
            }
            // If there is a role with the current tole identificator, return it
            else
            {
                // returns the role
                return role;
            }
        }
    }
}
