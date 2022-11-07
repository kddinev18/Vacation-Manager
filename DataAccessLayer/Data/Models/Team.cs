using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Data.Model
{
    public partial class Team
    {
        public Team()
        {
            UsersTeams = new HashSet<UsersTeam>();
        }

        public int TeamId { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<UsersTeam> UsersTeams { get; set; }
    }
}
