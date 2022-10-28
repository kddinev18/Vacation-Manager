using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class Project
    {
        public Project()
        {
            ProjectTeams = new HashSet<ProjectTeam>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }
    }
}
