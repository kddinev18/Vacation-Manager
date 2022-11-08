using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vacation_Manager.Models
{
    public class UserInformation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleIdentificator { get; set; }

        public Brush BgColor { get; set; }
        public string Initials { get; set; }

        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;

    }
}
