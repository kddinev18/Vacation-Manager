using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class ProjectTeam
    {
        public int ProjectTeamId { get; set; }
        public int ProjectId { get; set; }
        public int TeamId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Team Team { get; set; }
    }
}
