using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
# nullable disable
namespace Vacation_Manager.Models
{
    public class VacationInformation
    {
        public int VacationId { get; set; }
        public string UserName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Approoved { get; set; }
        public byte[] Image { get; set; }

        public Brush BgColor { get; set; }
        public string Initials { get; set; }

        public bool EditButton { get; set; } = false;
    }
}
