using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Data.Model
{
    public partial class Project
    {
        public Project()
        {
            Teams = new HashSet<Team>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
