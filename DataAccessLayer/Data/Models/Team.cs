using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class Team
    {
        public Team()
        {
            ProjectTeams = new HashSet<ProjectTeam>();
            UsersTeams = new HashSet<UsersTeam>();
        }

        public int TeamId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }
        public virtual ICollection<UsersTeam> UsersTeams { get; set; }
    }
}
