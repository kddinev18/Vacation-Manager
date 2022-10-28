using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public int RoleIdentifier { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
