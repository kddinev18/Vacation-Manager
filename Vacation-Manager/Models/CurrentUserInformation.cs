using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_Manager.Models
{
    // POCO class used for stroring the current user id
    public static class CurrentUserInformation
    {
        public static int? CurrentUserId { get; set; }
        public static int? RoleIdentificator { get; set; }
    }
}
