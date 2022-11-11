using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
#nullable disable
namespace Vacation_Manager.Models
{
    public class ProjectInformation
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Brush BgColor { get; set; }
        public string Initials { get; set; }

        public bool EditButton { get; set; } = false;
        public bool RemoveButton { get; set; } = false;
    }
}
