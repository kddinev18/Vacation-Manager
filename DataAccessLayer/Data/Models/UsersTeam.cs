using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class UsersTeam
    {
        public int UserTeamId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
