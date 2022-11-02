using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class User
    {
        public User()
        {
            UsersTeams = new HashSet<UsersTeam>();
            Vacations = new HashSet<Vacation>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<UsersTeam> UsersTeams { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}