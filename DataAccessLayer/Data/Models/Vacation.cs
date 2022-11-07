using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Data.Model
{
    public partial class Vacation
    {
        public int VacationId { get; set; }
        public int UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime Published { get; set; }
        public bool Approoved { get; set; }

        public virtual User User { get; set; }
    }
}
