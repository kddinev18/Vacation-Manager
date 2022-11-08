using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Data.Model
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleIdentificator { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
