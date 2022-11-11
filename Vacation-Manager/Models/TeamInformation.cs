using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
#nullable disable

namespace Vacation_Manager.Models
{
    public class TeamInformation
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Members { get; set; }
        public string ProjectName { get; set; }

        public Brush BgColor { get; set; }
        public string Initials { get; set; }

        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;

        public Brush ProjectBgColor { get; set; }
        public string ProjectInitials { get; set; }
    }
}
